using System.Collections.Generic;
using EdiFabric.Core.Annotations.Edi;
using EdiFabric.Core.Annotations.Validation;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;

namespace EDI275AttachmentParser.Templates
{
    /// <summary>
    /// Additional Information to Support a Health Care Claim or Encounter
    /// Transaction Set 275 - Version 5010
    /// </summary>
    [Serializable()]
    [Message("X12", "005010", "275")]
    public class TS275 : EdiMessage
    {
        [Required]
        [Pos(1)]
        public ST ST { get; set; }

        [Required]
        [Pos(2)]
        public BHT_AdditionalReportingCategories BHT { get; set; }

        [Required]
        [ListCount(10)]
        [Pos(3)]
        public List<Loop_2000_275> Loop2000 { get; set; }

        [Required]
        [Pos(4)]
        public SE SE { get; set; }
    }

    /// <summary>
    /// Information Source Level
    /// </summary>
    [Serializable()]
    [Group("2000")]
    public class Loop_2000_275
    {
        [Required]
        [Pos(1)]
        public NM1_InformationReceiverName NM1 { get; set; }

        [Pos(2)]
        public List<REF_InformationReceiverAdditionalIdentification> REF { get; set; }

        [ListCount(1000)]
        [Pos(3)]
        public List<Loop_2100_275> Loop2100 { get; set; }
    }

    /// <summary>
    /// Patient Level
    /// </summary>
    [Serializable()]
    [Group("2100")]
    public class Loop_2100_275
    {
        [Required]
        [Pos(1)]
        public NM1_PatientName NM1 { get; set; }

        [Pos(2)]
        public List<REF_PatientAdditionalIdentification> REF { get; set; }

        [Pos(3)]
        public DMG_PatientDemographicInformation DMG { get; set; }

        [ListCount(10)]
        [Pos(4)]
        public List<Loop_2110_275> Loop2110 { get; set; }
    }

    /// <summary>
    /// Attachment/Report Identification
    /// </summary>
    [Serializable()]
    [Group("2110")]
    public class Loop_2110_275
    {
        [Required]
        [Pos(1)]
        public PWK_AdditionalPatientInformation PWK { get; set; }

        [Pos(2)]
        public List<BIN_BinaryData> BIN { get; set; }

        [Pos(3)]
        public List<REF_AttachmentControlNumber> REF { get; set; }

        [Pos(4)]
        public PER_PropertyOrEntityContactInformation PER { get; set; }
    }

    /// <summary>
    /// Beginning Hierarchical Transaction - Additional Reporting Categories
    /// </summary>
    [Serializable()]
    [Segment("BHT", typeof(X12_ID_1005_9))]
    public class BHT_AdditionalReportingCategories : BHT
    {
    }

    /// <summary>
    /// Information Receiver Name
    /// </summary>
    [Serializable()]
    [Segment("NM1", typeof(X12_ID_98_48))]
    public class NM1_InformationReceiverName : NM1
    {
    }

    /// <summary>
    /// Patient Name
    /// </summary>
    [Serializable()]
    [Segment("NM1", typeof(X12_ID_98_45))]
    public class NM1_PatientName : NM1
    {
    }

    /// <summary>
    /// Information Receiver Additional Identification
    /// </summary>
    [Serializable()]
    [Segment("REF")]
    public class REF_InformationReceiverAdditionalIdentification : REF
    {
    }

    /// <summary>
    /// Patient Additional Identification
    /// </summary>
    [Serializable()]
    [Segment("REF")]
    public class REF_PatientAdditionalIdentification : REF
    {
    }

    /// <summary>
    /// Attachment Control Number
    /// </summary>
    [Serializable()]
    [Segment("REF")]
    public class REF_AttachmentControlNumber : REF
    {
    }

    /// <summary>
    /// Patient Demographic Information
    /// </summary>
    [Serializable()]
    [Segment("DMG")]
    public class DMG_PatientDemographicInformation : DMG
    {
    }

    /// <summary>
    /// Additional Patient Information (Paperwork/Attachment)
    /// </summary>
    [Serializable()]
    [Segment("PWK")]
    public class PWK_AdditionalPatientInformation : PWK
    {
    }

    /// <summary>
    /// Binary Data Segment
    /// </summary>
    [Serializable()]
    [Segment("BIN")]
    public class BIN_BinaryData : BIN
    {
    }

    /// <summary>
    /// Property or Entity Contact Information
    /// </summary>
    [Serializable()]
    [Segment("PER")]
    public class PER_PropertyOrEntityContactInformation : PER
    {
    }
}
