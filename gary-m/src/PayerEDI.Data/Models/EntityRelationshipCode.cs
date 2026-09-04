using System.Runtime.Serialization;
using FastEnumUtility;

namespace PayerEDI.Data.Models;

/// <summary>
/// NM1 entity relationship codes.
/// </summary>
/// <remarks>
/// Represents relationship codes from https://www.stedi.com/edi/x12-005010/segment/NM1#NM1-10
/// </remarks>
public enum EntityRelationshipCode
{
    [EnumMember(Value = "01")]
    Parent,

    [EnumMember(Value = "02")]
    Child,

    [EnumMember(Value = "03")]
    Corporation,

    [EnumMember(Value = "04")]
    Subsidiary,

    [EnumMember(Value = "05")]
    WhollyOwnedSubsidiary,

    [EnumMember(Value = "06")]
    Division,

    [EnumMember(Value = "07")]
    Company,

    [EnumMember(Value = "08")]
    DoingBusinessAs,

    [EnumMember(Value = "09")]
    Component,

    [EnumMember(Value = "10")]
    Partnership,

    [EnumMember(Value = "11")]
    Partner,

    [EnumMember(Value = "12")]
    Member,

    [EnumMember(Value = "13")]
    Association,

    [EnumMember(Value = "14")]
    Headquarters,

    [EnumMember(Value = "15")]
    ProfitCenter,

    [EnumMember(Value = "16")]
    CostCenter,

    [EnumMember(Value = "17")]
    ProductLine,

    [EnumMember(Value = "18")]
    Union,

    [EnumMember(Value = "19")]
    Group,

    [EnumMember(Value = "20")]
    Department,

    [EnumMember(Value = "21")]
    MultinationalCorporation,

    [EnumMember(Value = "22")]
    Sibling,

    [EnumMember(Value = "23")]
    Affiliate,

    [EnumMember(Value = "24")]
    DirectAffiliate,

    [EnumMember(Value = "25")]
    EstablishedPatient,

    [EnumMember(Value = "26")]
    NotEstablishedPatient,

    [EnumMember(Value = "27")]
    DomesticPartner,

    [EnumMember(Value = "29")]
    PowerOfAttorneyDelegee,

    [EnumMember(Value = "30")]
    SignificantOther,

    [EnumMember(Value = "31")]
    UltimateParentCompany,

    [EnumMember(Value = "32")]
    Branch,

    [EnumMember(Value = "33")]
    Owned,

    [EnumMember(Value = "34")]
    Managed,

    [EnumMember(Value = "35")]
    Leased,

    [EnumMember(Value = "36")]
    GroupAffiliate,

    [EnumMember(Value = "37")]
    OwnerAffiliate,

    [EnumMember(Value = "38")]
    Owner,

    [EnumMember(Value = "39")]
    RelatedForUsCustomsPurposes,

    [EnumMember(Value = "40")]
    RelatedForUsBureauOfTheCensusPurposes,

    [EnumMember(Value = "41")]
    Spouse,

    [EnumMember(Value = "42")]
    AdoptiveParent,

    [EnumMember(Value = "43")]
    Bank,

    [EnumMember(Value = "44")]
    Brother,

    [EnumMember(Value = "45")]
    BusinessAssociate,

    [EnumMember(Value = "46")]
    Daughter,

    [EnumMember(Value = "47")]
    Dependent,

    [EnumMember(Value = "48")]
    Employee,

    [EnumMember(Value = "49")]
    Employer,

    [EnumMember(Value = "50")]
    Father,

    [EnumMember(Value = "51")]
    Fiancee,

    [EnumMember(Value = "52")]
    Foreman,

    [EnumMember(Value = "53")]
    FosterParent,

    [EnumMember(Value = "54")]
    Friend,

    [EnumMember(Value = "55")]
    GrandChild,

    [EnumMember(Value = "56")]
    GrandParent,

    [EnumMember(Value = "57")]
    Guardian,

    [EnumMember(Value = "58")]
    InforcePolicyholder,

    [EnumMember(Value = "59")]
    Institution,

    [EnumMember(Value = "60")]
    Minister,

    [EnumMember(Value = "61")]
    Mother,

    [EnumMember(Value = "62")]
    Neighbor,

    [EnumMember(Value = "63")]
    NonFamily,

    [EnumMember(Value = "64")]
    None,

    [EnumMember(Value = "65")]
    Other,

    [EnumMember(Value = "66")]
    OtherFamily,

    [EnumMember(Value = "67")]
    Self,

    [EnumMember(Value = "68")]
    Sister,

    [EnumMember(Value = "69")]
    StepChild,

    [EnumMember(Value = "70")]
    Supervisor,

    [EnumMember(Value = "71")]
    Teacher,

    [EnumMember(Value = "72")]
    Unknown,

    [EnumMember(Value = "73")]
    BusinessName,

    [EnumMember(Value = "74")]
    Counselor,

    [EnumMember(Value = "75")]
    SanctioningOrganization,

    [EnumMember(Value = "76")]
    SponsoringOrganization,

    [EnumMember(Value = "77")]
    SameJobAsApplicant,

    [EnumMember(Value = "78")]
    Stockholder,

    [EnumMember(Value = "79")]
    Attorney,

    [EnumMember(Value = "80")]
    Aunt,

    [EnumMember(Value = "81")]
    BrotherInLaw,

    [EnumMember(Value = "82")]
    Cousin,

    [EnumMember(Value = "83")]
    DaughterInLaw,

    [EnumMember(Value = "84")]
    Family,

    [EnumMember(Value = "85")]
    FatherInLaw,

    [EnumMember(Value = "86")]
    FinancialInterest,

    [EnumMember(Value = "87")]
    MarketingUnit,

    [EnumMember(Value = "88")]
    MotherInLaw,

    [EnumMember(Value = "89")]
    Nephew,

    [EnumMember(Value = "90")]
    Niece,

    [EnumMember(Value = "91")]
    Officer,

    [EnumMember(Value = "92")]
    PrincipalCustomer,

    [EnumMember(Value = "93")]
    PrincipalSupplier,

    [EnumMember(Value = "94")]
    SisterInLaw,

    [EnumMember(Value = "95")]
    Son,

    [EnumMember(Value = "96")]
    SonInLaw,

    [EnumMember(Value = "97")]
    Uncle,

    [EnumMember(Value = "98")]
    Descendant,

    [EnumMember(Value = "99")]
    Director,

    [EnumMember(Value = "AA")]
    PrincipalStockholder,

    [EnumMember(Value = "AB")]
    InsuredEntity,

    [EnumMember(Value = "AC")]
    AlliedProfessional,

    [EnumMember(Value = "AD")]
    AncillaryReferral,

    [EnumMember(Value = "AE")]
    Contact,

    [EnumMember(Value = "AF")]
    Contract,

    [EnumMember(Value = "AG")]
    HealthCareFacilityAffiliation,

    [EnumMember(Value = "AH")]
    IndependentPhysicianPracticeAssociationAffiliation,

    [EnumMember(Value = "AI")]
    ReferralLabProvider,

    [EnumMember(Value = "AJ")]
    ManagedCareOrganizationAffiliation,

    [EnumMember(Value = "AK")]
    MedicalDirector,

    [EnumMember(Value = "AL")]
    HealthCareNetworkAffiliation,

    [EnumMember(Value = "AM")]
    OfficeManager,

    [EnumMember(Value = "AN")]
    OnCallPhysician,

    [EnumMember(Value = "AO")]
    PhysicianHospitalOrganizationAffiliation,

    [EnumMember(Value = "AP")]
    ProviderInPractice,

    [EnumMember(Value = "AQ")]
    ReferredByProvider,

    [EnumMember(Value = "AR")]
    ReferredToProvider,

    [EnumMember(Value = "AS")]
    ReferralXRayProvider,

    [EnumMember(Value = "AT")]
    ParentInLaw,

    [EnumMember(Value = "AU")]
    StepParent,

    [EnumMember(Value = "AV")]
    FormerSpouse,

    [EnumMember(Value = "AW")]
    Ward,

    [EnumMember(Value = "CP")]
    CustodialParent,

    [EnumMember(Value = "OP")]
    ObligatedParent,

    [EnumMember(Value = "PI")]
    Principal,
}

public static class EntityRelationshipCodeExtensions
{
    public static string ToCode(this EntityRelationshipCode code) =>
        code.GetEnumMemberValue()
        ?? throw new InvalidOperationException($"No EnumMember value found for {code}.");

    extension(EntityRelationshipCode)
    {
        public static EntityRelationshipCode? FromCode(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                foreach (var member in FastEnum.GetMembers<EntityRelationshipCode>())
                {
                    if (
                        string.Equals(
                            member.EnumMemberAttribute?.Value,
                            code,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    {
                        return member.Value;
                    }
                }
            }

            return null;
        }
    }
}
