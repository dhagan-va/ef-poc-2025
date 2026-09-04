using CommandLine;

namespace PayerEDI.Processor.Console.Command;

public enum EdiFileLocation
{
    S3,
    FileSystem,
}

public class CliOptions
{
    [Value(
        0,
        Required = true,
        HelpText =
            "EDI file location. Use a local path or file:/// URI, or an s3:///bucket/key URI. "
            + "If no URI scheme is provided, the location defaults to the local file system."
    )]
    public required string EdiFile { get; set; }

    [Option(
        "save",
        Required = false,
        Default = false,
        HelpText = "Save processed claims to the database"
    )]
    public bool Save { get; set; }

    public EdiFileLocation GetEdiFileLocation() =>
        EdiFile.StartsWith("s3:///", StringComparison.OrdinalIgnoreCase)
            ? EdiFileLocation.S3
            : EdiFileLocation.FileSystem;
}
