using EdiFabric.Core.Model.Edi.X12;
using EdiMettle.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdiMettle.Database.Models
{
    /**
     * ST section.
     */
    public class TransactionSetHeader
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
        public required string IdentifierCode { get; set; }
        public int ControlNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string ImplementationConventionPreference { get; set; }
        #endregion

        [ForeignKey(nameof(InstitutionalClaimId))]
        public InstitutionalClaim? GetInstitutionalClaim { get; set; }

        public ST ToInsanity()
        {
            ST st = new()
            {
                TransactionSetIdentifierCode_01 = IdentifierCode,
                TransactionSetControlNumber_02 = Helpers.GetPaddedControlNumber(ControlNumber),
                ImplementationConventionPreference_03 = ImplementationConventionPreference
            };

            return st;
        }
    }
}
