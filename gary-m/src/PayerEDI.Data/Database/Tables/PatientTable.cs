using FastEnumUtility;
using PayerEDI.Data.Models;

namespace PayerEDI.Data.Database.Tables;

public record PatientTable
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string EntityType { get; init; }
    public string? EntityIdentifierCode { get; init; }
    public string? IdentificationCodeQualifier { get; init; }
    public string? ResponseContactIdentifier { get; init; }
    public string? LastName { get; init; }
    public string? SecondLastName { get; init; }
    public string? FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public string? OrganizationName { get; init; }
    public string? AdditionalOrganizationName { get; init; }
    public string? Relationship { get; init; }
}

public static class PatientTableExtensions
{
    extension(PatientTable)
    {
        public static PatientTable New(Person person) =>
            new()
            {
                EntityType = "Person",
                IdentificationCodeQualifier = person.IdentificationCodeQualifier,
                ResponseContactIdentifier = person.ResponseContactIdentifier,
                LastName = person.LastName,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                Prefix = person.Prefix,
                Suffix = person.Suffix,
            };

        public static PatientTable ToPatientTable(IndividualOrOrganization entity) =>
            entity switch
            {
                Person person => new PatientTable
                {
                    EntityType = nameof(Person),
                    EntityIdentifierCode = person.EntityIdentifierCode,
                    IdentificationCodeQualifier = person.IdentificationCodeQualifier,
                    ResponseContactIdentifier = person.ResponseContactIdentifier,
                    LastName = person.LastName,
                    SecondLastName = person.SecondLastName,
                    FirstName = person.FirstName,
                    MiddleName = person.MiddleName,
                    Prefix = person.Prefix,
                    Suffix = person.Suffix,
                    Relationship = person.Relationship?.GetEnumMemberValue(),
                },
                NonPerson nonPerson => new PatientTable
                {
                    EntityType = nameof(NonPerson),
                    EntityIdentifierCode = nonPerson.EntityIdentifierCode,
                    IdentificationCodeQualifier = nonPerson.IdentificationCodeQualifier,
                    ResponseContactIdentifier = nonPerson.ResponseContactIdentifier,
                    OrganizationName = nonPerson.OrganizationName,
                    AdditionalOrganizationName = nonPerson.AdditionalOrganizationName,
                    Relationship = nonPerson.Relationship?.GetEnumMemberValue(),
                },
                _ => throw new ArgumentOutOfRangeException(
                    nameof(entity),
                    entity,
                    "Unsupported entity type."
                ),
            };
    }
}
