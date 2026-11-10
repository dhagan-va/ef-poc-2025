namespace EdiConsole.Domain.Parties;

/// <summary>
/// A postal address, as sent in the N3/N4 segments.
/// Value object: stored as columns of the owning table (EF Core complex type).
/// </summary>
/// <remarks>
/// Every property is optional because some loops send no address (e.g. 2010BA when the
/// subscriber is not the patient). EF Core 8 complex types can't be null, so "no address"
/// is an <see cref="Address"/> whose properties are all null.
/// </remarks>
public sealed record Address
{
    public string? Line1 { get; init; }       // N301
    public string? Line2 { get; init; }       // N302
    public string? City { get; init; }        // N401
    public string? State { get; init; }       // N402
    public string? PostalCode { get; init; }  // N403
}
