using EDI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDI.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(EdiDbContext))]
    [Migration("20251117090500_AddFileIngestionView")]
    public partial class AddFileIngestionView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID('dbo.vw_EdiFileIngestion', 'V') IS NOT NULL
                    DROP VIEW dbo.vw_EdiFileIngestion;
                """);
        }

        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            new SnapshotInvoker().Invoke(modelBuilder);
        }

        private sealed class SnapshotInvoker : EdiDbContextModelSnapshot
        {
            public void Invoke(ModelBuilder modelBuilder) => base.BuildModel(modelBuilder);
        }
    }
}
