using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using System.Configuration;

namespace EDI_NCPDP_Ingestion
{
    public class Program
    {
        public static async Task Main() 
        {
            // Get the settings for reading from S3 and local path from app.config
            string _readS3 = ConfigurationManager.AppSettings["ReadS3"];
            string _local = ConfigurationManager.AppSettings["ReadLocal"];
            string _localFileName = ConfigurationManager.AppSettings["LocalFileName"];
            string _bucketName = ConfigurationManager.AppSettings["S3BucketName"];
            string _loadFilePath = ConfigurationManager.AppSettings["LocalFileLocation"];
            string _ediFabaricKey = ConfigurationManager.AppSettings["EDIFabricKey"];
            string _s3FilePrefix = ConfigurationManager.AppSettings["S3FilePrefix"];
            string _s3AccessKey = ConfigurationManager.AppSettings["AccessKey"];
            string _s3SecretKey = ConfigurationManager.AppSettings["SecretAccessKey"];

            var ncpdpFiles = new List<EdiFabric.Templates.TelcoD0.TSB1>();

            try
            {
                //Get the serial key from App.config
                var serialKey = _ediFabaricKey;

                // Read files locally
                if (_readS3 != "true")
                {
                    DirectoryInfo directory = new DirectoryInfo(_loadFilePath);
                    FileInfo[] files = directory.GetFiles();

                    foreach (FileInfo i in files)
                    {
                        string fullFilePath = i.FullName;

                        Console.WriteLine("Reading file from local path: " + fullFilePath);

                        // Read the file and get the list of TSB1 objects
                        await ReadNCPDP.ReadFile(fullFilePath, serialKey);
                    }

                }

                // Read files from S3 Bucket
                if (_readS3 == "true")
                {
                    // Get the S3Config settings from App.config
                    var s3Config = new AmazonS3Config
                    {
                        ServiceURL = ConfigurationManager.AppSettings["EndpointUrl"],
                        ForcePathStyle = true
                    };

                    var s3Client = new AmazonS3Client(_s3AccessKey, _s3SecretKey, s3Config);
                    var intializer = new MotoInitializer(s3Client, _bucketName, _s3FilePrefix, verbose: true);

                    Console.WriteLine("Reading file from S3 bucket: ");

                    // Start Moto, create bucket, upload sample files
                    Console.WriteLine("** Starting Moto S3 mock server");
                    await intializer.InitializeAsync();
                    Console.WriteLine("** Moto initialized and Files Uploaded");

                    var s3Request = new ListObjectsV2Request
                    {
                        BucketName = _bucketName,
                        Prefix = _s3FilePrefix
                    };

                    var paginatorsResponse = s3Client.Paginators.ListObjectsV2(s3Request);

                    // Loop through S3 Bucket and parse each file
                    await foreach (var p in paginatorsResponse.Responses)
                    {
                        foreach (var s3obj in p.S3Objects.Where(o => o.Size > 0))
                        {
                            //Read file into a stream and then converted into a list prior to parsing
                            var s3Loader = new S3FileLoad();
                            await s3Loader.ParseS3File(s3Client, _bucketName, s3obj.Key, serialKey);
                        }
                    }
                    // Stop Moto
                    await intializer.CleanupAsync();
                }
                
            }
            catch (Exception ex)
            {
                // Print the exception message to the console
                Console.WriteLine(ex.Message);
            }

        } // End of Main
    } // End of Program
} // End of Namespace
