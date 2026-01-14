using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDI837.src.Models
{
    public class ProcessedClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClaimIdentifier  { get; set; } = string.Empty;
        public string ClaimControlNumber { get; set;} = string.Empty;
        public string ClaimConventionReference { get; set;} = string.Empty;
        public string Claim { get; set; } = string.Empty;   
    }
}
