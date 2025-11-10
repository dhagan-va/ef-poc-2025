using EDI837.src.Models;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;

namespace EDI837.src.Services
{
    public class Edi837FileService : IEdi837FileService
    {
        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;

        public Edi837FileService(AppDataContext context, IConfiguration configuration, IFileProvider fileProvider)
        {
            this._context = context;
            this._configuration = configuration;
            this._fileProvider = fileProvider;
        }

        
        public void GetFileByName(string fileName)
        {
            var fileInfo = this._fileProvider.GetFileInfo($"{_configuration["LocalFileFolder"]}\\{fileName}");
            if (fileInfo.Exists)
            {

                var fileStream = fileInfo.CreateReadStream();
            
                List<IEdiItem> ediItems;
                using (var ediReader = new X12Reader(fileStream, "EdiFabric.Templates.Hipaa"))
                {
                    ediItems = ediReader.ReadToEnd().ToList();
                }


                //var transactions = ediItems.OfType<TS837P>();

                //foreach (var transaction in transactions)
                //{
                //    if (transaction.HasErrors)
                //    {
                //        //  partially parsed
                //        var errors = transaction.ErrorContext.Flatten();
                //    }
                //}
            }
            else
            {
                throw new Exception($"File {fileName} was not found.");
            }
                
        }
    }
}
