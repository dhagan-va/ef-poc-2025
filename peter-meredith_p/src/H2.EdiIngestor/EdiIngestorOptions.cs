namespace H2.EdiIngestor;

using System.ComponentModel.DataAnnotations;

public class EdiTestOptions
{
    [Required]
    [MinLength(1)]
    public string FileName { get; set; } = string.Empty;
}
