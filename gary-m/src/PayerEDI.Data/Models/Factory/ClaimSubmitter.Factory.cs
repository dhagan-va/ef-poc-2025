using EdiFabric.Templates.Hipaa5010;

namespace PayerEDI.Data.Models.Factory;

public static class ClaimSubmitterFactory
{
    extension(ClaimSubmitter)
    {
        public static ClaimSubmitter New(TS837P claim)
        {
            var submitter = IndividualOrOrganization.NewSubmitter(
                claim.AllNM1.Loop1000A.NM1_SubmitterName
            );
            var administrativeCommunicationsContact = CommunicationsContact.New(
                claim.AllNM1.Loop1000A.PER_SubmitterEDIContactInformation
            );

            var externalIdentifier = new ExternalIdentifier(
                submitter.IdentificationCodeQualifier,
                submitter.ResponseContactIdentifier
            );

            return new ClaimSubmitter(
                submitter,
                administrativeCommunicationsContact,
                externalIdentifier
            ); // TODO: Handle submitter and acc are required
        }

        public static ClaimSubmitter New(TS837D claim)
        {
            var submitter = IndividualOrOrganization.NewSubmitter(
                claim.AllNM1.Loop1000A.NM1_SubmitterName
            );
            var administrativeCommunicationsContact = CommunicationsContact.New(
                claim.AllNM1.Loop1000A.PER_SubmitterEDIContactInformation
            );

            var externalIdentifier = new ExternalIdentifier(
                submitter.IdentificationCodeQualifier,
                submitter.ResponseContactIdentifier
            );

            return new ClaimSubmitter(
                submitter,
                administrativeCommunicationsContact,
                externalIdentifier
            ); // TODO: Handle submitter and acc are required
        }
    }
}
