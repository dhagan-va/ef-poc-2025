namespace Deepika.EDIIngestion.Services
{
    public interface IEdiIngestionService
    {
        void ProcessFolder(string folderPath);
        System.Threading.Tasks.Task ProcessS3BucketAsync(string bucket, int degreeOfParallelism = 4);
    }
}
