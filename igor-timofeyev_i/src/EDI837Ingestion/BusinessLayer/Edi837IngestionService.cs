using EDI837Ingestion.EF;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837Ingestion.BusinessLayer
{
    public class Edi837IngestionService : IEdi837IngestionService
    {
        private readonly string _filePath;
        private readonly AppDbContext _dbContext;

        public Edi837IngestionService(AppDbContext dbContext, IConfiguration config)
        {
                                                                       //or env variable, or some other default fallback path
            _filePath = config["FilePaths:Edi837PathWithFilename"] ?? "Server=Igor-Surface\\SQLEXPRESS;Database=PayerEDI;Trusted_Connection=True;TrustServerCertificate=True;";
            _dbContext = dbContext;
        }

        public async Task IngestEdi837()
        {
            using var ediStream = File.OpenRead(_filePath);
            using var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa5010");

            //ProfessionalClaimInterchange currentInterchange = null;


            //var count = _dbContext.Payers.Count();
        }
    }
}
