namespace EdiParser.Entities
{
    public class ClaimType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
    
    
    public enum ClaimTypeEnum
    {
        Unknown = 0,
        Professional = 1,
        Institutional = 2,
        Dental = 3,
    }
    
}