using System.Text.Json.Serialization;
using EdiFabric.Templates.Hipaa5010;

namespace PayerEDI.Data.Models;

// NM1 segment definition -> https://www.stedi.com/edi/x12-005010/segment/NM1

/// <summary>
/// Common identity data mapped from an X12 NM1 segment.
/// </summary>
/// <remarks>
/// <para>
/// NM101 identifies the entity's role in the transaction. NM108 qualifies the
/// identifier stored in NM109; the qualifier must be retained because the same
/// identifier field can contain an NPI, member identifier, tax identifier,
/// submitter identifier, or another identifier type depending on the code.
/// </para>
/// <para>
/// The NM1 factory methods require NM101, NM102, NM103, NM108, and NM109 to be
/// present and non-blank. They trim accepted string values and throw
/// <see cref="InvalidNm1Exception"/> when a required value is missing or when
/// NM102 contains an unsupported entity type. Optional name fields and NM110
/// relationship data remain nullable.
/// </para>
/// <para>
/// NM102 value <c>1</c> creates a <see cref="Person"/>; value <c>2</c> creates a
/// <see cref="NonPerson"/>. NM110 is converted to
/// <see cref="EntityRelationshipCode"/> when its code is recognized.
/// </para>
/// </remarks>
[JsonPolymorphic]
[JsonDerivedType(typeof(Person), "person")]
[JsonDerivedType(typeof(NonPerson), "nonPerson")]
// These attributes are required because these values are exposed through the
// abstract base type; System.Text.Json otherwise serializes only base properties
// and omits fields declared on Person or NonPerson.
public abstract record IndividualOrOrganization(
    string? EntityIdentifierCode,
    string? IdentificationCodeQualifier,
    string? ResponseContactIdentifier
);

/// <summary>
/// NM1 - NM102. Entity Type Qualifier code = 1 for a person entity.
/// </summary>
/// <remarks>
/// Represents an individual person identified by the shared NM1 segment fields.
/// </remarks>
/// <param name="EntityIdentifierCode">NM1-01: Entity Identifier Code identifying the person's role in the transaction.</param>
/// <param name="LastName">NM1-03: Primary Name Last or Organization Name. For a person, this is the person's primary last name. NM103 is required when NM112 is present.</param>
/// <param name="SecondLastName">NM1-12: Additional Name Last or Organization Name. For a person, this may contain a second surname.</param>
/// <param name="FirstName">NM1-04: Optional Name First value.</param>
/// <param name="MiddleName">NM1-05: Optional Name Middle value.</param>
/// <param name="Prefix">NM1-06: Optional Name Prefix value.</param>
/// <param name="Suffix">NM1-07: Optional Name Suffix value.</param>
/// <param name="IdentificationCodeQualifier">NM1-08: Identification Code Qualifier describing the type of identifier stored in NM109.</param>
/// <param name="ResponseContactIdentifier">NM1-09: Identification Code identifying the person. NM108 and NM109 are required together.</param>
/// <param name="Relationship">NM1-10: Entity Relationship Code describing the person's relationship to the NM101 entity. It is nullable when NM110 is absent or not recognized.</param>
public sealed record Person(
    string? EntityIdentifierCode,
    string LastName,
    string? SecondLastName,
    string? FirstName,
    string? MiddleName,
    string? Prefix,
    string? Suffix,
    string? IdentificationCodeQualifier,
    string? ResponseContactIdentifier,
    EntityRelationshipCode? Relationship
)
    : IndividualOrOrganization(
        EntityIdentifierCode,
        IdentificationCodeQualifier,
        ResponseContactIdentifier
    );

/// <summary>
/// NM1 - NM102. Entity Type Qualifier code = 2 for a non-person entity, such as a corporation.
/// </summary>
/// <remarks>
/// Represents a non-person entity such as a corporation, facility, payer, or other organization.
/// The values are mapped from the shared NM1 segment fields used to identify the entity.
/// </remarks>
/// <param name="EntityIdentifierCode">NM1-01: Entity Identifier Code identifying the organization's role in the transaction.</param>
/// <param name="OrganizationName">NM1-03: Primary Name Last or Organization Name. For a non-person entity, this is the organization's primary name. NM103 is required when NM112 is present.</param>
/// <param name="AdditionalOrganizationName">NM1-12: Additional Name Last or Organization Name. For a non-person entity, this may contain a secondary or overflow organization name.</param>
/// <param name="IdentificationCodeQualifier">NM1-08: Identification Code Qualifier describing the type of identifier stored in NM109.</param>
/// <param name="ResponseContactIdentifier">NM1-09: Identification Code identifying the organization or other entity. NM108 and NM109 are required together.</param>
/// <param name="Relationship">NM1-10: Entity Relationship Code describing the entity's relationship to the NM101 entity. It is nullable when NM110 is absent or not recognized. C1110: If NM1-11 is present, then NM1-10 is required</param>
public sealed record NonPerson(
    string? EntityIdentifierCode,
    string OrganizationName,
    string? AdditionalOrganizationName,
    string IdentificationCodeQualifier,
    string ResponseContactIdentifier,
    EntityRelationshipCode? Relationship
)
    : IndividualOrOrganization(
        EntityIdentifierCode,
        IdentificationCodeQualifier,
        ResponseContactIdentifier
    );
