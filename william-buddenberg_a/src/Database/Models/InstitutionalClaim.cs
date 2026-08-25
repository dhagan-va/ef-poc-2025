using EdiFabric.Templates.Hipaa5010;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdiMettle.Database.Models
{
    public class InstitutionalClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }

        [NotMapped]
        public int ControlNumber { get; set; } // Not sure if this needs to be stored or what.

        [NotMapped]
        public GroupSegment? GetGroupSegment { get; set; }

        public TransactionSetHeader? GetTransactionSetHeader { get; set; }
        public BeginningHierarchicalTransaction? GetBeginningHierarchicalTransaction { get; set; }

        // REF: I_REF<C040> ????

        // List<Loop_NM1_837> NM1Loop
        // List<Loop_HL_837> HLLoop
        // SE Set CRC or something.

        public TS837I ToInsanity()
        {
            // TODO:  Nullness of child objects
            TS837I insanity = new()
            {
                ST = GetTransactionSetHeader.ToInsanity(),
                BHT_BeginningOfHierarchicalTransaction = GetBeginningHierarchicalTransaction.ToInsanity(),
                AllNM1 = new All_NM1_837I_6(),
                Loop2000A = [],
                SE = new EdiFabric.Core.Model.Edi.X12.SE()
            };

            return insanity;
        }
    }
}
