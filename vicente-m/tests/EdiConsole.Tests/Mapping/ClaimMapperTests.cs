using EdiConsole.Domain.Claims;
using EdiConsole.Domain.Parties;
using EdiConsole.Mapping;
using EdiConsole.Tests.Support;

namespace EdiConsole.Tests.Mapping;

/// <summary>
/// Maps the real sample files and checks the result against what the files say.
/// Each test pins down one rule, so a failure names the rule that broke.
/// </summary>
public sealed class ClaimMapperTests(ParsedSamples samples) : IClassFixture<ParsedSamples>
{
    private readonly ClaimMapper _mapper = new();

    // ── 837p-sample.edi: two subscribers, each the patient (claims under 2000B) ─────────────────

    [Fact]
    public void Maps_every_claim_under_2000B()
    {
        var claims = _mapper.Map(samples.SubscriberIsPatient);

        Assert.Equal(["1000A", "1001A"], claims.Select(c => c.PatientControlNumber));
    }

    [Fact]
    public void Maps_claim_header()
    {
        var claim = FirstClaim(samples.SubscriberIsPatient);

        Assert.Equal("1239", claim.TransactionSetControlNumber);    // ST02
        Assert.Equal(140m, claim.TotalCharge);                      // CLM02
        Assert.Equal("19", claim.PlaceOfServiceCode);               // CLM05-1
        Assert.Equal("1", claim.FrequencyCode);                     // CLM05-3
    }

    [Fact]
    public void Maps_billing_provider()
    {
        var provider = FirstClaim(samples.SubscriberIsPatient).BillingProvider;

        Assert.Equal("1122334455", provider.Npi);
        Assert.Equal("BILLING PROVIDER", provider.Name.LastOrOrganizationName);
        Assert.Equal("999999999", provider.TaxId);
        Assert.Equal(
            new Address { Line1 = "1234 SOME ROAD", City = "CHICAGO", State = "IL", PostalCode = "606739999" },
            provider.Address);
    }

    [Fact]
    public void Maps_subscriber()
    {
        var subscriber = FirstClaim(samples.SubscriberIsPatient).Subscriber;

        Assert.Equal("1234567890", subscriber.MemberId);
        Assert.Equal(new PartyName { LastOrOrganizationName = "BLOGGS", FirstName = "JOE" }, subscriber.Name);
        Assert.Equal(new DateOnly(1957, 1, 11), subscriber.DateOfBirth);
        Assert.Equal("M", subscriber.Gender);
        Assert.Equal("P", subscriber.PayerResponsibility);         // SBR01
        Assert.Equal("12", subscriber.ClaimFilingIndicator);       // SBR09
        Assert.Equal("1 SOME BLVD", subscriber.Address.Line1);
    }

    [Fact]
    public void Copies_subscriber_into_patient_when_there_is_no_2000C()
    {
        var claim = FirstClaim(samples.SubscriberIsPatient);

        Assert.True(claim.Patient.IsSubscriber);
        Assert.Equal(Patient.SelfRelationshipCode, claim.Patient.RelationshipToSubscriber);
        Assert.Equal(claim.Subscriber.Name, claim.Patient.Name);
        Assert.Equal(claim.Subscriber.DateOfBirth, claim.Patient.DateOfBirth);
        Assert.Equal(claim.Subscriber.Gender, claim.Patient.Gender);
    }

    [Fact]
    public void Maps_payer()
    {
        var payer = FirstClaim(samples.SubscriberIsPatient).Payer;

        Assert.Equal("12345", payer.PayerId);
        Assert.Equal("PAYER", payer.Name);
    }

    [Fact]
    public void Maps_principal_diagnosis()
    {
        var diagnosis = Assert.Single(FirstClaim(samples.SubscriberIsPatient).Diagnoses);

        Assert.Equal(1, diagnosis.Sequence);
        Assert.Equal("ABK", diagnosis.Qualifier);
        Assert.Equal("I10", diagnosis.Code);
    }

    [Fact]
    public void Maps_service_line()
    {
        var line = Assert.Single(FirstClaim(samples.SubscriberIsPatient).ServiceLines);

        Assert.Equal(1, line.LineNumber);                           // LX01
        Assert.Equal("99213", line.ProcedureCode);                  // SV101-2
        Assert.Null(line.Modifier1);
        Assert.Equal(140m, line.Charge);                            // SV102
        Assert.Equal("UN", line.UnitBasis);                         // SV103
        Assert.Equal(1m, line.Units);                               // SV104
        Assert.Null(line.PlaceOfServiceCode);                       // SV105 not sent: the claim's applies
        Assert.Equal(1, line.DiagnosisPointer1);                    // SV107-1
        Assert.Null(line.DiagnosisPointer2);
        Assert.Equal(new DateOnly(2015, 11, 24), line.ServiceDateFrom);   // DTP*472 D8
        Assert.Null(line.ServiceDateTo);
    }

    // ── 837-sample-file-2.edi: the patient is a dependent (claim under 2000C) ───────────────────

    [Fact]
    public void Maps_claim_under_2000C()
    {
        var claim = Assert.Single(_mapper.Map(samples.DependentIsPatient));

        Assert.Equal("26463774", claim.PatientControlNumber);
        Assert.Equal(100m, claim.TotalCharge);
        Assert.Equal("11", claim.PlaceOfServiceCode);
    }

    [Fact]
    public void Maps_dependent_as_patient()
    {
        var patient = FirstClaim(samples.DependentIsPatient).Patient;

        Assert.False(patient.IsSubscriber);
        Assert.Equal("19", patient.RelationshipToSubscriber);      // PAT01: child
        Assert.Equal(new PartyName { LastOrOrganizationName = "SMITH", FirstName = "TED" }, patient.Name);
        Assert.Equal(new DateOnly(1973, 5, 1), patient.DateOfBirth);
        Assert.Equal("M", patient.Gender);
    }

    [Fact]
    public void Keeps_the_subscriber_separate_from_a_dependent_patient()
    {
        var subscriber = FirstClaim(samples.DependentIsPatient).Subscriber;

        Assert.Equal("JS00111223333", subscriber.MemberId);
        Assert.Equal(new PartyName { LastOrOrganizationName = "SMITH", FirstName = "JANE" }, subscriber.Name);
        Assert.Equal(new DateOnly(1943, 5, 1), subscriber.DateOfBirth);
        Assert.Equal("F", subscriber.Gender);
        Assert.Equal("2222-SJ", subscriber.GroupNumber);           // SBR03
    }

    [Fact]
    public void Keeps_HI_position_as_sequence()
    {
        var diagnoses = FirstClaim(samples.DependentIsPatient).Diagnoses;

        Assert.Equal(
            [(1, "BK", "0340"), (2, "BF", "V7389")],
            diagnoses.Select(d => ((int)d.Sequence, d.Qualifier, d.Code)));
    }

    [Fact]
    public void Maps_every_service_line_with_its_pointer()
    {
        var lines = FirstClaim(samples.DependentIsPatient).ServiceLines;

        Assert.Equal(
            [
                (1, "99213", 40m, 1, new DateOnly(2006, 10, 3)),
                (2, "87070", 15m, 1, new DateOnly(2006, 10, 3)),
                (3, "99214", 35m, 2, new DateOnly(2006, 10, 10)),
                (4, "86663", 10m, 2, new DateOnly(2006, 10, 10)),
            ],
            lines.Select(l => (l.LineNumber, l.ProcedureCode, l.Charge, (int)l.DiagnosisPointer1, l.ServiceDateFrom)));
    }

    [Fact]
    public void Every_pointer_refers_to_a_mapped_diagnosis()
    {
        var claim = FirstClaim(samples.DependentIsPatient);
        var sequences = claim.Diagnoses.Select(d => d.Sequence).ToHashSet();

        Assert.All(claim.ServiceLines, line => Assert.Contains(line.DiagnosisPointer1, sequences));
    }

    private Claim FirstClaim(EdiFabric.Templates.Hipaa5010.TS837P transaction) => _mapper.Map(transaction)[0];
}
