using System.Text;
using Edi837Ingestion.Data;
using Edi837Ingestion.Edi;            
using Microsoft.EntityFrameworkCore;
using Xunit;

public class EdiIngestionServiceTests
{
    private const string Sample837P =
        "ISA*00*          *00*          *ZZ*SENDERID      *ZZ*RECEIVERID    *20210101*1253*^*00501*000000905*0*T*:~" +
        "GS*HC*SENDER*RECEIVER*20210101*1253*1*X*005010X222A1~" +
        "ST*837*0001*005010X222A1~" +
        "BHT*0019*00*0123*20210101*1319*CH~" +
        "NM1*41*2*SENDER*****46*12345~" +
        "NM1*40*2*RECEIVER*****46*98765~" +
        "HL*1**20*1~" +
        "NM1*85*2*PROVIDER NAME*****XX*1234567893~" +
        "HL*2*1*22*0~" +
        "NM1*IL*1*DOE*JOHN****MI*W000000000~" +
        "NM1*PR*2*INSURANCE CO*****PI*842610001~" +
        "CLM*PCN12345*100***11:B:1*Y*A*Y*I~" +
        "SE*12*0001~" +
        "GE*1*1~" +
        "IEA*1*000000905~";

    [Fact]
    public async Task Ingest837Async_SavesAtLeastOneTransaction()
    {
        var options = new DbContextOptionsBuilder<IngestionDbContext>()
            .UseInMemoryDatabase(databaseName: "ingestdb-smoke")
            .Options;

        await using var db = new IngestionDbContext(options);
        var svc = new EdiIngestionService(db);

        // Act
        await using var ms = new MemoryStream(Encoding.ASCII.GetBytes(Sample837P));
        var count = await svc.Ingest837Async(ms);

        // Assert
        Assert.True(count > 0);
        Assert.True(db.TransactionSets.Count() > 0);
        Assert.True(db.ClaimStubs.Count() > 0);
    }
}
