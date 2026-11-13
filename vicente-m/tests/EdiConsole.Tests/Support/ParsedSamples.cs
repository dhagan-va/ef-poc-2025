using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace EdiConsole.Tests.Support;

/// <summary>
/// Parses each sample once and shares the result with every test in the class (an xUnit class fixture).
/// Each parse spends EdiFabric operations, so a full test run costs two, not one per test.
/// </summary>
public sealed class ParsedSamples
{
    /// <summary><c>837p-sample.edi</c>: two claims under 2000B; each subscriber is the patient.</summary>
    public TS837P SubscriberIsPatient { get; } = ReadSingleTransaction("837p-sample.edi");

    /// <summary><c>837-sample-file-2.edi</c>: one claim under 2000C; the patient is a dependent.</summary>
    public TS837P DependentIsPatient { get; } = ReadSingleTransaction("837-sample-file-2.edi");

    private static TS837P ReadSingleTransaction(string fileName)
    {
        TestLicense.EnsureSet();

        // The samples are linked into the test output's "samples" folder (see the .csproj).
        var path = Path.Combine(AppContext.BaseDirectory, "samples", fileName);
        using var stream = File.OpenRead(path);
        using var reader = new X12Reader(stream, "EdiFabric.Templates.Hipaa", new X12ReaderSettings());

        var transaction = reader.ReadToEnd().OfType<TS837P>().Single();
        if (transaction.HasErrors)
            throw new InvalidDataException($"{fileName} did not parse cleanly, so it can't be used as a mapper test input.");

        return transaction;
    }
}
