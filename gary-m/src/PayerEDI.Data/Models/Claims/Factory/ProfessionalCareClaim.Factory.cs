using System.Globalization;
using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Models.Factory;

namespace PayerEDI.Data.Models.Claims.Factory;

public static class ProfessionalCareClaimFactory
{
    private static readonly string[] ClaimDateFormat = ["yyyyMMdd", "yyMMdd"];
    private static readonly string[] ClaimTimeFormat = ["HHmm", "HHmmss", "HHmmssf", "HHmmssff"];

    extension(ProfessionalCareClaim)
    {
        public static ProfessionalCareClaim New(string claimDate, string claimTime, TS837P claim)
        {
            if (string.IsNullOrWhiteSpace(claimDate) || string.IsNullOrWhiteSpace(claimTime))
            {
                throw new ArgumentException(
                    "Claim Date and Claim Time are required and cannot be empty."
                );
            }

            if (
                DateOnly.TryParseExact(
                    claimDate,
                    ClaimDateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date
                )
                && TimeOnly.TryParseExact(
                    claimTime,
                    ClaimTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var time
                )
            )
            {
                var submitter = ClaimSubmitter.New(claim);

                var receiver = IndividualOrOrganization.NewReceiver(
                    claim.AllNM1.Loop1000B.NM1_ReceiverName
                );
                var subscribers = Subscriber.New(claim);
                var healthcareProviders = HealthcareProvider.New(claim);
                var procedures = Procedure.New(claim);

                return new ProfessionalCareClaim(
                    TransactedAt: date.ToDateTime(time),
                    Submitter: submitter,
                    Receiver: receiver,
                    Subscribers: subscribers,
                    HealthcareProviders: healthcareProviders,
                    Procedures: procedures
                );
            }

            throw new ArgumentException(
                $"Claim Date must be formatted as {ClaimDateFormat} and claimTime must be formatted as {ClaimDateFormat}"
            );
        }
    }
}
