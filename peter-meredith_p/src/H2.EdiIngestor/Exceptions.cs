namespace H2.EdiIngestor;

public class InvalidOptionsException : Exception
{
    public InvalidOptionsException() { }

    public InvalidOptionsException(string? message)
        : base(message) { }
}

public class InvalidX12NFileException : Exception
{
    public string FileName { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public InvalidX12NFileException(string fileName, IEnumerable<string> errors)
        : base(CreateErrorMessage(fileName, errors))
    {
        this.FileName = fileName;
        this.Errors = errors;
    }

    private static string? CreateErrorMessage(string fileName, IEnumerable<string> errors)
    {
        return $"Errors encountered in file: {fileName} {Environment.NewLine} {string.Join(Environment.NewLine, errors)}";
    }
}
