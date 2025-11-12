namespace EDI837.src.Services
{
    public interface IEdiParserService
    {
        public Stream GetStreamByFileName(string fileName);
    }
}
