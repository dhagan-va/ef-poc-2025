using EdiFabric;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Hl7;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.TelcoD0;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace EDI_NCPDP_Ingestion
{
    public class ReadNCPDP
    {
        public static async Task ReadFile(string filePath, string serialKey)
        {
            // Check the SerialKey prior to parsing the file. If the key is invalid or expired, an exception will be thrown and the file will not be parsed.
            try
            {   
                SerialKey.Set(serialKey, true);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Can't set token"))
                    throw new Exception("Your trial has expired! To continue using EdiFabric SDK you must purchase a plan from https://www.edifabric.com/pricing.html");
            }

            // Load into a Stream
            var ncpdpList = new List<TSB1>();
            List<IEdiItem> ncpdpItems;            
            using var ncpdpStream = File.OpenRead(filePath);
            using (var ncpdpReader = new NcpdpTelcoReader(ncpdpStream, "EdiFabric.Templates.Ncpdp"))
                ncpdpItems = ncpdpReader.ReadToEnd().ToList();

            var ncpdpTrans = ncpdpItems.OfType<TSB1>();
            
            // Loop through each of the NCPDP transactions
            foreach (var transaction in ncpdpTrans)
            {
                Tuple<bool, MessageErrorContext> result = await transaction.IsValidAsync();
                if(!result.Item1)
                {
                    var errors = result.Item2.Flatten();

                    foreach (var e in errors)
                    {
                        using (var db = new NCPDPContext())
                        {
                            Console.WriteLine($" *** {filePath}  ERROR: {e}");

                            SqlParameter param1 = new SqlParameter("@FilePath", filePath);
                            SqlParameter param2 = new SqlParameter("@ErrorMessage", e);

                            db.Database.ExecuteSqlCommand("EXEC EDI.log.InsertParsingErrors @FilePath, @ErrorMessage", param1, param2);
                        };
                    }
                }
                else
                {
                    Console.WriteLine($"File Parsed: {filePath}");
                    ncpdpList.Add(transaction);          
                }
            } // End foreach

            if (ncpdpList.Count > 0)
            {
                SaveNCPDP.ProcessClaim(ncpdpList);
            }

        } // End ReadFile
    } // End ReadNCPDP
} // End Namespace
