namespace EdiConsole.Configuration;

public sealed class EdiFabricOptions
{
    public const string SectionName = "EdiFabric";

    public string SerialKey { get; set; } = string.Empty;
}
