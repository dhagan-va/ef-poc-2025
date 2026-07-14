using EDI837Ingestion.EF;
using EDI837Ingestion.EF.Entities;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.Hipaa5010.PreErrata;
using EdiFabric.Templates.X12004010;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TS837P = EdiFabric.Templates.Hipaa5010.TS837P;

namespace EDI837Ingestion.BusinessLayer
{
    public class Edi837IngestionService : IEdi837IngestionService
    {
        private readonly string _filePath;
        private readonly AppDbContext _dbContext;

        public Edi837IngestionService(AppDbContext dbContext, IConfiguration config)
        {
            //or env variable, or some other default fallback path
            _filePath = config["FilePaths:Edi837PathWithFilename"] ?? "C:\\Projects\\VA\\EDI 837\\igor-timofeyev_i\\samples\\EDI837-sample.edi";
            _dbContext = dbContext;
        }

        public async Task IngestEdi837()
        {
            string ediPayload = string.Empty;

            // 1. Detect if the script is being piped via Python's Standard Input
            if (Console.IsInputRedirected)
            {
                using (var reader = new StreamReader(Console.OpenStandardInput(), Encoding.UTF8))
                {
                    ediPayload = await reader.ReadToEndAsync();
                }
            }
            // 2. Fallback: If running locally without Python simulation, look for a local file argument
            else if (File.Exists(_filePath))
            {
                using (var ediStream = File.OpenRead(_filePath))
                {
                    ediPayload = await File.ReadAllTextAsync(_filePath);
                }
            }
            else
            {
                Console.Error.WriteLine("Error: No EDI input detected via stream piping or local file arguments.");
            }


            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ediPayload)))
                //using (var ediStream = File.OpenRead(_filePath))
                {
                    using (var ediReader = new X12Reader(stream, "EdiFabric.Templates.Hipaa"))
                    {
                        List<IEdiItem> ediItems = ediReader.ReadToEnd().ToList();

                        // Extract the Professional 837 transaction sets
                        var transactions = ediItems.OfType<TS837P>();

                        //control segments are at the ISA/IEA level, so we need to capture them before mapping
                        string sender = string.Empty;
                        string receiver = string.Empty;
                        string controlNum = string.Empty;

                        string gsControlNum = string.Empty;
                        string gsVersionCode = string.Empty;

                    
                        // Configure the SNIP validation settings explicitly
                        var snipSettings = new ValidationSettings
                        {
                            // Options: SyntaxOnly_SNIP1, LimitsAndCodes_SNIP2, Balancing_SNIP3, InterSegment_SNIP4
                            ValidationLevel = ValidationLevel.InterSegment_SNIP4
                        };
                        MessageErrorContext errorContext;



                        foreach (var transaction in transactions)
                        {
                            if (transaction.IsValid(out errorContext, snipSettings))
                            {
                                Console.WriteLine("Success: SNIP Level 4 validation passed.");
                            }

                            if (!transaction.HasErrors)
                            {
                                    var isaHeader = ediItems.OfType<ISA>().FirstOrDefault();
                                    var gsHeader = ediItems.OfType<GS>().FirstOrDefault();

                                    // 1. EdiFabric exposes control segments directly via the Item property
                                    if (isaHeader is not null)
                                    {
                                        sender = isaHeader.InterchangeSenderID_6;
                                        receiver = isaHeader.InterchangeReceiverID_8;
                                        controlNum = isaHeader.InterchangeControlNumber_13;
                                    }

                                    // 2. NEW: Capture GS Functional Group Header Values
                                    if (gsHeader is not null)
                                    {
                                        gsControlNum = gsHeader.GroupControlNumber_6;
                                        gsVersionCode = gsHeader.VersionAndRelease_8;
                                    }

                                    InterchangeControl entityRecord = MapEdiToEntities(transaction, sender, receiver, controlNum, gsControlNum, gsVersionCode);

                                    _dbContext.InterchangeControls.Add(entityRecord);
                                    _dbContext.SaveChanges();

                                    Console.WriteLine("Success: Ingestion complete.");
                            }
                            // Check if structural or validation errors occurred during parsing
                            else
                            {
                                // Flattens the error hierarchy into an easy-to-read list of string messages
                                var errors = transaction.ErrorContext.Flatten();
                                throw new Exception($"EDI 837 parsing errors: {string.Join(", ", errors)}");
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

        public InterchangeControl MapEdiToEntities(TS837P transaction, string sender, string receiver, string controlNum, string gsControlNum, string gsVersionCode)
        {
            // 1. Map Interchange Control (ISA/IEA Layer)
            var interchange = new InterchangeControl
            {
                SenderId = sender.Trim(),
                ReceiverId = receiver.Trim(),
                ControlNumber = controlNum,
                TransmissionDate = DateTime.UtcNow, // Tracks processing timestamp
                FunctionalGroups = new List<FunctionalGroup>()
            };

            // 2. Map Functional Group (GS/GE Layer)
            var functionalGroup = new FunctionalGroup
            {
                GroupControlNumber = controlNum, 
                VersionCode = gsVersionCode,
                ClaimBatches = new List<ClaimBatch>()
            };
            interchange.FunctionalGroups.Add(functionalGroup);

            // 3. Map Claim Batch (ST/BHT Layer)
            var batch = new ClaimBatch
            {
                TransactionId = transaction.ST.TransactionSetControlNumber_02,
                ReferenceNumber = transaction.BHT_BeginningOfHierarchicalTransaction.SubmitterTransactionIdentifier_03,
                MedicalClaims = new List<MedicalClaim>()
            };

            // Map Submitter Clearinghouse (Loop 1000A)
            if (transaction.AllNM1.Loop1000A != null)
            {
                batch.SubmitterName = transaction.AllNM1.Loop1000A.NM1_SubmitterName?.ResponseContactLastorOrganizationName_03;
                batch.SubmitterContactPhone = transaction.AllNM1.Loop1000A.PER_SubmitterEDIContactInformation?.FirstOrDefault()?.ResponseContactCommunicationNumber_04; ;
            }

            // Map Receiver Payer (Loop 1000B)
            if (transaction.AllNM1.Loop1000B != null)
            {
                batch.PayerName = transaction.AllNM1.Loop1000B.NM1_ReceiverName?.ResponseContactLastorOrganizationName_03;
            }

            functionalGroup.ClaimBatches.Add(batch);

            // 4. Map Billing Provider, Patients, and Claims (Loop 2000B Hierarchy)
            foreach (var loop2000A in transaction.Loop2000A)
            {
                //1. Map Billing Provider
                var provider = new BillingProvider();

            
                if (loop2000A.AllNM1 != null && loop2000A.AllNM1.Loop2010AA != null)
                {
                    var loop2010AA = loop2000A.AllNM1.Loop2010AA;

                    if (loop2010AA.NM1_BillingProviderName != null)
                    {
                        provider.LastName = loop2010AA.NM1_BillingProviderName.ResponseContactLastorOrganizationName_03;
                        provider.FirstName = loop2010AA.NM1_BillingProviderName.ResponseContactFirstName_04;
                        provider.Npi = loop2010AA.NM1_BillingProviderName.ResponseContactIdentifier_09;
                    }

                    if (loop2010AA.N3_BillingProviderAddress != null)
                    {
                        provider.Address = loop2010AA.N3_BillingProviderAddress.ResponseContactAddressLine_01;
                    }

                    if (loop2010AA.N4_BillingProviderCity_State_ZIPCode != null)
                    {
                        provider.City = loop2010AA.N4_BillingProviderCity_State_ZIPCode.AdditionalPatientInformationContactCityName_01;
                        provider.State = loop2010AA.N4_BillingProviderCity_State_ZIPCode.AdditionalPatientInformationContactStateCode_02;
                        provider.ZipCode = loop2010AA.N4_BillingProviderCity_State_ZIPCode.AdditionalPatientInformationContactPostalZoneorZIPCode_03;
                    }

                    if (loop2010AA.AllREF.REF_BillingProviderTaxIdentification != null)
                    {
                        var taxRef = loop2010AA.AllREF.REF_BillingProviderTaxIdentification;

                        if (taxRef.ReferenceIdentificationQualifier_01 == "EI")
                        {
                            provider.TaxId = taxRef.MemberGrouporPolicyNumber_02;
                        }
                    }

                    _dbContext.BillingProviders.Add(provider);
                }
                
                // 2. NOW you can loop through the Subscribers/Patients inside that provider
                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    var subscriberPatient = new SubscriberPatient();

                    if (loop2000B.AllNM1 != null && loop2000B.AllNM1.Loop2010BA != null)
                    {
                        var loop2010BA = loop2000B.AllNM1.Loop2010BA;

                        // Map Subscriber Demographics 
                        if (loop2010BA.NM1_SubscriberName != null)
                        {
                            subscriberPatient.MemberId = loop2010BA.NM1_SubscriberName.ResponseContactIdentifier_09;//? not sure if that's correct one
                            subscriberPatient.LastName = loop2010BA.NM1_SubscriberName.ResponseContactLastorOrganizationName_03;
                            subscriberPatient.FirstName = loop2010BA.NM1_SubscriberName.ResponseContactFirstName_04;
                            // FIX 2: Correct property chain name for N4 (City/State/Zip) segment
                            if (loop2010BA.N4_SubscriberCity_State_ZIPCode != null)
                            {
                                subscriberPatient.Address = loop2010BA.N3_SubscriberAddress?.ResponseContactAddressLine_01; // Assuming N3 segment is present for address
                                subscriberPatient.City = loop2010BA.N4_SubscriberCity_State_ZIPCode.AdditionalPatientInformationContactCityName_01;
                                subscriberPatient.State = loop2010BA.N4_SubscriberCity_State_ZIPCode.AdditionalPatientInformationContactStateCode_02;
                                subscriberPatient.ZipCode = loop2010BA.N4_SubscriberCity_State_ZIPCode.AdditionalPatientInformationContactPostalZoneorZIPCode_03;
                            }

                            // FIX 3: How to get BirthDate and Gender from DMG_SubscriberDemographicInformation
                            if (loop2010BA.DMG_SubscriberDemographicInformation != null)
                            {
                                // DateTimePeriod_02 holds the raw string "19800101"
                                subscriberPatient.BirthDate = ParseEdiDate(loop2010BA.DMG_SubscriberDemographicInformation.DependentBirthDate_02);

                                // GenderCode_03 holds the raw string "F"
                                subscriberPatient.Gender = loop2010BA.DMG_SubscriberDemographicInformation.DependentGenderCode_03;
                            }
                        }
                        _dbContext.SubscriberPatients.Add(subscriberPatient);
                    }

                    // Map Claims
                    foreach (var loop2000C in loop2000B.Loop2000C)
                    {
                        foreach (var loop2300 in loop2000C.Loop2300)
                        {
                            var claim = new MedicalClaim
                            {
                                ClaimSubmitterIdentifier = loop2300.CLM_ClaimInformation.PatientControlNumber_01,
                                TotalClaimChargeAmount = decimal.Parse(loop2300.CLM_ClaimInformation.TotalClaimChargeAmount_02, CultureInfo.InvariantCulture),
                                FacilityCode = loop2300.CLM_ClaimInformation.HealthCareServiceLocationInformation_05.FacilityTypeCode_01,
                                StatementDate = DateTime.Now, //this is not right, need to find correct mapping     //ParseEdiDate(loop2300.AllDTP.date.FirstOrDefault(d => d.DateTimeQualifier_01 == "472")?.DateTimePeriod_03),
                                ServiceLines = new List<ClaimServiceLine>(),
                                Diagnoses = new List<ClaimDiagnosis>()
                            };

                            // Map Diagnosis Codes (HI Segment)
                            if (loop2300.AllHI != null)
                            {
                                // Principal Diagnosis
                                if (loop2300.AllHI.HI_HealthCareDiagnosisCode.HealthCareCodeInformation_01 != null)
                                {
                                    claim.Diagnoses.Add(new ClaimDiagnosis
                                    {
                                        DiagnosisType = loop2300.AllHI.HI_HealthCareDiagnosisCode.HealthCareCodeInformation_01.CodeListQualifierCode_01,
                                        DiagnosisCode = loop2300.AllHI.HI_HealthCareDiagnosisCode.HealthCareCodeInformation_01.IndustryCode_02
                                    });
                                }
                                // Secondary Diagnosis
                                if (loop2300.AllHI.HI_HealthCareDiagnosisCode.HealthCareCodeInformation_02 != null)
                                {
                                    claim.Diagnoses.Add(new ClaimDiagnosis
                                    {
                                        DiagnosisType = loop2300.AllHI.HI_HealthCareDiagnosisCode.HealthCareCodeInformation_02.CodeListQualifierCode_01,
                                        DiagnosisCode = loop2300.AllHI.HI_HealthCareDiagnosisCode.HealthCareCodeInformation_02.IndustryCode_02
                                    });
                                }
                            }

                            // Map Service Lines (Loop 2400)
                            foreach (var loop2400 in loop2300.Loop2400)
                            {
                                var line = new ClaimServiceLine
                                {
                                    LineNumber = int.Parse(loop2400.LX_ServiceLineNumber.AssignedNumber_01),
                                    ProcedureCode = loop2400.SV1_ProfessionalService.CompositeMedicalProcedureIdentifier_01.ProcedureCode_02,
                                    LineChargeAmount = decimal.Parse(loop2400.SV1_ProfessionalService.LineItemChargeAmount_02, CultureInfo.InvariantCulture),
                                    UnitCount = decimal.Parse(loop2400.SV1_ProfessionalService.ServiceUnitCount_04, CultureInfo.InvariantCulture),
                                    ServiceDate = loop2400.AllDTP.DTP_Date_ServiceDate.DateTimeQualifier_01 == "472"
                                        ? ParseEdiDate(loop2400.AllDTP.DTP_Date_ServiceDate.DateTimePeriod_03)
                                        : DateTime.Now // this is not right, but we need to handle the case where the service date is not provided
                                };
                                claim.ServiceLines.Add(line);
                            }

                            batch.MedicalClaims.Add(claim);
                        }
                    }
                }
            }

            return interchange;
        }

        // Helper method to securely handle EDI CCYYMMDD date string patterns
        private DateTime ParseEdiDate(string ediDate)
        {
            if (string.IsNullOrWhiteSpace(ediDate)) return DateTime.MinValue;
            return DateTime.TryParseExact(ediDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : DateTime.MinValue;
        }
    }
}
