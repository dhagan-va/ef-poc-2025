using EdiFabric.Templates.TelcoD0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI_NCPDP_Ingestion
{
    public class SaveNCPDP
    {
        public static void ProcessClaim(List<TSB1> tSB1s)
        {
            using (var db = new NCPDPContext())
            {
                foreach (var tSB in tSB1s)
                {
                    tSB.ClearCache();

                    // Load into TransactionHeader
                    if (tSB.G1 is not null)
                    {
                        db.transactionHeader.Add(tSB.G1);
                        db.SaveChanges();
                    }

                    // Load into AM01
                    if (tSB.AM04Loop.AM01 is not null)
                    {
                        db.AM01.Add(tSB.AM04Loop.AM01);
                        db.SaveChanges();
                    }

                    // Load into AM04
                    if (tSB.AM04Loop.AM04 is not null)
                    {
                        db.AM04.Add(tSB.AM04Loop.AM04);
                        db.SaveChanges();
                    }

                    // Load into AM02
                    if (tSB.AM07Loop[0].AM02 is not null)
                    {
                        db.AM02.Add(tSB.AM07Loop[0].AM02);
                        db.SaveChanges();
                    }

                    // Load into AM03
                    if (tSB.AM07Loop[0].AM03 is not null)
                    {
                        db.AM03.Add(tSB.AM07Loop[0].AM03);
                        db.SaveChanges();
                    }

                    // Load into AM05
                    if (tSB.AM07Loop[0].AM05 is not null)
                    {
                        db.AM05.Add(tSB.AM07Loop[0].AM05);
                        db.SaveChanges();
                    }

                    // Load into AM06
                    if (tSB.AM07Loop[0].AM06 is not null)
                    {
                        db.AM06.Add(tSB.AM07Loop[0].AM06);
                        db.SaveChanges();
                    }

                    // Load into AM07
                    if (tSB.AM07Loop[0].AM07 is not null)
                    {
                        db.AM07.Add(tSB.AM07Loop[0].AM07);
                        db.SaveChanges();
                    }

                    // Load into AM08
                    if (tSB.AM07Loop[0].AM08 is not null)
                    {
                        db.AM08.Add(tSB.AM07Loop[0].AM08);
                        db.SaveChanges();
                    }

                    // Load into AM09
                    if (tSB.AM07Loop[0].AM09 is not null)
                    {
                        db.AM09.Add(tSB.AM07Loop[0].AM09);
                        db.SaveChanges();
                    }

                    // Load into AM10
                    if (tSB.AM07Loop[0].AM10 is not null)
                    {
                        db.AM10.Add(tSB.AM07Loop[0].AM10);
                        db.SaveChanges();
                    }

                    // Load into AM11
                    if (tSB.AM07Loop[0].AM11 is not null)
                    {
                        db.AM11.Add(tSB.AM07Loop[0].AM11);
                        db.SaveChanges();
                    }

                    // Load into AM13
                    if (tSB.AM07Loop[0].AM13 is not null)
                    {
                        db.AM13.Add(tSB.AM07Loop[0].AM13);
                        db.SaveChanges();
                    }

                    // Load into AM14
                    if (tSB.AM07Loop[0].AM14 is not null)
                    {
                        db.AM14.Add(tSB.AM07Loop[0].AM14);
                        db.SaveChanges();
                    }

                    // Load into AM15
                    if (tSB.AM07Loop[0].AM15 is not null)
                    {
                        db.AM15.Add(tSB.AM07Loop[0].AM15);
                        db.SaveChanges();
                    }

                    // Load into AM16
                    if (tSB.AM07Loop[0].AM16 is not null)
                    {
                        db.AM16.Add(tSB.AM07Loop[0].AM16);
                        db.SaveChanges();
                    }
                }// End ForEach
            } // End Using

            //Console.WriteLine("File(s) loaded.");

        } // End ProcessClaim
    } // End SaveNPDPD
} //End namespace
