using EdiFabric.Core.Model.Edi.X12;
using EdiMettle.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdiMettle.Database.Models
{
    public class InterchangeHeader
    {
        #region database metadata
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int InstitutionalClaimId { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }
        #endregion

        #region document fields
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string AuthorizationInformationQualifier { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string AuthorizationInformation { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string SecurityInformationQualifier { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string SecurityInformation { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string SenderIdQualifier { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string InterchangeSenderId { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string ReceiverIdQualifier { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string InterchangeReceiverId { get; set; }

        public DateTime InterchangeDateTime { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string InterchangeControlStandardsIdentifier { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string InterchangeControlVersionNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public int InterchangeControlNumber { get; set; }

        public bool AcknowledgementRequested { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string UsageIndicator { get; set; }
        #endregion

        /// <summary>
        /// Translates the HeaderSegment into the poorly made EDI Fabric ISA object.
        /// </summary>
        /// <returns>ISA object fully populated.</returns>
        public ISA ToInsanity()
        {
            ISA insanity = new()
            {
                AuthorizationInformationQualifier_1 = AuthorizationInformationQualifier,
                AuthorizationInformation_2 = AuthorizationInformation,
                SecurityInformationQualifier_3 = SecurityInformationQualifier,
                SecurityInformation_4 = SecurityInformation,
                SenderIDQualifier_5 = SenderIdQualifier,
                InterchangeSenderID_6 = InterchangeSenderId,
                ReceiverIDQualifier_7 = ReceiverIdQualifier,
                InterchangeReceiverID_8 = InterchangeReceiverId,
                InterchangeDate_9 = InterchangeDateTime.ToEdiDate(),
                InterchangeTime_10 = InterchangeDateTime.ToEdiTime(),
                InterchangeControlStandardsIdentifier_11 = InterchangeControlStandardsIdentifier,
                InterchangeControlVersionNumber_12 = InterchangeControlVersionNumber,
                InterchangeControlNumber_13 = Helpers.GetPaddedControlNumber(InterchangeControlNumber),
                AcknowledgementRequested_14 = Helpers.BoolToBitString(AcknowledgementRequested),
                UsageIndicator_15 = UsageIndicator,
            };

            return insanity;
        }
    }
}
