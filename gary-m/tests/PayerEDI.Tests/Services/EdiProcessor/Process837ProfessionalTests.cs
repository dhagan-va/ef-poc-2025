using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Models;
using PayerEDI.Data.Models.Claims;
using PayerEDI.Tests.Fixtures;

namespace PayerEDI.Tests.Services.EdiProcessor;

public class Process837ProfessionalTests
    : IClassFixture<TestLoggingFixture>,
        IClassFixture<TestEdiFabricFixture>
{
    private readonly Data.Services.EdiFabricEdiProcessor _processor;

    public Process837ProfessionalTests(
        TestLoggingFixture logging,
        TestEdiFabricFixture testEdiFabricFixture
    )
    {
        _ = testEdiFabricFixture;
        _processor = new Data.Services.EdiFabricEdiProcessor(
            logging.CreateLogger<Data.Services.EdiFabricEdiProcessor>()
        );
    }

    [Fact]
    public void Process837ProfessionalAll()
    {
        var samplePath = Path.Combine(
            AppContext.BaseDirectory,
            "samples",
            "EDI",
            "837P-all-fields.edi"
        );

        using var ediStream = File.OpenRead(samplePath);

        var claims = _processor.ProcessEdi(ediStream);

        Assert.NotEmpty(claims);
        Assert.Single(claims);
        var (edi, firstClaim) = claims[0];

        Assert.IsType<TS837P>(edi);
        ProfessionalCareClaim professionalCareClaim = Assert.IsType<ProfessionalCareClaim>(
            firstClaim
        );

        Assert.Equal(new DateTime(2006, 10, 15, 17, 5, 0), professionalCareClaim.TransactedAt);

        // test submitter
        Assert.Equal("41", professionalCareClaim.Submitter.Submitter.EntityIdentifierCode);
        Assert.IsType<ClaimSubmitter>(professionalCareClaim.Submitter);
        NonPerson submitter = (NonPerson)professionalCareClaim.Submitter.Submitter;
        Assert.Equal("PREMIER BILLING SERVICE", submitter.OrganizationName);
        Assert.Null(submitter.AdditionalOrganizationName);
        Assert.Equal("41", submitter.EntityIdentifierCode);
        Assert.Equal("46", submitter.IdentificationCodeQualifier);
        Assert.Equal("TGJ23", submitter.ResponseContactIdentifier);
        Assert.Equal("46", professionalCareClaim.Submitter.ExternalIdentifier.Qualifier);
        Assert.Equal("TGJ23", professionalCareClaim.Submitter.ExternalIdentifier.Value);
        Assert.NotEqual(Guid.Empty, professionalCareClaim.Submitter.Id);
        Assert.Equal('7', professionalCareClaim.Submitter.Id.ToString()[14]);

        // test admin comm contact
        Assert.NotNull(professionalCareClaim.Submitter.AdministrativeCommunicationsContact);
        Assert.Single(professionalCareClaim.Submitter.AdministrativeCommunicationsContact);
        CommunicationsContact communicationsContact = professionalCareClaim
            .Submitter
            .AdministrativeCommunicationsContact[0];
        Assert.Equal("IC", communicationsContact.ContactFunctionCode);
        Assert.Equal("JERRY", communicationsContact.Name);
        Assert.NotNull(communicationsContact.PrimaryNumber);
        Assert.Equal("3055552222", communicationsContact.PrimaryNumber.Number);
        Assert.Equal(
            CommunicationNumberQualifier.Telephone,
            communicationsContact.PrimaryNumber.Qualifier
        );
        Assert.NotNull(communicationsContact.SecondaryNumber);
        Assert.Equal("231", communicationsContact.SecondaryNumber.Number);
        Assert.Equal(
            CommunicationNumberQualifier.TelephoneExtension,
            communicationsContact.SecondaryNumber.Qualifier
        );
        Assert.Null(communicationsContact.TertiaryNumber);

        // test receiver
        Assert.NotNull(professionalCareClaim.Receiver);
        Assert.IsType<NonPerson>(professionalCareClaim.Receiver);
        NonPerson receiver = (NonPerson)professionalCareClaim.Receiver;
        Assert.Equal("KEY INSURANCE COMPANY", receiver.OrganizationName);
        Assert.Equal("40", receiver.EntityIdentifierCode);
        Assert.Equal("46", receiver.IdentificationCodeQualifier);
        Assert.Equal("66783JJT", receiver.ResponseContactIdentifier);

        // test subscriber
        Assert.NotNull(professionalCareClaim.Subscribers);
        Assert.Single(professionalCareClaim.Subscribers);
        Subscriber subscriber = professionalCareClaim.Subscribers[0];
        Assert.NotEqual(Guid.Empty, subscriber.Id);
        Assert.Equal('7', subscriber.Id.ToString()[14]);
        Assert.IsType<Person>(subscriber.Primary);
        Person primary = (Person)subscriber.Primary;
        Assert.Equal("IL", primary.EntityIdentifierCode);
        Assert.Equal("SMITH", primary.LastName);
        Assert.Equal("JANE", primary.FirstName);
        Assert.Null(primary.MiddleName);
        Assert.Null(primary.Prefix);
        Assert.Null(primary.Suffix);
        Assert.Equal("MI", primary.IdentificationCodeQualifier);
        Assert.Equal("JS00111223333", primary.ResponseContactIdentifier);
        Assert.Null(primary.Relationship);

        // test dependent
        Assert.NotNull(subscriber.Dependents);
        Assert.Single(subscriber.Dependents);
        Assert.IsType<Person>(subscriber.Dependents[0]);
        Person dependent = (Person)subscriber.Dependents[0];
        Assert.Equal("QC", dependent.EntityIdentifierCode);
        Assert.Equal("SMITH", dependent.LastName);
        Assert.Equal("TED", dependent.FirstName);
        Assert.Null(dependent.MiddleName);
        Assert.Null(dependent.Prefix);
        Assert.Null(dependent.Suffix);
        Assert.Null(dependent.IdentificationCodeQualifier);
        Assert.Null(dependent.ResponseContactIdentifier);
        Assert.Null(dependent.Relationship);

        // test service lines
        Assert.Equal(4, professionalCareClaim.Procedures.Count);
        Assert.Equal("99299", professionalCareClaim.Procedures[0].ProcedureCode);
        Assert.Equal("40", professionalCareClaim.Procedures[0].ChargeAmount);
        Assert.Equal("RD8", professionalCareClaim.Procedures[0].ServiceDateFormatQualifier);

        // test healthcare providers
        Assert.Single(professionalCareClaim.HealthcareProviders);
        var healthCareProvider = professionalCareClaim.HealthcareProviders[0];

        Assert.Equal(
            new Person(
                EntityIdentifierCode: "DN",
                LastName: "DOE",
                SecondLastName: null,
                FirstName: "JONE",
                MiddleName: "C",
                Prefix: null,
                Suffix: null,
                IdentificationCodeQualifier: "XX",
                ResponseContactIdentifier: "5234567805",
                Relationship: null
            ),
            healthCareProvider.Provider
        );
    }
}
