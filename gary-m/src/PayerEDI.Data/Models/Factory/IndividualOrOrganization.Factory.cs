using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Exceptions;
using PayerEDI.Data.Helpers;

namespace PayerEDI.Data.Models.Factory;

public static class PersonFactory
{
    extension(Person)
    {
        public static Person New(NM1 receiverName) =>
            new(
                EntityIdentifierCode: receiverName.RequireNm1(
                    x => x.EntityIdentifierCode_01,
                    "NM101"
                ),
                LastName: receiverName.RequireNm1(
                    x => x.ResponseContactLastorOrganizationName_03,
                    "NM103"
                ),
                SecondLastName: receiverName.NameLastorOrganizationName_12?.Trim(),
                FirstName: receiverName.ResponseContactFirstName_04?.Trim(),
                MiddleName: receiverName.ResponseContactMiddleName_05?.Trim(),
                Prefix: receiverName.NamePrefix_06,
                Suffix: receiverName.ResponseContactNameSuffix_07,
                IdentificationCodeQualifier: receiverName.IdentificationCodeQualifier_08?.Trim(),
                ResponseContactIdentifier: receiverName.ResponseContactIdentifier_09?.Trim(),
                Relationship: EntityRelationshipCode.FromCode(
                    receiverName.EntityRelationshipCode_10
                )
            );

        public static Person New(Loop_2310A_837P provider) =>
            Person.New(provider.NM1_ReferringProviderName);

        public static Person New(Loop_2310A_837D provider) =>
            Person.New(provider.NM1_ReferringProviderName);
    }
}

public static class NonPersonFactory
{
    extension(NonPerson)
    {
        public static NonPerson New(NM1 receiverName) =>
            new(
                EntityIdentifierCode: receiverName.RequireNm1(
                    x => x.EntityIdentifierCode_01,
                    "NM101"
                ),
                OrganizationName: receiverName.RequireNm1(
                    x => x.ResponseContactLastorOrganizationName_03,
                    "NM103"
                ), // Individual last name or organizational name, NM112 is present then NM103 is required
                AdditionalOrganizationName: receiverName.NameLastorOrganizationName_12, // C1203: If NM1-12 is present, then NM1-03 is required, NM112 can identify a second surname.
                IdentificationCodeQualifier: receiverName.RequireNm1(
                    x => x.IdentificationCodeQualifier_08,
                    "NM108"
                ), // P0809: If either NM1-08 or NM1-09 is present, then the other is required
                ResponseContactIdentifier: receiverName.RequireNm1(
                    x => x.ResponseContactIdentifier_09,
                    "NM109"
                ), // NM109 - Code identifying a party or other code
                Relationship: EntityRelationshipCode.FromCode(
                    receiverName.EntityRelationshipCode_10
                ) // NM110 - Code describing entity relationship
            );
    }
}

public static class IndividualOrOrganizationFactory
{
    extension(IndividualOrOrganization)
    {
        private static IndividualOrOrganization NewNm1(NM1 nm1) => // keeping this private for now to make a more readable API call
            nm1.RequireNm1(x => x.EntityTypeQualifier_02, "NM102") switch // Note: there are 16 codes, 1 and 2 are for my naive implementation. See: https://www.stedi.com/edi/x12-005010/segment/NM1#NM1-02
            {
                "1" => Person.New(nm1),
                "2" => NonPerson.New(nm1),
                _ => throw new InvalidNm1Exception(
                    $"entity type qualifier {nm1.EntityTypeQualifier_02} not handled"
                ),
            };

        public static IndividualOrOrganization NewSubmitter(
            NM1_InformationReceiverName_4 submitterName // from what I can tell even though it has Receiver in the name this is the submitter name property
        ) => IndividualOrOrganization.NewNm1(submitterName);

        public static IndividualOrOrganization NewSubscriber(NM1_SubscriberName_2 subscriber) =>
            IndividualOrOrganization.NewNm1(subscriber);

        public static IndividualOrOrganization NewReceiver(NM1_ReceiverName receiverName) =>
            IndividualOrOrganization.NewNm1(receiverName); // see above comment, but keep in mind this seems less dumb

        public static IndividualOrOrganization NewSubscriber(NM1_SubscriberName_5 subscriber) =>
            IndividualOrOrganization.NewNm1(subscriber);

        public static IndividualOrOrganization NewDependent(Loop_2010CA_837P dependent) =>
            IndividualOrOrganization.NewNm1(dependent.NM1_PatientName);

        public static IndividualOrOrganization NewDependent(Loop_2010CA_837D dependent) =>
            IndividualOrOrganization.NewNm1(dependent.NM1_PatientName);
    }
}
