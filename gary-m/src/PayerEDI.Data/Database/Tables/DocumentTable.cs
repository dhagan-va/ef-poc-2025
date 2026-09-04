using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using PayerEDI.Data.Helpers;
using PayerEDI.Data.Models.Claims;

namespace PayerEDI.Data.Database.Tables;

public record DocumentTable
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string EdiMessageType { get; init; }
    public required DateTime TransactionDateTime { get; init; }
    public required string Xml { get; init; }
}

public static class DocumentTableExtensions
{
    public static DocumentTable CreateDocument(this TS837P ts837P, DateTime transactionDateTime) =>
        new()
        {
            EdiMessageType = nameof(TS837P),
            TransactionDateTime = transactionDateTime,
            Xml = ts837P.ToXml(),
        };

    public static DocumentTable CreateDocument(this TS837D ts837D, DateTime transactionDateTime) =>
        new()
        {
            EdiMessageType = nameof(TS837D),
            TransactionDateTime = transactionDateTime,
            Xml = ts837D.ToXml(),
        };

    public static DocumentTable CreateDocument(this TS275 ts275, DateTime transactionDateTime) =>
        new()
        {
            EdiMessageType = nameof(TS275),
            TransactionDateTime = transactionDateTime,
            Xml = ts275.ToXml(),
        };
}
