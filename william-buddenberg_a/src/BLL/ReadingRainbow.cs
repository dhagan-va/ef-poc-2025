using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.X12004010;
using EdiMettle.Common;
using EdiMettle.Database.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle.BLL
{
    public static class ReadingRainbow
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /**
         * Used by Read() for the X12Reader to specify the assembly path containing 837 rules.
         */
        private const string HipaaRulesAssembly = "EdiFabric.Templates.Hipaa";

        /// <summary>
        /// Read in text file containing one or more Institutional Claims and 
        /// return a collection of TS837 objects for validation and conversion to entities.
        /// </summary>
        /// <param name="claimFilePath">Path to file containing 837 claims</param>
        public static IEnumerable<TS837> Read(string claimFilePath)
        {
            var ediStream = File.OpenRead(Directory.GetCurrentDirectory() + claimFilePath);

            List<IEdiItem> ediItems;
            using (var ediReader = new X12Reader(ediStream, HipaaRulesAssembly))
            {
                ediItems = [.. ediReader.ReadToEnd()];
            }

            var transactions = ediItems.OfType<TS837>();

            return transactions;
        }

        /// <summary>
        /// Converts a TS837 Edi Fabric object into an InstitutionalClaim database entity.
        /// </summary>
        /// <param name="ediClaim">Raw TS837 from file</param>
        /// <returns>Partial claim object because I don't wanna</returns>
        public static InstitutionalClaim ClaimToEntity(TS837 ediClaim)
        {
            InstitutionalClaim claimEntity = new()
            {
                IsActive = true,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
            };

            TransactionSetHeader header = new()
            {
                IsActive = true,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                IdentifierCode = ediClaim.ST.TransactionSetIdentifierCode_01,
                ControlNumber = int.Parse(ediClaim.ST.TransactionSetControlNumber_02),
                ImplementationConventionPreference = ediClaim.ST.ImplementationConventionPreference_03
            };

            claimEntity.GetTransactionSetHeader = header;

            BeginningHierarchicalTransaction bht = new()
            {
                IsActive = true,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                StructureCode = int.Parse(ediClaim.BHT.HierarchicalStructureCode_01),
                SetPurpose = int.Parse(ediClaim.BHT.TransactionSetPurposeCode_02),
                SubmitterIdentifier = int.Parse(ediClaim.BHT.ReferenceIdentification_03),
                CreationTimestamp = Helpers.GetDateTimeFromEdi(ediClaim.BHT.Date_04, ediClaim.BHT.Time_05),
                TypeCode = ediClaim.BHT.TransactionTypeCode_06
            };

            claimEntity.GetBeginningHierarchicalTransaction = bht;

            return claimEntity;
        }
    }
}
