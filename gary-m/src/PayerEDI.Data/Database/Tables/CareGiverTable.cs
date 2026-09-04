using PayerEDI.Data.Models;

namespace PayerEDI.Data.Database.Tables;

public record CareGiverTable
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string? LastName { get; init; }
    public string? SecondLastName { get; init; }
    public string? FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string? Title { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
}

public static class CareGiverTableExtensions
{
    extension(CareGiverTable)
    {
        public static CareGiverTable New(Person person) =>
            new()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                SecondLastName = person.SecondLastName,
                MiddleName = person.MiddleName,
                Title = null, //TODO: Find this in the medical professional's data
                Prefix = person.Prefix,
                Suffix = person.Suffix,
            };
    }
}
