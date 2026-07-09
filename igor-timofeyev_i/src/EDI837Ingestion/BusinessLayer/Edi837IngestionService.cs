using EDI837Ingestion.EF;
using EDI837Ingestion.EF.Entities;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
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
            try
            {
                using (var ediStream = File.OpenRead(_filePath))
                {
                    // Fix 2: Explicitly pass the template namespace as a string
                    using (var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa"))
                    {
                        List<IEdiItem> ediItems = ediReader.ReadToEnd().ToList();

                        // Extract the Professional 837 transaction sets
                        var transactions = ediItems.OfType<TS837P>();

                        foreach (var transaction in transactions)
                        {
                            // Converts the parsed EDI object directly into XML
                            //var xml = transaction.Serialize();

                            // Check if structural or validation errors occurred during parsing
                            if (transaction.HasErrors)
                            {
                                // Flattens the error hierarchy into an easy-to-read list of string messages
                                var errors = transaction.ErrorContext.Flatten();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred while ingesting EDI 837: {ex.Message}");
            }
        }
    }
}
