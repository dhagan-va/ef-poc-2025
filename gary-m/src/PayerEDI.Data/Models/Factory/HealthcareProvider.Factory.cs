using EdiFabric.Templates.Hipaa5010;

namespace PayerEDI.Data.Models.Factory;

public static class HealthcareProviderFactory
{
    extension(HealthcareProvider)
    {
        private static List<HealthcareProvider> New(All_NM1_837P_3 nm1) =>
            nm1
                .Loop2310A.Select(Person.New)
                .Select(p => new ReferringProvider(p))
                .ToList<HealthcareProvider>();

        private static List<HealthcareProvider> New(All_NM1_837D_3 nm1) =>
            nm1
                .Loop2310A.Select(Person.New)
                .Select(p => new ReferringProvider(p))
                .ToList<HealthcareProvider>();

        public static List<HealthcareProvider> New(TS837P claim) =>
            claim
                .Loop2000A.SelectMany(billing => billing.Loop2000B)
                .SelectMany(subscriber => subscriber.Loop2000C)
                .SelectMany(o => o.Loop2300)
                .Where(o => o.AllNM1 != null)
                .SelectMany(healthcareProvider => HealthcareProvider.New(healthcareProvider.AllNM1))
                .ToList();

        public static List<HealthcareProvider> New(TS837D claim) =>
            claim
                .Loop2000A.SelectMany(billing => billing.Loop2000B)
                .SelectMany(subscriber => subscriber.Loop2000C)
                .SelectMany(o => o.Loop2300)
                .SelectMany(healthcareProvider => HealthcareProvider.New(healthcareProvider.AllNM1))
                .ToList();
    }
}
