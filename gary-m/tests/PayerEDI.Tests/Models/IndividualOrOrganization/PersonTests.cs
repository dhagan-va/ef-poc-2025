using Bogus;
using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Exceptions;
using PayerEDI.Data.Models;
using PayerEDI.Data.Models.Factory;
using Person = PayerEDI.Data.Models.Person;

namespace PayerEDI.Tests.Models.IndividualOrOrganization;

public class PersonTests
{
    private readonly Faker<NM1_InformationReceiverName_4> _faker;

    public PersonTests()
    {
        _faker = new Faker<NM1_InformationReceiverName_4>()
            .RuleFor(o => o.EntityIdentifierCode_01, _ => "41")
            .RuleFor(o => o.IdentificationCodeQualifier_08, _ => "46")
            .RuleFor(o => o.ResponseContactIdentifier_09, f => f.Random.AlphaNumeric(10))
            .RuleFor(o => o.NameLastorOrganizationName_12, f => f.Person.LastName)
            .RuleFor(
                o => o.ResponseContactLastorOrganizationName_03,
                (f, o) =>
                    o.EntityTypeQualifier_02 == "2" ? f.Company.CompanyName() : f.Person.LastName
            )
            .RuleFor(
                o => o.ResponseContactFirstName_04,
                (f, o) => o.EntityTypeQualifier_02 == "1" ? f.Person.FirstName : null
            )
            .RuleFor(
                o => o.ResponseContactMiddleName_05,
                (f, o) => o.EntityTypeQualifier_02 == "1" ? f.Person.FirstName : null
            )
            .RuleFor(
                o => o.EntityRelationshipCode_10,
                f => f.PickRandom<EntityRelationshipCode>().ToString()
            );
    }

    [Fact]
    public void ConstructPersonSubmitter()
    {
        var personNm1 = _faker.RuleFor(o => o.EntityTypeQualifier_02, _ => "1").Generate();
        var individualOrOrg = Data.Models.IndividualOrOrganization.NewSubmitter(personNm1);

        Assert.NotNull(individualOrOrg);
        Assert.Equal(
            personNm1.ResponseContactIdentifier_09,
            individualOrOrg.ResponseContactIdentifier
        );

        Assert.IsType<Person>(individualOrOrg);
        Person person = (Person)individualOrOrg;

        Assert.NotNull(person.LastName);
        Assert.Equal(personNm1.ResponseContactIdentifier_09, person.ResponseContactIdentifier);
        Assert.Equal(personNm1.EntityIdentifierCode_01, person.EntityIdentifierCode);
        Assert.Equal(personNm1.IdentificationCodeQualifier_08, person.IdentificationCodeQualifier);
        Assert.Equal(personNm1.ResponseContactLastorOrganizationName_03, person.LastName);
        Assert.Equal(personNm1.NameLastorOrganizationName_12, person.SecondLastName);
        Assert.Equal(personNm1.ResponseContactFirstName_04, person.FirstName);
        Assert.Equal(personNm1.ResponseContactMiddleName_05, person.MiddleName);
    }

    [Fact]
    public void MapsPrimaryAndSecondLastName_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.ResponseContactLastorOrganizationName_03, f => f.Person.LastName)
            .RuleFor(o => o.NameLastorOrganizationName_12, f => f.Random.String()) // f.Person.LastName is generated once per run, so using it twice results in the last name being the same for both parts
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal(personNm1.ResponseContactLastorOrganizationName_03, person.LastName);
        Assert.Equal(personNm1.NameLastorOrganizationName_12, person.SecondLastName);
        Assert.NotEqual(person.LastName, person.SecondLastName);
    }

    [Fact]
    public void MapsPersonNameFields_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.ResponseContactFirstName_04, _ => "MARIA")
            .RuleFor(o => o.ResponseContactMiddleName_05, _ => "ELENA")
            .RuleFor(o => o.NamePrefix_06, _ => "DR")
            .RuleFor(o => o.ResponseContactNameSuffix_07, _ => "JR")
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal("MARIA", person.FirstName);
        Assert.Equal("ELENA", person.MiddleName);
        Assert.Equal("DR", person.Prefix);
        Assert.Equal("JR", person.Suffix);
    }

    [Fact]
    public void MapsRecognizedRelationshipCode_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.EntityRelationshipCode_10, _ => "02")
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal(EntityRelationshipCode.Child, person.Relationship);
    }

    [Fact]
    public void LeavesRelationshipNullWhenAbsent_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.EntityRelationshipCode_10, _ => (string?)null)
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Null(person.Relationship);
    }

    [Fact]
    public void LeavesRelationshipNullWhenCodeIsUnrecognized_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.EntityRelationshipCode_10, _ => "ZZ")
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Null(person.Relationship);
    }

    [Fact]
    public void LeavesOptionalNameFieldsNullWhenAbsent_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.NameLastorOrganizationName_12, _ => (string?)null)
            .RuleFor(o => o.ResponseContactFirstName_04, _ => (string?)null)
            .RuleFor(o => o.ResponseContactMiddleName_05, _ => (string?)null)
            .RuleFor(o => o.NamePrefix_06, _ => (string?)null)
            .RuleFor(o => o.ResponseContactNameSuffix_07, _ => (string?)null)
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Null(person.SecondLastName);
        Assert.Null(person.FirstName);
        Assert.Null(person.MiddleName);
        Assert.Null(person.Prefix);
        Assert.Null(person.Suffix);
    }

    [Fact]
    public void RejectsMissingEntityIdentifierCode_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.EntityIdentifierCode_01, _ => " ")
            .Generate();

        var exception = Assert.Throws<InvalidNm1Exception>(() =>
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal("NM101 is required for an NM1 identity.", exception.Message);
    }

    [Fact]
    public void RejectsMissingEntityTypeQualifier_PersonSubmitter()
    {
        var personNm1 = _faker.RuleFor(o => o.EntityTypeQualifier_02, _ => null!).Generate();

        var exception = Assert.Throws<InvalidNm1Exception>(() =>
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal("NM102 is required for an NM1 identity.", exception.Message);
    }

    [Fact]
    public void RejectsMissingLastName_PersonSubmitter()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => "1")
            .RuleFor(o => o.ResponseContactLastorOrganizationName_03, _ => "\t")
            .Generate();

        var exception = Assert.Throws<InvalidNm1Exception>(() =>
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal("NM103 is required for an NM1 identity.", exception.Message);
    }

    [Fact]
    public void RejectsUnsupportedEntityTypeQualifier()
    {
        var personNm1 = _faker.RuleFor(o => o.EntityTypeQualifier_02, _ => "3").Generate();

        var exception = Assert.Throws<InvalidNm1Exception>(() =>
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Contains("entity type qualifier 3 not handled", exception.Message);
    }

    [Fact]
    public void TrimsRequiredNm1Values()
    {
        var personNm1 = _faker
            .RuleFor(o => o.EntityTypeQualifier_02, _ => " 1 ")
            .RuleFor(o => o.EntityIdentifierCode_01, _ => " 41 ")
            .RuleFor(o => o.ResponseContactLastorOrganizationName_03, _ => " SMITH ")
            .RuleFor(o => o.IdentificationCodeQualifier_08, _ => " MI ")
            .RuleFor(o => o.ResponseContactIdentifier_09, _ => " 12345 ")
            .Generate();

        var person = Assert.IsType<Person>(
            Data.Models.IndividualOrOrganization.NewSubmitter(personNm1)
        );

        Assert.Equal("41", person.EntityIdentifierCode);
        Assert.Equal("SMITH", person.LastName);
        Assert.Equal("MI", person.IdentificationCodeQualifier);
        Assert.Equal("12345", person.ResponseContactIdentifier);
    }
}
