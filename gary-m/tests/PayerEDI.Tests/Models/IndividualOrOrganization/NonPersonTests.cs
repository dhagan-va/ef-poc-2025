using Bogus;
using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Models;
using PayerEDI.Data.Models.Factory;

namespace PayerEDI.Tests.Models.IndividualOrOrganization;

public class NonPersonTests
{
    private readonly Faker<NM1_InformationReceiverName_4> _faker;

    public NonPersonTests()
    {
        _faker = new Faker<NM1_InformationReceiverName_4>()
            .RuleFor(o => o.EntityTypeQualifier_02, f => "2")
            .RuleFor(o => o.EntityIdentifierCode_01, f => "41")
            .RuleFor(o => o.IdentificationCodeQualifier_08, f => "46")
            .RuleFor(o => o.ResponseContactIdentifier_09, f => f.Random.AlphaNumeric(10))
            .RuleFor(o => o.ResponseContactLastorOrganizationName_03, f => f.Company.CompanyName())
            .RuleFor(o => o.NameLastorOrganizationName_12, f => (string?)null)
            .RuleFor(
                o => o.EntityRelationshipCode_10,
                f => f.Random.Enum<EntityRelationshipCode>().ToCode()
            )
            .RuleFor(
                o => o.EntityIdentifierCode_11,
                f => "1P"
            ) // hard coding to Provider there are 1500 possible codes that can be fed into this
        ;
    }

    [Fact]
    public void ConstructNonPersonSubmitter()
    {
        var nonPersonNm1 = _faker.Generate();
        var individualOrOrg = Data.Models.IndividualOrOrganization.NewSubmitter(nonPersonNm1);

        Assert.IsType<NonPerson>(individualOrOrg);
        NonPerson nonPerson = (NonPerson)individualOrOrg;

        Assert.NotNull(nonPerson.OrganizationName);
        Assert.Equal(
            nonPersonNm1.ResponseContactIdentifier_09,
            nonPerson.ResponseContactIdentifier
        );
        Assert.Equal(nonPersonNm1.EntityIdentifierCode_01, nonPerson.EntityIdentifierCode);
        Assert.Equal(
            nonPersonNm1.IdentificationCodeQualifier_08,
            nonPerson.IdentificationCodeQualifier
        );
        Assert.Equal(
            nonPersonNm1.ResponseContactLastorOrganizationName_03,
            nonPerson.OrganizationName
        );
        Assert.Null(nonPerson.AdditionalOrganizationName);
        Assert.NotNull(nonPerson.Relationship);
    }

    [Fact]
    public void ConstructNonPersonSubmitterWithAdditionalOrganizationName()
    {
        var nonPersonNm1 = _faker
            .RuleFor(o => o.NameLastorOrganizationName_12, f => f.Random.AlphaNumeric(10))
            .Generate();
        var individualOrOrg = Data.Models.IndividualOrOrganization.NewSubmitter(nonPersonNm1);

        Assert.IsType<NonPerson>(individualOrOrg);
        NonPerson nonPerson = (NonPerson)individualOrOrg;

        Assert.NotNull(nonPerson.OrganizationName);
        Assert.Equal(
            nonPersonNm1.ResponseContactIdentifier_09,
            nonPerson.ResponseContactIdentifier
        );
        Assert.Equal(nonPersonNm1.EntityIdentifierCode_01, nonPerson.EntityIdentifierCode);
        Assert.Equal(
            nonPersonNm1.IdentificationCodeQualifier_08,
            nonPerson.IdentificationCodeQualifier
        );
        Assert.Equal(
            nonPersonNm1.ResponseContactLastorOrganizationName_03,
            nonPerson.OrganizationName
        );
        Assert.NotNull(nonPerson.AdditionalOrganizationName);
        Assert.Equal(
            nonPersonNm1.NameLastorOrganizationName_12,
            nonPerson.AdditionalOrganizationName
        );
    }
}
