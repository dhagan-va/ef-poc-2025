namespace Edi837Ingester.Services;

public interface IEdiParserService
{
    Task Parse(string filePath);
}