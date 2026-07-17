using System.Diagnostics;
using Edi837Ingestion.Configuration;
using Microsoft.Extensions.Configuration;

namespace Edi837Ingestion.LicenseWarmer;

/// <summary>
/// Applies the EdiFabric license reliably from inside a test host. EdiFabric persists/validates its
/// license token through .NET IsolatedStorage: a test host (Rider's runner or VSTest) can <em>read</em>
/// that cache but cannot reliably perform the first-time token <em>write</em> — that write surfaces as
/// "the serial key is invalid" the first time a cold cache is hit. A normal console process writes it
/// fine. So this type applies the license in-process when the cache is already warm (the common case),
/// and otherwise warms it in a short-lived child console process and then reads it.
/// </summary>
/// <remarks>
/// One class, two roles: <see cref="Main"/> is the console entry point (the child process the fixtures
/// spawn, and what <c>dotnet run</c> executes); <see cref="EnsureLicensed"/> is what the test fixtures
/// call in the test host. Both resolve the serial key from the shared user-secrets store (dev) or the
/// <c>EdiFabric__SerialKey</c> environment variable (CI).
/// </remarks>
public static class LicenseWarmer
{
    /// <summary>
    /// Console entry point: applies the license from this writable context, warming EdiFabric's token
    /// cache. Returns 0 on success so a spawning caller can proceed, or 1 with the reason on stderr.
    /// </summary>
    public static int Main()
    {
        try
        {
            ApplyLicense();
            Console.WriteLine("EdiFabric license token cache warmed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine("Failed to warm the EdiFabric license token cache: " + exception.Message);
            return 1;
        }
    }

    /// <summary>
    /// Ensures the EdiFabric license is applied in the current (test-host) process. Tries the in-process
    /// apply first — a no-op-fast read when the cache is warm — and, if that fails because the cache is
    /// cold and the host cannot write it, warms the cache in a child console process and reads it.
    /// </summary>
    public static void EnsureLicensed()
    {
        try
        {
            ApplyLicense();
        }
        catch
        {
            WarmInChildProcess();
            ApplyLicense();
        }
    }

    private static void ApplyLicense() =>
        EdiFabricLicense.Apply(BuildConfiguration().GetEdiFabricOptions());

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddUserSecrets(typeof(LicenseWarmer).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

    // Spawn this same project as a child process (a normal console context) to perform the token write,
    // then let the caller re-read the now-warm cache. Throws with the child's output if it fails, so a
    // genuine licensing/network problem still surfaces loudly rather than as a silent miss.
    private static void WarmInChildProcess()
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(LocateWarmerProject());
        startInfo.ArgumentList.Add("--configuration");
        startInfo.ArgumentList.Add(BuildConfigurationName);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Could not start the EdiFabric license warmer process.");
        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"The EdiFabric license warmer exited with code {process.ExitCode}. " +
                $"{standardError}{standardOutput}".Trim());
    }

    // The warmer is built in the same configuration as the test project that references it, so the
    // compiled-in value matches and is passed straight through to the child `dotnet run`.
    private const string BuildConfigurationName =
#if DEBUG
        "Debug";
#else
        "Release";
#endif

    // Walk up from the running test assembly's directory to the repo, where the warmer project lives at
    // a fixed relative path — so the fixtures need not know the repo layout.
    private static string LocateWarmerProject()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
             directory is not null;
             directory = directory.Parent)
        {
            var candidate = Path.Combine(
                directory.FullName, "tools", "EdiFabricLicenseWarmer", "EdiFabricLicenseWarmer.csproj");
            if (File.Exists(candidate))
                return candidate;
        }

        throw new InvalidOperationException(
            $"Could not locate EdiFabricLicenseWarmer.csproj by walking up from {AppContext.BaseDirectory}.");
    }
}
