using System.Runtime.CompilerServices;

namespace integration;

/// <summary>
/// Selects the <c>Testing</c> environment for the whole test assembly before any fixture builds
/// configuration, so <see cref="Edi837Ingestion.Configuration.AppConfiguration.Build"/> layers the
/// top-level <c>appsettings.Testing.json</c> (the SQL container password and fast SQS polling) over the
/// shared base <c>appsettings.json</c>. A <see cref="ModuleInitializerAttribute"/> runs before any type
/// in this assembly is touched — i.e. before the collection fixtures' constructors call
/// <c>AppConfiguration.Build()</c>. Kept in the test project so no test-only configuration or code
/// leaks into the application.
/// </summary>
internal static class TestEnvironment
{
    [ModuleInitializer]
    internal static void UseTestingEnvironment() =>
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Testing");
}
