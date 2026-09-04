using EdiFabric.Templates.Hipaa5010;

namespace PayerEDI.Data.Models.Factory;

public static class CommunicationsContactExtensions
{
    extension(CommunicationsContact)
    {
        private static CommunicationsContact New(PER_BillingProviderContactInformation contactInfo)
        {
            if (string.IsNullOrWhiteSpace(contactInfo.ContactFunctionCode_01))
            {
                throw new ArgumentException(
                    "PER Billing Provider Contact Information Contact Function Code not allowed to be Empty or Null.", // https://www.stedi.com/edi/x12-005010/segment/PER#PER-01
                    nameof(contactInfo)
                );
            }

            return new CommunicationsContact(
                ContactFunctionCode: contactInfo.ContactFunctionCode_01,
                Name: contactInfo.ResponseContactName_02,
                PrimaryNumber: CommunicationNumber.MaybeNew(
                    contactInfo.ResponseContactCommunicationNumber_04,
                    contactInfo.CommunicationNumberQualifier_03
                ),
                SecondaryNumber: CommunicationNumber.MaybeNew(
                    contactInfo.ResponseContactCommunicationNumber_06,
                    contactInfo.CommunicationNumberQualifier_05
                ),
                TertiaryNumber: CommunicationNumber.MaybeNew(
                    contactInfo.ResponseContactCommunicationNumber_08,
                    contactInfo.CommunicationNumberQualifier_07
                )
            );
        }

        private static CommunicationsContact? MaybeNew(
            PER_BillingProviderContactInformation? contactInfo
        )
        {
            if (
                contactInfo == null
                || string.IsNullOrWhiteSpace(contactInfo.ContactFunctionCode_01)
            )
            {
                return null;
            }

            return CommunicationsContact.New(contactInfo);
        }

        public static List<CommunicationsContact> New(
            IList<PER_BillingProviderContactInformation> contactInfos
        ) =>
            contactInfos
                .Select(CommunicationsContact.MaybeNew)
                .OfType<CommunicationsContact>()
                .ToList();
    }
}
