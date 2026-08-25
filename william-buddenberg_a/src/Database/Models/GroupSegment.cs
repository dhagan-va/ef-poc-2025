using EdiFabric.Core.Model.Edi.X12;
using EdiMettle.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdiMettle.Database.Models
{
    public class GroupSegment()
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
        public required string CodeIdentifyingInformationType { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string SenderId { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string ReceiverId { get; set; }
        public DateTime TimeStamp { get; set; }

        public int GroupControlNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string TransactionTypeCode { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string VersionAndRelease { get; set; }
        #endregion

        [ForeignKey(nameof(InstitutionalClaimId))]
        public InstitutionalClaim? GetInstitutionalClaim { get; set; }

        /// <summary>
        /// And beyond.
        /// </summary>
        /// <returns>EDI Fabric GS object</returns>
        public GS ToInsanity()
        {
            GS newGroup = new()
            {
                CodeIdentifyingInformationType_1 = CodeIdentifyingInformationType,
                SenderIDCode_2 = SenderId,
                ReceiverIDCode_3 = ReceiverId,
                Date_4 = TimeStamp.Date.ToString(Constants.InterchangeDateFormat),
                Time_5 = TimeStamp.TimeOfDay.ToString(Constants.InterchangeTimeFormat),

                //  Must be unique to both partners for this interchange
                GroupControlNumber_6 = Helpers.GetPaddedControlNumber(GroupControlNumber),

                //  Responsible Agency Code
                TransactionTypeCode_7 = TransactionTypeCode,
                VersionAndRelease_8 = VersionAndRelease
            };

            return newGroup;
        }
    }
}
