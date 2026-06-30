namespace Edi837Ingestion.Parsing;

/// <summary>
/// Which HIPAA 5010 837 implementation a parsed transaction uses: professional
/// (837P, 005010X222A1), institutional (837I, 005010X223A2), or dental
/// (837D, 005010X224A2). Lets consumers branch on the claim type without
/// downcasting the EdiFabric message.
/// </summary>
public enum Edi837Variant
{
    Professional,
    Institutional,
    Dental,
}
