using EdiFabric.Templates.Hipaa5010;
using EdiMettle.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdiMettle.Database.Models
{
    /**
     * BHT
     */
    public class BeginningHierarchicalTransaction()
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
        public int StructureCode { get; set; }
        public int SetPurpose { get; set; }
        public int SubmitterIdentifier { get; set; }
        public DateTime CreationTimestamp { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(1000)]
        public required string TypeCode { get; set; }
        #endregion

        [ForeignKey(nameof(InstitutionalClaimId))]
        public InstitutionalClaim? GetInstitutionalClaim { get; set; }

        public BHT_BeginningOfHierarchicalTransaction_8 ToInsanity()
        {
            BHT_BeginningOfHierarchicalTransaction_8 bht = new()
            {
                HierarchicalStructureCode_01 = StructureCode.ToString().PadLeft(4, '0'),
                TransactionSetPurposeCode_02 = SetPurpose.ToString().PadLeft(2, '0'),
                SubmitterTransactionIdentifier_03 = SubmitterIdentifier.ToString().PadLeft(4, '0'),
                TransactionSetCreationDate_04 = CreationTimestamp.ToEdiDate(),
                TransactionSetCreationTime_05 = CreationTimestamp.ToEdiTime(),
                TransactionTypeCode_06 = TypeCode
            };

            return bht;
        }
    }
}
