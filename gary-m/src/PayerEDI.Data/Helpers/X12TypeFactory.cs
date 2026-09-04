using System.Reflection;
using EdiFabric.Core.Model.Edi.X12;

namespace PayerEDI.Data.Helpers;

public static class X12TypeFactory
{
    public static TypeInfo GetTypeInfo(ISA isa, GS gs, ST st) =>
        st.TransactionSetIdentifierCode_01 switch
        {
            "837" => st.ImplementationConventionPreference_03 switch
            {
                "005010X224A2" or "005010X224" =>
                    typeof(EdiFabric.Templates.Hipaa5010.TS837D).GetTypeInfo(),
                "005010X222A1" or "005010X222A2" =>
                    typeof(EdiFabric.Templates.Hipaa5010.TS837P).GetTypeInfo(),
                "005010X223A2" => typeof(EdiFabric.Templates.Hipaa5010.TS837I).GetTypeInfo(),
                var implementationCodePreference => throw new Exception(
                    $"Unsupported 837 implementation code preference. {implementationCodePreference}"
                ),
            },
            "275" => st.ImplementationConventionPreference_03 switch
            {
                "005010X215" => typeof(EdiFabric.Templates.X12004010.TS275).GetTypeInfo(),
                var implementationCodePreference => throw new Exception(
                    $"Unsupported 275 implementation code preference. {implementationCodePreference}"
                ),
            },
            var transactionCode => throw new Exception(
                $"Unsupported transaction. {transactionCode}"
            ),
        };
}
