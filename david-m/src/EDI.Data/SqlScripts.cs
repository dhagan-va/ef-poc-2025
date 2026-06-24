using EDI.Data;

namespace EDI.Data
{
    /// <summary>
    /// Central home for reusable SQL scripts (views used inside migrations and startup helpers).
    /// </summary>
    public static class SqlScripts
    {
        public const string EdiFileIngestionView =
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
            """;

        public const string EdiFileIngestionDetailsView =
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
                JSON_QUERY(
                    (SELECT
                        JSON_QUERY((SELECT * FROM T837PTransactions tx WHERE tx.Id = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionRow,
                        JSON_QUERY((SELECT * FROM ST stx WHERE stx.Id = t.STId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS ST,
                        JSON_QUERY((SELECT * FROM BHT_BeginningOfHierarchicalTransaction_8 bhtx WHERE bhtx.Id = t.BHT_BeginningOfHierarchicalTransactionId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS BHT,
                        JSON_QUERY((SELECT * FROM SE sex WHERE sex.Id = t.SEId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS SE,
                        JSON_QUERY((SELECT * FROM All_NM1_837P_6 nm1 WHERE nm1.Id = t.AllNM1Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionLevelNm1,
                        JSON_QUERY((SELECT * FROM Loop_2000A_837P l WHERE l.TS837PId = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000A,
                        JSON_QUERY((SELECT * FROM Loop_2000B_837P l WHERE l.Loop_2000A_837PId IN (SELECT Id FROM Loop_2000A_837P WHERE TS837PId = t.Id) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000B,
                        JSON_QUERY((SELECT * FROM Loop_2000C_837P l WHERE l.Loop_2000B_837PId IN (SELECT Id FROM Loop_2000B_837P WHERE Loop_2000A_837PId IN (SELECT Id FROM Loop_2000A_837P WHERE TS837PId = t.Id)) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000C,
                        JSON_QUERY((SELECT * FROM Loop_2300_837P l WHERE l.Loop_2000B_837PId IN (SELECT Id FROM Loop_2000B_837P WHERE Loop_2000A_837PId IN (SELECT Id FROM Loop_2000A_837P WHERE TS837PId = t.Id)) OR l.Loop_2000C_837PId IN (SELECT Id FROM Loop_2000C_837P WHERE Loop_2000B_837PId IN (SELECT Id FROM Loop_2000B_837P WHERE Loop_2000A_837PId IN (SELECT Id FROM Loop_2000A_837P WHERE TS837PId = t.Id))) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2300
                    FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
                ) AS TransactionGraph
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
                bht.TransactionTypeCode_06 AS TransactionTypeCode,
                JSON_QUERY(
                    (SELECT
                        JSON_QUERY((SELECT * FROM T837ITransactions tx WHERE tx.Id = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionRow,
                        JSON_QUERY((SELECT * FROM ST stx WHERE stx.Id = t.STId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS ST,
                        JSON_QUERY((SELECT * FROM BHT_BeginningOfHierarchicalTransaction_8 bhtx WHERE bhtx.Id = t.BHT_BeginningOfHierarchicalTransactionId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS BHT,
                        JSON_QUERY((SELECT * FROM SE sex WHERE sex.Id = t.SEId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS SE,
                        JSON_QUERY((SELECT * FROM All_NM1_837I_6 nm1 WHERE nm1.Id = t.AllNM1Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionLevelNm1,
                        JSON_QUERY((SELECT * FROM Loop_2000A_837I l WHERE l.TS837IId = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000A,
                        JSON_QUERY((SELECT * FROM Loop_2000B_837I l WHERE l.Loop_2000A_837IId IN (SELECT Id FROM Loop_2000A_837I WHERE TS837IId = t.Id) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000B,
                        JSON_QUERY((SELECT * FROM Loop_2000C_837I l WHERE l.Loop_2000B_837IId IN (SELECT Id FROM Loop_2000B_837I WHERE Loop_2000A_837IId IN (SELECT Id FROM Loop_2000A_837I WHERE TS837IId = t.Id)) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000C,
                        JSON_QUERY((SELECT * FROM Loop_2300_837I l WHERE l.Loop_2000B_837IId IN (SELECT Id FROM Loop_2000B_837I WHERE Loop_2000A_837IId IN (SELECT Id FROM Loop_2000A_837I WHERE TS837IId = t.Id)) OR l.Loop_2000C_837IId IN (SELECT Id FROM Loop_2000C_837I WHERE Loop_2000B_837IId IN (SELECT Id FROM Loop_2000B_837I WHERE Loop_2000A_837IId IN (SELECT Id FROM Loop_2000A_837I WHERE TS837IId = t.Id))) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2300
                    FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
                ) AS TransactionGraph
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
                bht.TransactionTypeCode_06 AS TransactionTypeCode,
                JSON_QUERY(
                    (SELECT
                        JSON_QUERY((SELECT * FROM T837DTransactions tx WHERE tx.Id = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionRow,
                        JSON_QUERY((SELECT * FROM ST stx WHERE stx.Id = t.STId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS ST,
                        JSON_QUERY((SELECT * FROM BHT_BeginningOfHierarchicalTransaction_8 bhtx WHERE bhtx.Id = t.BHT_BeginningOfHierarchicalTransactionId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS BHT,
                        JSON_QUERY((SELECT * FROM SE sex WHERE sex.Id = t.SEId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS SE,
                        JSON_QUERY((SELECT * FROM All_NM1_837D_6 nm1 WHERE nm1.Id = t.AllNM1Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionLevelNm1,
                        JSON_QUERY((SELECT * FROM Loop_2000A_837D l WHERE l.TS837DId = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000A,
                        JSON_QUERY((SELECT * FROM Loop_2000B_837D l WHERE l.Loop_2000A_837DId IN (SELECT Id FROM Loop_2000A_837D WHERE TS837DId = t.Id) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000B,
                        JSON_QUERY((SELECT * FROM Loop_2000C_837D l WHERE l.Loop_2000B_837DId IN (SELECT Id FROM Loop_2000B_837D WHERE Loop_2000A_837DId IN (SELECT Id FROM Loop_2000A_837D WHERE TS837DId = t.Id)) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2000C,
                        JSON_QUERY((SELECT * FROM Loop_2300_837D l WHERE l.Loop_2000B_837DId IN (SELECT Id FROM Loop_2000B_837D WHERE Loop_2000A_837DId IN (SELECT Id FROM Loop_2000A_837D WHERE TS837DId = t.Id)) OR l.Loop_2000C_837DId IN (SELECT Id FROM Loop_2000C_837D WHERE Loop_2000B_837DId IN (SELECT Id FROM Loop_2000B_837D WHERE Loop_2000A_837DId IN (SELECT Id FROM Loop_2000A_837D WHERE TS837DId = t.Id))) FOR JSON PATH, INCLUDE_NULL_VALUES)) AS Loop_2300
                    FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
                ) AS TransactionGraph
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
                NULL AS TransactionTypeCode,
                JSON_QUERY(
                    (SELECT
                        JSON_QUERY((SELECT * FROM T835Transactions tx WHERE tx.Id = t.Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TransactionRow,
                        JSON_QUERY((SELECT * FROM ST stx WHERE stx.Id = t.STId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS ST,
                        JSON_QUERY((SELECT * FROM BPR_FinancialInformation_2 bpr WHERE bpr.Id = t.BPR_FinancialInformationId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS BPR_FinancialInformation,
                        JSON_QUERY((SELECT * FROM TRN_DependentTraceNumber trnInner WHERE trnInner.Id = t.TRN_ReassociationTraceNumberId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS TRN_ReassociationTraceNumber,
                        JSON_QUERY((SELECT * FROM CUR_ForeignCurrencyInformation_2 cur WHERE cur.Id = t.CUR_ForeignCurrencyInformationId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS CUR_ForeignCurrencyInformation,
                        JSON_QUERY((SELECT * FROM All_REF_835_3 ref WHERE ref.Id = t.AllREFId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS All_REF_835_3,
                        JSON_QUERY((SELECT * FROM DTM_ProductionDate dtmInner WHERE dtmInner.Id = t.DTM_ProductionDateId FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS DTM_ProductionDate,
                        JSON_QUERY((SELECT * FROM All_N1_835 n1 WHERE n1.Id = t.AllN1Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)) AS All_N1_835
                    FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
                ) AS TransactionGraph
            FROM T835Transactions AS t
            LEFT JOIN ST AS st ON st.Id = t.STId
            LEFT JOIN TRN_DependentTraceNumber AS trn ON trn.Id = t.TRN_ReassociationTraceNumberId
            LEFT JOIN DTM_ProductionDate AS dtm ON dtm.Id = t.DTM_ProductionDateId;
            """;
    }
}
