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

    // Create detailed transaction view with claim information
    dbContext.Database.ExecuteSqlRaw(
        """
        CREATE OR ALTER VIEW dbo.vw_EdiFileIngestionDetails AS
        SELECT
            t.Id AS TransactionRecordId,
            '837P' AS TransactionSet,
            st.TransactionSetIdentifierCode_01 AS TransactionSetIdentifierCode,
            st.TransactionSetControlNumber_02 AS TransactionSetControlNumber,
            bht.SubmitterTransactionIdentifier_03 AS TransactionReferenceNumber,
            bht.TransactionSetCreationDate_04 AS TransactionSetCreationDate,
            bht.TransactionSetCreationTime_05 AS TransactionSetCreationTime,
            bht.TransactionSetPurposeCode_02 AS TransactionSetPurposeCode,
            bht.TransactionTypeCode_06 AS TransactionTypeCode,
            clm.PatientControlNumber_01 AS PatientControlNumber,
            clm.TotalClaimChargeAmount_02 AS TotalClaimChargeAmount,
            clm.ClaimFilingIndicatorCode_05 AS ClaimFilingIndicatorCode,
            il.NameLastOrOrganizationName_03 AS PatientLastName,
            il.NameFirst_04 AS PatientFirstName,
            pr.EntityIdentifierCode_01 AS ProviderEntityCode,
            pr.NameLastOrOrganizationName_03 AS ProviderName,
            npi.IdentificationCode_04 AS ProviderNPI
        FROM T837PTransactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN BHT_BeginningOfHierarchicalTransaction_8 AS bht ON bht.Id = t.BHT_BeginningOfHierarchicalTransactionId
        LEFT JOIN CLM_CleanClaimBenefitDataIdentification AS clm ON clm.Id = t.CLM_CleanClaimBenefitDataIdentificationId
        LEFT JOIN NM1_InformationReceiverName_1 AS il ON il.Id = t.NM1_InformationReceiverName_1Id
        LEFT JOIN NM1_BillingProviderName AS pr ON pr.Id = t.NM1_BillingProviderNameId
        LEFT JOIN REF_BillingProviderSecondaryIdentifier AS npi ON npi.Id = t.REF_BillingProviderSecondaryIdentifierId

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
            bht.TransactionTypeCode_06 AS TransactionTypeCode,
            clm.PatientControlNumber_01 AS PatientControlNumber,
            clm.TotalClaimChargeAmount_02 AS TotalClaimChargeAmount,
            clm.ClaimFilingIndicatorCode_05 AS ClaimFilingIndicatorCode,
            il.NameLastOrOrganizationName_03 AS PatientLastName,
            il.NameFirst_04 AS PatientFirstName,
            pr.EntityIdentifierCode_01 AS ProviderEntityCode,
            pr.NameLastOrOrganizationName_03 AS ProviderName,
            npi.IdentificationCode_04 AS ProviderNPI
        FROM T837ITransactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN BHT_BeginningOfHierarchicalTransaction_8 AS bht ON bht.Id = t.BHT_BeginningOfHierarchicalTransactionId
        LEFT JOIN CLM_CleanClaimBenefitDataIdentification AS clm ON clm.Id = t.CLM_CleanClaimBenefitDataIdentificationId
        LEFT JOIN NM1_InformationReceiverName_1 AS il ON il.Id = t.NM1_InformationReceiverName_1Id
        LEFT JOIN NM1_BillingProviderName AS pr ON pr.Id = t.NM1_BillingProviderNameId
        LEFT JOIN REF_BillingProviderSecondaryIdentifier AS npi ON npi.Id = t.REF_BillingProviderSecondaryIdentifierId

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
            bht.TransactionTypeCode_06 AS TransactionTypeCode,
            clm.PatientControlNumber_01 AS PatientControlNumber,
            clm.TotalClaimChargeAmount_02 AS TotalClaimChargeAmount,
            clm.ClaimFilingIndicatorCode_05 AS ClaimFilingIndicatorCode,
            il.NameLastOrOrganizationName_03 AS PatientLastName,
            il.NameFirst_04 AS PatientFirstName,
            pr.EntityIdentifierCode_01 AS ProviderEntityCode,
            pr.NameLastOrOrganizationName_03 AS ProviderName,
            npi.IdentificationCode_04 AS ProviderNPI
        FROM T837DTransactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN BHT_BeginningOfHierarchicalTransaction_8 AS bht ON bht.Id = t.BHT_BeginningOfHierarchicalTransactionId
        LEFT JOIN CLM_CleanClaimBenefitDataIdentification AS clm ON clm.Id = t.CLM_CleanClaimBenefitDataIdentificationId
        LEFT JOIN NM1_InformationReceiverName_1 AS il ON il.Id = t.NM1_InformationReceiverName_1Id
        LEFT JOIN NM1_BillingProviderName AS pr ON pr.Id = t.NM1_BillingProviderNameId
        LEFT JOIN REF_BillingProviderSecondaryIdentifier AS npi ON npi.Id = t.REF_BillingProviderSecondaryIdentifierId

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
            NULL AS TransactionTypeCode,
            clp.PatientControlNumber_03 AS PatientControlNumber,
            clp.ClaimSubmissionAmount_04 AS TotalClaimChargeAmount,
            NULL AS ClaimFilingIndicatorCode,
            NULL AS PatientLastName,
            NULL AS PatientFirstName,
            NULL AS ProviderEntityCode,
            NULL AS ProviderName,
            NULL AS ProviderNPI
        FROM T835Transactions AS t
        LEFT JOIN ST AS st ON st.Id = t.STId
        LEFT JOIN TRN_DependentTraceNumber AS trn ON trn.Id = t.TRN_ReassociationTraceNumberId
        LEFT JOIN DTM_ProductionDate AS dtm ON dtm.Id = t.DTM_ProductionDateId
        LEFT JOIN CLP_ClaimPaymentInformation AS clp ON clp.Id = t.CLP_ClaimPaymentInformationId;
        """);

    // Set required environment variable for EdiFabric
    Environment.SetEnvironmentVariable("EDIFABRIC_SERIAL_KEY", "c417cb9dd9d54297a55c032a74c87996");

    // Set environment variable for AWS service URL (moto server)
    Environment.SetEnvironmentVariable("AWS_SERVICE_URL", "http://localhost:5000");

    // Process EDI files from S3 bucket
    await service.ProcessEdiFilesFromS3Async("edi-837-bucket");

    Console.WriteLine("EDI ingestion from S3 completed.");
}
