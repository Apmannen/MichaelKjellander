using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Domains.Models.Wordpress;

public enum Language
{
    Swedish = 0
}

[Table("wp_translation_entries")]
public class WpTranslationEntry : DbModel
{
    [Required]
    [MaxLength(VarcharLength)]
    public string? Key { get; set; }
    [Required] 
    public Language Language { get; set; }
    [Required] 
    [Column(TypeName = "text")]
    public string? Text { get; set; }
}