using EdiFabric.Templates.Hipaa5010;

namespace PayerEDI.Data.Models.Factory;

public static class SubscriberFactory
{
    extension(Subscriber)
    {
        public static List<Subscriber> New(TS837P claim) =>
            claim
                .Loop2000A.SelectMany(billingProvider => billingProvider.Loop2000B)
                .Select(subscriber => new Subscriber(
                    Primary: IndividualOrOrganization.NewSubscriber(
                        subscriber.AllNM1.Loop2010BA.NM1_SubscriberName
                    ),
                    Dependents: subscriber
                        .Loop2000C.Select(single2000C =>
                            IndividualOrOrganization.NewDependent(single2000C.Loop2010CA)
                        )
                        .ToList()
                ))
                .ToList();

        public static List<Subscriber> New(TS837D claim) =>
            claim
                .Loop2000A.SelectMany(billingProvider => billingProvider.Loop2000B)
                .Select(subscriber => new Subscriber(
                    Primary: IndividualOrOrganization.NewSubscriber(
                        subscriber.AllNM1.Loop2010BA.NM1_SubscriberName
                    ),
                    Dependents: subscriber
                        .Loop2000C.Select(single2000C =>
                            IndividualOrOrganization.NewDependent(single2000C.Loop2010CA)
                        )
                        .ToList()
                ))
                .ToList();
    }
}
