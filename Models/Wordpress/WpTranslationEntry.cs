using System.ComponentModel.DataAnnotations;

namespace MichaelKjellander.Models.Wordpress;

public enum Language
{
    Swedish = 0
}

public class WpTranslationEntry : DbModel
{
    [Required]
    [MaxLength(VarcharLength)]
    public string? Key { get; set; }
    [Required] 
    public Language Language { get; set; }
    [Required] 
    public string? Translation { get; set; }
}