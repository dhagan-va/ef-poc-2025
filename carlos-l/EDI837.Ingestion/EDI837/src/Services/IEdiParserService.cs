using EdiFabric.Templates.Hipaa5010;

namespace EDI837.src.Services
{
    public interface IEdiParserService
    {
        public IEnumerable<TS837P> ExtractValid837PTransactions(Stream stream, IEnumerable<string> parsingErrors);
        public Stream GetStreamByFileName(string fileName);
    }
}
