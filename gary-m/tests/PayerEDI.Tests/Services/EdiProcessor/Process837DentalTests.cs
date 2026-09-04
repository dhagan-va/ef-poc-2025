using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Models;
using PayerEDI.Data.Models.Claims;
using PayerEDI.Tests.Fixtures;

namespace PayerEDI.Tests.Services.EdiProcessor;

public class Process837Dental
    : IClassFixture<TestLoggingFixture>,
        IClassFixture<TestEdiFabricFixture>
{
    private readonly Data.Services.EdiFabricEdiProcessor _processor;

    public Process837Dental(TestLoggingFixture logging, TestEdiFabricFixture testEdiFabricFixture)
    {
        _ = testEdiFabricFixture;
        _processor = new Data.Services.EdiFabricEdiProcessor(
            logging.CreateLogger<Data.Services.EdiFabricEdiProcessor>()
        );
    }

    [Fact]
    public void ProcessSample3()
    {
        var samplePath = Path.Combine(
            AppContext.BaseDirectory,
            "samples",
            "EDI",
            "837d-sample-3.edi"
        );

        using var ediStream = File.OpenRead(samplePath);

        var claims = _processor.ProcessEdi(ediStream);

        Assert.NotEmpty(claims);
        Assert.Single(claims);
        var (edi, firstClaim) = claims[0];

        Assert.IsType<TS837D>(edi);
        DentalCareClaim dentalCareClaim = Assert.IsType<DentalCareClaim>(firstClaim);

        Assert.Equal(new DateTime(2008, 5, 3, 17, 5, 0), dentalCareClaim.TransactedAt);

        // test submitter
        Assert.Equal("41", dentalCareClaim.Submitter.Submitter.EntityIdentifierCode);
        Assert.IsType<ClaimSubmitter>(dentalCareClaim.Submitter);
        NonPerson submitter = (NonPerson)dentalCareClaim.Submitter.Submitter;
        Assert.Equal("PREMIER BILLING SERVICE", submitter.OrganizationName);
        Assert.Null(submitter.AdditionalOrganizationName);
        Assert.Equal("41", submitter.EntityIdentifierCode);
        Assert.Equal("46", submitter.IdentificationCodeQualifier);
        Assert.Equal("TGJ23", submitter.ResponseContactIdentifier);
        Assert.Equal("46", dentalCareClaim.Submitter.ExternalIdentifier.Qualifier);
        Assert.Equal("TGJ23", dentalCareClaim.Submitter.ExternalIdentifier.Value);

        // test admin comm contact
        Assert.NotNull(dentalCareClaim.Submitter.AdministrativeCommunicationsContact);
        Assert.Single(dentalCareClaim.Submitter.AdministrativeCommunicationsContact);
        CommunicationsContact communicationsContact = dentalCareClaim
            .Submitter
            .AdministrativeCommunicationsContact[0];
        Assert.Equal("IC", communicationsContact.ContactFunctionCode);
        Assert.Equal("JERRY", communicationsContact.Name);
        Assert.NotNull(communicationsContact.PrimaryNumber);
        Assert.Equal("7176149999", communicationsContact.PrimaryNumber.Number);
        Assert.Equal(
            CommunicationNumberQualifier.Telephone,
            communicationsContact.PrimaryNumber.Qualifier
        );
        Assert.Null(communicationsContact.SecondaryNumber);
        Assert.Null(communicationsContact.TertiaryNumber);

        // test receiver
        Assert.NotNull(dentalCareClaim.Receiver);
        Assert.IsType<NonPerson>(dentalCareClaim.Receiver);
        NonPerson receiver = (NonPerson)dentalCareClaim.Receiver;
        Assert.Equal("INSURANCE COMPANY XYZ", receiver.OrganizationName);
        Assert.Equal("40", receiver.EntityIdentifierCode);
        Assert.Equal("46", receiver.IdentificationCodeQualifier);
        Assert.Equal("66783JJT", receiver.ResponseContactIdentifier);

        // test subscriber
        Assert.NotNull(dentalCareClaim.Subscribers);
        Assert.Single(dentalCareClaim.Subscribers);
        Subscriber subscriber = dentalCareClaim.Subscribers[0];
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
        Assert.Equal("111223333", primary.ResponseContactIdentifier);
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
        Assert.Equal(2, dentalCareClaim.Procedures.Count);
        Assert.Equal("D2150", dentalCareClaim.Procedures[0].ProcedureCode);
        Assert.Equal("100", dentalCareClaim.Procedures[0].ChargeAmount);

        // test healthcare providers
        Assert.NotEmpty(dentalCareClaim.HealthcareProviders);

        var healthCareProvider = dentalCareClaim.HealthcareProviders[0];

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
