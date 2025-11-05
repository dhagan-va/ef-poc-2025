using System.Text;
using Edi837Ingestion.Data;
using Edi837Ingestion.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class EdiIngestionServiceTests
{
    private static Hipaa5010Context CreateInMemoryDb()
        => new(new DbContextOptionsBuilder<Hipaa5010Context>()
            .UseInMemoryDatabase("HipaaTestDb")
            .Options);

    [Fact]
    public async Task Ingest_Sample837_SavesTransactions()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<EdiIngestionService>();
        var svc = new EdiIngestionService(db, logger);

        // Prefer your file if present
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "Documents", "DentalClaim.txt");

        Stream input;
        if (File.Exists(path))
        {
            input = File.OpenRead(path);
        }
        else
        {
            // Minimal, valid 005010X222A1 837P fallback
            var edi =
                "ISA*00*          *00*          *ZZ*SENDERID       *ZZ*RECEIVERID     *230101*1200*^*00501*000000905*1*T*:~" +
                "GS*HC*SENDER*RECEIVER*20230101*1200*1*X*005010X222A1~" +
                "ST*837*0001*005010X222A1~" +
                "BHT*0019*00*0123*20230101*1200*CH~" +
                "NM1*41*2*SUBMITTER*****46*SUBMITID~" +
                "PER*IC*SUBMITTER CONTACT*TE*5555555555~" +
                "NM1*40*2*RECEIVER*****46*RECEIVERID~" +
                "HL*1**20*1~" +
                "NM1*85*2*BILLING PROVIDER*****XX*1234567893~" +
                "N3*123 MAIN ST~" +
                "N4*CITY*ST*12345~" +
                "REF*EI*123456789~" +
                "HL*2*1*22*0~" +
                "SBR*P*18*******MC~" +
                "NM1*IL*1*DOE*JANE****MI*W000000001~" +
                "N3*456 OAK ST~" +
                "N4*CITY*ST*12345~" +
                "DMG*D8*19700101*F~" +
                "NM1*PR*2*PAYER*****PI*01234~" +
                "CLM*PCN12345*100***11:B:1*Y*A*Y*Y~" +
                "HI*ABK:Z123~" +
                "SE*20*0001~" +
                "GE*1*1~" +
                "IEA*1*000000905~";
            input = new MemoryStream(Encoding.UTF8.GetBytes(edi));
        }

        await using (input)
        {
            // Act
            var count = await svc.IngestAsync(input);

            // Assert
            Assert.True(count >= 1);                 // at least one 837P saved
            Assert.True(await db.TS837P.CountAsync() >= 1);
            Assert.True(await db.ST.CountAsync() >= 1);
            Assert.True(await db.SE.CountAsync() >= 1);
        }
    }
}
