namespace X12EDI837Ingestion.Producer.Configuration;

public sealed class FileInformation
{
    public string FilePath { get; set; } = string.Empty;
    public int WorkerCount { get; set; }
    public int ChannelCapacity { get; set; }
}