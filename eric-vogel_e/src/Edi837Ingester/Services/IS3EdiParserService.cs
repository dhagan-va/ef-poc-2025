using EdiFabric.Core.Model.Edi;

namespace Edi837Ingester.Services
{
    public interface IS3EdiParserService
    {
        Task ParseFromS3Async(string bucketName, string fileName, ValidationLevel validationLevel);
    }
}