using EDI837IngestionTask.Models;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;


namespace EDI837IngestionTask.Services
{
    public static class ClaimSaver
    {
        private const int BATCH_SIZE = 100;

        /// <summary>
        /// Saves a list of 837P transactions to the database.
        /// check the duplicate records in file and db
        /// </summary>
        public static void Save837P(List<TS837P> trans)
        {
            using var ediDb = new HIPAA_5010_837P_Context();

            var existingKeys = ediDb.ClaimProcesses
                .AsNoTracking()
                .Select(c => new { c.ProviderNPI, c.TransactionControlNumber, c.Tin, c.SubmitterId })
                .ToList()
                .Select(x => (x.ProviderNPI, x.TransactionControlNumber, x.Tin, x.SubmitterId))
                .ToHashSet();

            var seenKeys = new HashSet<(string ProviderNpi, string TransactionNumber, string Tin, string SubmitterId)>();

            int total = 0;
            int skippedDuplicates = 0;
            int skippedInvalid = 0;
            int inserted = 0;

            foreach (var transaction in trans)
            {
                total++;
                var providerNpi = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactIdentifier_09)
                    .FirstOrDefault(npi => !string.IsNullOrWhiteSpace(npi));
                var transactionControlNumber = transaction.ST?.TransactionSetControlNumber_02;

                var submitterId = transaction.BHT_BeginningOfHierarchicalTransaction?.SubmitterTransactionIdentifier_03;
                var claimCreatedDt = transaction.BHT_BeginningOfHierarchicalTransaction?.TransactionSetCreationDate_04;
                var pbsEdiNumber = transaction.AllNM1.Loop1000A?.NM1_SubmitterName?.ResponseContactIdentifier_09;
                var kicEdiNumber = transaction.AllNM1.Loop1000B?.NM1_ReceiverName?.ResponseContactIdentifier_09;
                var providerName = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactLastorOrganizationName_03)
                    .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name));
                var providerZIP = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.N4_BillingProviderCity_State_ZIPCode?.AdditionalPatientInformationContactPostalZoneorZIPCode_03)
                    .FirstOrDefault(zip => !string.IsNullOrWhiteSpace(zip));
                var tin = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.AllREF?.REF_BillingProviderTaxIdentification?.MemberGrouporPolicyNumber_02)
                    .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

                // Validate for non-null
                if (string.IsNullOrWhiteSpace(providerNpi) || string.IsNullOrWhiteSpace(transactionControlNumber) || string.IsNullOrWhiteSpace(tin)|| string.IsNullOrWhiteSpace(submitterId))
                {
                    skippedInvalid++;
                    Console.WriteLine("Skipping claim with missing identifiers.");
                    continue;
                }

                var key = (providerNpi, transactionControlNumber, tin, submitterId);
                // check dup in file
                if (!seenKeys.Add(key))
                {
                    skippedDuplicates++;
                    Console.WriteLine($"Duplicate Records in File: NPI={providerNpi}, ST02={transactionControlNumber}, TIN={tin}, SUBMITTERID={submitterId}");
                    continue;
                }


                if (existingKeys.Contains(key))
                {
                    skippedDuplicates++;
                    Console.WriteLine($"Duplicate Records in File: NPI={providerNpi}, ST02={transactionControlNumber}, TIN={tin}, SUBMITTERID={submitterId}");
                    continue;
                }

                var claim = new ClaimProcess
                {
                    PBSEDINumber = pbsEdiNumber,
                    KICEDINumber = kicEdiNumber,
                    SubmitterId = submitterId,
                    ClaimCreatedDt = claimCreatedDt,
                    ProviderNPI = providerNpi,
                    ProviderName = providerName,
                    ProviderZIP = providerZIP,
                    Tin = tin,
                    TransactionControlNumber = transactionControlNumber,
                    ProcessedAt = DateTime.UtcNow
                };

                ediDb.ClaimProcesses.Add(claim);
                ediDb.TS837P.Add(transaction);
                inserted++;

                if (inserted % BATCH_SIZE == 0)
                {
                    ediDb.SaveChanges();
                    Console.WriteLine($"batch inserted {inserted} records");
                }
            }
            ediDb.SaveChanges();

            Console.WriteLine($"Successfully ingested claim");
            Console.WriteLine($"TOTAL: {total}, INSERTED: {inserted}, DUP SKIP: {skippedDuplicates}, INVALID: {skippedInvalid}");

        }
    }
}