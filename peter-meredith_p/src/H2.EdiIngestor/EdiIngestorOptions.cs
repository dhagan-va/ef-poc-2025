namespace H2.EdiIngestor;

public class EdiTestOptions
{
    public string FileName { get; set; }

    public EdiTestOptions(string[] args)
    {
        if (args == null || args.Length == 0)
            throw new InvalidOptionsException("A file argument is required");
        FileName = args.First();
    }
}
