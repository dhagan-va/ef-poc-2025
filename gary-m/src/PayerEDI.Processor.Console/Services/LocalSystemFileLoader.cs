namespace PayerEDI.Processor.Console.Services;

public class LocalSystemFileLoader : IEdiFileLoader
{
    public Task<Stream> OpenStreamAsync(string ediLocation) =>
        GetFilePath(ediLocation) is var filePath
        && File.Exists(filePath)
            ? Task.FromResult<Stream>(File.OpenRead(filePath))
            : Task.FromException<Stream>(new FileNotFoundException($"File {ediLocation} not found"));

    private static string GetFilePath(string ediLocation) =>
        ediLocation.StartsWith("file:///", StringComparison.OrdinalIgnoreCase)
            ? new Uri(ediLocation, UriKind.Absolute).LocalPath
            : ediLocation;
}
