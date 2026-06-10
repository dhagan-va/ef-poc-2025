using Amazon.S3;
using Amazon.S3.Model;
using EdiFabric;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.TelcoD0;
using System.Data.SqlClient;

namespace EDI_NCPDP_Ingestion
{
    public class S3FileLoad
    {
        // This method parses an NCPDP file from S3 using the EdiFabric SDK.
        // It takes in the bucket name, key name, and serial key as parameters and returns
        // a list of TSB1 objects representing the parsed transactions.
        public async Task ParseS3File(IAmazonS3 s3Client, string bucketName, string fileName, string serialKey)
        {
            try
            {
                SerialKey.Set(serialKey, true);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Can't set token"))
                    throw new Exception("Your trial has expired! To continue using EdiFabric SDK you must purchase a plan from https://www.edifabric.com/pricing.html");
            }

            // Load the S3 File into a stream for processing
            var memStream = LoadS3FileToMemoryStreamAsync(s3Client, bucketName, fileName).Result;
            using var ncpdpReader = new NcpdpTelcoReader(memStream, "EdiFabric.Templates.Ncpdp");
            var ncpdpItems = ncpdpReader.ReadToEnd().ToList();
            var ncpdpTrans = ncpdpItems.OfType<TSB1>();
            var ncpdpList = new List<TSB1>();

            // Loop through each of the NCPDP transactions and save them to a list
            foreach (var transaction in ncpdpTrans)
            {
                Tuple<bool, MessageErrorContext> result = await transaction.IsValidAsync();
                if (!result.Item1)
                {
                    var errors = result.Item2.Flatten();

                    foreach (var e in errors)
                    {
                        using (var db = new NCPDPContext())
                        {
                            Console.WriteLine($" *** {fileName}  ERROR: {e}");

                            SqlParameter param1 = new SqlParameter("@FilePath", fileName);
                            SqlParameter param2 = new SqlParameter("@ErrorMessage", e);

                            db.Database.ExecuteSqlCommand("EXEC EDI.log.InsertParsingErrors @FilePath, @ErrorMessage", param1, param2);
                        }
                        ;
                    }
                }
                else
                {
                    Console.WriteLine($"File Parsed: {transaction}");
                    ncpdpList.Add(transaction);
                }
            }

            if (ncpdpList.Count > 0)
            {
                SaveNCPDP.ProcessClaim(ncpdpList);
            }
        } // End ParseS3File

        // This method loads a file from S3 into a MemoryStream to be parsed by the EdiFabric SDK.
        // The method takes in the S3 client, bucket name, and key name as parameters and
        // returns a MemoryStream containing the file data.
        private async Task<MemoryStream> LoadS3FileToMemoryStreamAsync(IAmazonS3 s3Client, string bucketName, string fileName)
        {
            var _memStream = new MemoryStream();
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                using var response = await s3Client.GetObjectAsync(request);
                using var responseStream = response.ResponseStream;
                await responseStream.CopyToAsync(_memStream);
                _memStream.Position = 0; // Reset the position to the beginning of the stream
                return _memStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file from S3: {ex.Message}");
                throw;
            }
        }// End LoadS3FileToMemoryStreamAsync

    } // End S3FileLoad
} // End Namespace
