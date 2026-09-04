using EdiFabric.Core.ErrorCodes;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using PayerEDI.Data.Database.Tables;

namespace PayerEDI.Tests.Database;

public class EdiErrorTableExtensionsTests
{
    [Fact]
    public void CreateEdiError_MapsMessageAndSegmentErrors()
    {
        var source = new MessageErrorContext
        {
            Name = "837",
            ControlNumber = "0031",
            Edition = "005010",
            Release = "X222A2",
            Index = 2,
            ValidatedSegmentsCount = 0,
            Errors =
            [
                new SegmentErrorContext
                {
                    Message = "Unexpected segment",
                    Name = "NM1",
                    Position = 112,
                    Value = "NM1*P3*1",
                    Codes = [SegmentErrorCode.UnexpectedSegment],
                },
            ],
        };
        var documentId = Guid.NewGuid();

        var result = source.CreateEdiError(documentId);

        Assert.Equal(documentId, result.DocumentId);
        Assert.Equal("837", result.Name);
        Assert.Equal("0031", result.ControlNumber);
        Assert.Single(result.Errors);
        Assert.Equal("NM1", result.Errors.Single().Name);
        Assert.Equal(112, result.Errors.Single().Position);
        Assert.Equal("UnexpectedSegment", result.Errors.Single().Codes.Single());
    }

    [Fact]
    public void CreateEdiError_DoesNotPersistHasErrors()
    {
        var source = new MessageErrorContext { Name = "837", HasErrors = true };

        var result = source.CreateEdiError(Guid.NewGuid());

        Assert.DoesNotContain(
            typeof(EdiErrorTable).GetProperties(),
            property => property.Name == nameof(MessageErrorContext.HasErrors)
        );
    }
}
