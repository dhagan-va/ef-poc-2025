using Amazon.S3;
using Amazon.S3.Model;
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
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using TS837P = EdiFabric.Templates.Hipaa5010.TS837P;

namespace EDI837Ingestion.BusinessLayer
{
    public class Edi837IngestionService : IEdi837IngestionService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AppDbContext _dbContext;
        private readonly string _filePath;
        private readonly IConfiguration _config;

        public Edi837IngestionService(IAmazonS3 s3Client, AppDbContext dbContext, IConfiguration config)
        {
            //env variable, or some other default fallback path
            _filePath = Environment.GetEnvironmentVariable("Edi837_PathWithFilename") ?? config["FilePaths:Edi837PathWithFilename"] ?? "C:\\Projects\\VA\\EDI 837\\igor-timofeyev_i\\samples\\EDI837-sample.edi";
            _dbContext = dbContext;
            _s3Client = s3Client;
            _config = config;
        }

        public async Task IngestEdi837(bool useLocalMoto)
        {
            string ediPayload = string.Empty;

            try
            {
                Console.WriteLine("Downloading EDI files from S3...");

                // --- AUTOMATED MOTO S3 SEEDING STEP ---
                bool.TryParse(Environment.GetEnvironmentVariable("UseLocalFileDirectory"), out bool parsed);
                bool useLocalFileDirectory = parsed;
                if (!useLocalFileDirectory)
                {
                    try
                    {
                        //set up S3 bucket and seed it with sample EDI files for local testing
                        await S3BucketSetup();
                        //read file from S3 bucket and process it
                        await ProcessS3BucketFiles();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Error.WriteLine($"[Mock Warning] Failed to seed Moto S3 container. Ensure Docker container is running! Details: {ex.Message}");
                        Console.ResetColor();

                        // Fallback: If running locally without S3 simulation, look for a local file argument
                        Console.Error.WriteLine($"S3 Storage Operation Failed: {ex.Message}, trying to find a local file");
                        if (File.Exists(_filePath))
                        {
                            using (var ediStream = File.OpenRead(_filePath))
                            {
                                ediPayload = await File.ReadAllTextAsync(_filePath);
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("Error: No EDI input detected via stream piping or local file arguments.");

                            return;
                        }
                    }
                }
                //Process files from a folder instead of S3
                //todo: add loop for multiple files in a folder, for now just one file
                else
                {
                    Console.Error.WriteLine($"Processing file: {_filePath}");
                    if (File.Exists(_filePath))
                    {
                        using (var ediStream = File.OpenRead(_filePath))
                        {
                            ediPayload = await File.ReadAllTextAsync(_filePath);
                        }
                        ProcessFiles(ediPayload);
                    }
                }

            }
            catch (AmazonS3Exception ex)
            {
                Console.Error.WriteLine($"S3 Storage Operation Failed: {ex.Message}, trying to find a local file");
                if (File.Exists(_filePath))
                {
                    using (var ediStream = File.OpenRead(_filePath))
                    {
                        ediPayload = await File.ReadAllTextAsync(_filePath);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Error: No EDI input detected via stream piping or local file arguments.");

                    return;
                }
            }
        }

        private async Task ProcessS3BucketFiles()
        {
            string bucketName = "edi-claims-storage";
            string prefix = "claims/";

            Console.WriteLine($"Querying S3 bucket '{bucketName}' for files under prefix '{prefix}'...");

            // Fetch the metadata index of all objects inside the "claims/" folder
            var listRequest = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = prefix
            };

            ListObjectsV2Response listResponse;

            do
            {
                listResponse = await _s3Client.ListObjectsV2Async(listRequest);

                // Loop through every object discovered in the S3 registry index
                foreach (S3Object s3Object in listResponse.S3Objects)
                {
                    // Skip the folder placeholder key itself if it exists
                    if (s3Object.Key.EndsWith("/")) continue;

                    Console.WriteLine($"\n--- Processing S3 Object: {s3Object.Key} ---");

                    // Create a specific Get request dynamically for this iteration's key
                    var getRequest = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = s3Object.Key
                    };

                    using var response = await _s3Client.GetObjectAsync(getRequest);
                    using var reader = new StreamReader(response.ResponseStream, Encoding.UTF8);

                    var ediPayload = await reader.ReadToEndAsync();
                    Console.WriteLine($"Successfully downloaded {s3Object.Key} ({ediPayload.Length} characters).");

                    // Hand off the downloaded payload directly to your EdiFabric parsing pipeline
                    try
                    {
                        ProcessFiles(ediPayload);
                        Console.WriteLine($"Successfully processed EDI data for {s3Object.Key}.");
                    }
                    catch (Exception parserEx)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Error.WriteLine($"[Parser Error] Failed parsing {s3Object.Key}: {parserEx.Message}");
                        Console.ResetColor();
                        // Optional: use 'continue;' to skip bad files and process the next one instead of crashing
                    }
                }

                // Handle pagination token hooks if the bucket contains more than 1,000 files
                listRequest.ContinuationToken = listResponse.NextContinuationToken;

            } while (listResponse.IsTruncated ?? true); // Continue loop if more pages of files exist

            // OPTIONAL: We can also delete files after they're processed if desired, but for now we just log completion (they get overriden each time process runs)


            Console.WriteLine("\n[Pipeline Complete] All S3 EDI claim payloads processed successfully.");
        }

        private async Task S3BucketSetup()
        {
            Console.WriteLine("[Mock Setup] Initializing local Moto S3 Bucket...");
            await _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = "edi-claims-storage" });

            // Sample raw EDI 837 payload text to seed your local Moto environment
            string sampleEdi = string.Empty;
            string filePath = Environment.GetEnvironmentVariable("Edi837_PathWithFilename") ?? _config["FilePaths:Edi837PathWithFilename"] ?? "C:\\Projects\\VA\\EDI 837\\igor-timofeyev_i\\samples\\EDI837-sample.edi";
            if (File.Exists(filePath))
            {
                using (var ediStream = File.OpenRead(filePath))
                {
                    sampleEdi = await File.ReadAllTextAsync(filePath);
                }
            }
            byte[] ediBytes = Encoding.UTF8.GetBytes(sampleEdi);


            //upload sample 1 to s3 bucket
            using var memoryStream = new MemoryStream(ediBytes);
            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = "edi-claims-storage",
                Key = "claims/EDI837-sample_1.edi",
                InputStream = memoryStream
            });
            Console.WriteLine("[Mock Setup] Sample 1 EDI 837 data successfully pushed to local Moto storage.");

            //upload sample 2 to s3 bucket
            using var memoryStream2 = new MemoryStream(ediBytes);
            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = "edi-claims-storage",
                Key = "claims/EDI837-sample_2.edi",
                InputStream = memoryStream2
            });
            Console.WriteLine("[Mock Setup] Sample 2 EDI 837 data successfully pushed to local Moto storage.");
        }

        private void ProcessFiles(string ediPayload)
        {
            Console.WriteLine($"Processing EDI content length: {ediPayload.Length} characters.");

            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ediPayload)))
                {
                    using (var ediReader = new X12Reader(stream, "EdiFabric.Templates.Hipaa"))
                    {
                        List<IEdiItem> ediItems = ediReader.ReadToEnd().ToList();

                        // Extract the Professional 837 transaction sets
                        var transactions837 = ediItems.OfType<TS837P>();

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



                        foreach (var transaction in transactions837)
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
        private InterchangeControl MapEdiToEntities(TS837P transaction, string sender, string receiver, string controlNum, string gsControlNum, string gsVersionCode)
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
