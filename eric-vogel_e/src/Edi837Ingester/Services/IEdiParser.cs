namespace Edi837Ingester.Services;

public interface IEdiParser
{
    Task Parse(string filePath);
}