using EdiFabric.Core.Model.Edi;

namespace Edi837Ingester.Services;

public interface IEdiParserService
{
    Task Parse(string filePath, ValidationLevel validationLevel);
    Task Parse(Stream stream, ValidationLevel validationLevel);
}