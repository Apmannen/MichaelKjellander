using System.ComponentModel.DataAnnotations;

namespace MichaelKjellander.Models.WebGames;

public class Word
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    [Required]
    public string? WordString { get; set; }
}