using EDI.Data;
using EDI.Services;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<EdiDbContext>(options =>
            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=EDI;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"EDIProcessor\";Command Timeout=0"));
        services.AddSingleton<IEdiValidationService, EdiValidationService>();
        services.AddScoped<IEdiProcessingService, EdiProcessingService>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var service = serviceProvider.GetRequiredService<IEdiProcessingService>();
    var dbContext = serviceProvider.GetRequiredService<EdiDbContext>();

    // Ensure database exists for ad-hoc runs
    dbContext.Database.EnsureCreated();
    // Keep the reporting view available even when migrations aren't applied
    dbContext.Database.ExecuteSqlRaw(
        """
        CREATE OR ALTER VIEW dbo.vw_EdiFileIngestion AS
        SELECT
            t.Id AS TransactionRecordId,
            '837P' AS TransactionSet,
            st.TransactionSetIdentifierCode_01 AS TransactionSetIdentifierCode,
            st.TransactionSetControlNumber_02 AS TransactionSetControlNumber,
            bht.SubmitterTransactionIdentifier_03 AS TransactionReferenceNumber,
            bht.TransactionSetCreationDate_04 AS TransactionSetCreationDate,
            bht.TransactionSetCreationTime_05 AS TransactionSetCreationTime,
            bht.TransactionSetPurposeCode_02 AS TransactionSetPurposeCode,
            bht.TransactionTypeCode_06 AS TransactionTypeCode
        FROM T837PTransactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN BHT_BeginningOfHierarchicalTransaction_8 AS bht ON bht.Id = t.BHT_BeginningOfHierarchicalTransactionId

        UNION ALL

        SELECT
            t.Id AS TransactionRecordId,
            '837I' AS TransactionSet,
            st.TransactionSetIdentifierCode_01 AS TransactionSetIdentifierCode,
            st.TransactionSetControlNumber_02 AS TransactionSetControlNumber,
            bht.SubmitterTransactionIdentifier_03 AS TransactionReferenceNumber,
            bht.TransactionSetCreationDate_04 AS TransactionSetCreationDate,
            bht.TransactionSetCreationTime_05 AS TransactionSetCreationTime,
            bht.TransactionSetPurposeCode_02 AS TransactionSetPurposeCode,
            bht.TransactionTypeCode_06 AS TransactionTypeCode
        FROM T837ITransactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN BHT_BeginningOfHierarchicalTransaction_8 AS bht ON bht.Id = t.BHT_BeginningOfHierarchicalTransactionId

        UNION ALL

        SELECT
            t.Id AS TransactionRecordId,
            '837D' AS TransactionSet,
            st.TransactionSetIdentifierCode_01 AS TransactionSetIdentifierCode,
            st.TransactionSetControlNumber_02 AS TransactionSetControlNumber,
            bht.SubmitterTransactionIdentifier_03 AS TransactionReferenceNumber,
            bht.TransactionSetCreationDate_04 AS TransactionSetCreationDate,
            bht.TransactionSetCreationTime_05 AS TransactionSetCreationTime,
            bht.TransactionSetPurposeCode_02 AS TransactionSetPurposeCode,
            bht.TransactionTypeCode_06 AS TransactionTypeCode
        FROM T837DTransactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN BHT_BeginningOfHierarchicalTransaction_8 AS bht ON bht.Id = t.BHT_BeginningOfHierarchicalTransactionId

        UNION ALL

        SELECT
            t.Id AS TransactionRecordId,
            '835' AS TransactionSet,
            st.TransactionSetIdentifierCode_01 AS TransactionSetIdentifierCode,
            st.TransactionSetControlNumber_02 AS TransactionSetControlNumber,
            trn.CurrentTransactionTraceNumber_02 AS TransactionReferenceNumber,
            dtm.Date_02 AS TransactionSetCreationDate,
            dtm.Time_03 AS TransactionSetCreationTime,
            NULL AS TransactionSetPurposeCode,
            NULL AS TransactionTypeCode
        FROM T835Transactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN TRN_DependentTraceNumber AS trn ON trn.Id = t.TRN_ReassociationTraceNumberId
        LEFT JOIN DTM_ProductionDate AS dtm ON dtm.Id = t.DTM_ProductionDateId;
        """);

    EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");

    // Set environment variable for AWS service URL (moto server)
    Environment.SetEnvironmentVariable("AWS_SERVICE_URL", "http://localhost:5000");

    // Process EDI files from S3 bucket
    await service.ProcessEdiFilesFromS3Async("edi-837-bucket");

    Console.WriteLine("EDI ingestion from S3 completed.");
}
