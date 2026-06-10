using EDI_NCPDP_Ingestion;
using EdiFabric;
using EdiFabric.Templates.TelcoD0;
using Microsoft.Extensions.DependencyModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCPDP_Test
{
    [TestClass]
    public class NCPDPTest
    {

        [TestMethod]
        public void TestDBConnection()
        {
            var dbConn = new Mock<DbConnection>().Object;

            var targetDB = new NCPDPContext();
            Assert.IsNotNull(targetDB);

        }

        [TestMethod]
        public void TestFileParse()
        {
            var filePath = "C:\\Files\\ClaimBilling";
            var serialKey = "c417cb9dd9d54297a55c032a74c87996";

            var result = ReadNCPDP.ReadFile(filePath, serialKey);

            Assert.IsNotNull(result);
            //Assert.IsNotEmpty(result);
        }

        [TestMethod]
        public void TestFileParseWithInvalidKey()
        {
            var filePath = "C:\\Files\\ClaimBilling";
            var invalidSerialKey = "invalid";

            var action = () => ReadNCPDP.ReadFile(filePath, invalidSerialKey);

            var exception = Assert.Throws<Exception>(action) ;
            Assert.AreEqual("The token was not set!", exception.Message);
        }
    } // End of NCPDPTest
}
