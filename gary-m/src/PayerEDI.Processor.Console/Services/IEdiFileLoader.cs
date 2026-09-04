namespace PayerEDI.Processor.Console.Services;

public interface IEdiFileLoader
{
    public Task<Stream> OpenStreamAsync(string ediLocation);
}
