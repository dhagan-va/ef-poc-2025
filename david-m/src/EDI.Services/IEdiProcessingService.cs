using EdiFabric.Templates.Hipaa5010;

namespace EDI.Services
{
    public interface IEdiProcessingService
    {
        Task ProcessEdiAsync(string ediContent);
        Task ProcessEdiDocumentAsync(string ediContent);
        Task ProcessT837DAsync(string ediContent);
        Task ProcessT837IAsync(string ediContent);
        Task ProcessT837PAsync(string ediContent);
        Task ProcessT835Async(string ediContent);
        Task<List<string>> GetEdiFilesFromS3Async(string bucketName);
        Task ProcessEdiFilesFromS3Async(string bucketName);
        // Add other business logic methods as needed
    }
}
