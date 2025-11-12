using EDI837.src.Models;
using Microsoft.Extensions.FileProviders;

namespace EDI837.src.Services
{
    public class EdiParserService : IEdiParserService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger _logger;

        public EdiParserService(IConfiguration configuration, IFileProvider fileProvider, ILogger logger)
        {
            this._configuration = configuration;
            this._fileProvider = fileProvider;
            _logger = logger;
        }

        public Stream GetStreamByFileName(string fileName)
        {
            var fileInfo = this._fileProvider.GetFileInfo($"{_configuration["LocalFileFolder"]}\\{fileName}");
            this._logger.LogInformation(fileInfo.Exists ? $"{fileInfo.Name} does exist." : $"{fileInfo.Name} does not exist.");

            if (fileInfo.Exists)
            {
                return fileInfo.CreateReadStream();
            }

            return Stream.Null;
        }
    }
}
