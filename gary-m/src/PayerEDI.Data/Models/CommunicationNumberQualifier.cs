using System.Reflection;
using System.Runtime.Serialization;

namespace PayerEDI.Data.Models;

/// <summary>
/// Communication number qualifier codes. See https://www.stedi.com/edi/x12-005010/element/365
/// </summary>
public enum CommunicationNumberQualifier
{
    [EnumMember(Value = "AA")]
    InternationalTelephoneAccessCode,

    [EnumMember(Value = "AB")]
    JointFacsimileAndPhoneNumber,

    [EnumMember(Value = "AC")]
    MessageOnlyVoiceNumber,

    [EnumMember(Value = "AD")]
    DeliveryLocationPhone,

    [EnumMember(Value = "AE")]
    AreaCode,

    [EnumMember(Value = "AP")]
    AlternateTelephone,

    [EnumMember(Value = "AS")]
    AnsweringService,

    [EnumMember(Value = "AU")]
    DefenseSwitchedNetwork,

    [EnumMember(Value = "BN")]
    BeeperNumber,

    [EnumMember(Value = "BT")]
    BtxNumber,

    [EnumMember(Value = "CA")]
    Cable,

    [EnumMember(Value = "CP")]
    CellularPhone,

    [EnumMember(Value = "DN")]
    DefenseDataNetwork,

    [EnumMember(Value = "EA")]
    InternetEmailAddress,

    [EnumMember(Value = "ED")]
    ElectronicDataInterchangeAccessNumber,

    [EnumMember(Value = "EM")]
    ElectronicMail,

    [EnumMember(Value = "EX")]
    TelephoneExtension,

    [EnumMember(Value = "FT")]
    FederalTelecommunicationsSystem,

    [EnumMember(Value = "FU")]
    FacsimileUserIdentifier,

    [EnumMember(Value = "FX")]
    Facsimile,

    [EnumMember(Value = "HF")]
    HomeFacsimileNumber,

    [EnumMember(Value = "HP")]
    HomePhoneNumber,

    [EnumMember(Value = "IT")]
    InternationalTelephone,

    [EnumMember(Value = "MN")]
    ModemNumber,

    [EnumMember(Value = "NP")]
    NightTelephone,

    [EnumMember(Value = "OF")]
    OtherResidentialFacsimileNumber,

    [EnumMember(Value = "OT")]
    OtherResidentialTelephoneNumber,

    [EnumMember(Value = "PA")]
    AppointmentPhone,

    [EnumMember(Value = "PC")]
    PersonalCellular,

    [EnumMember(Value = "PP")]
    PersonalPhone,

    [EnumMember(Value = "PS")]
    PacketSwitching,

    [EnumMember(Value = "SP")]
    ShowingPhone,

    [EnumMember(Value = "TE")]
    Telephone,

    [EnumMember(Value = "TL")]
    Telex,

    [EnumMember(Value = "TM")]
    Telemail,

    [EnumMember(Value = "TN")]
    TeletexNumber,

    [EnumMember(Value = "TX")]
    Twx,

    [EnumMember(Value = "UR")]
    UniformResourceLocator,

    [EnumMember(Value = "VM")]
    VoiceMail,

    [EnumMember(Value = "WC")]
    WorkCellular,

    [EnumMember(Value = "WF")]
    WorkFacsimileNumber,

    [EnumMember(Value = "WP")]
    WorkPhoneNumber,
}
