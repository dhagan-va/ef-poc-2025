using EdiFabric.Templates.Hipaa5010;

namespace EDI837.src.Services
{
    public interface IEdi837FileService
    {
        public IEnumerable<TS837P> ExtractValid837PTransactions(Stream stream);
    }
}
