using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Models.WebGames;

public class Word
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    [StringLength(50)]
    [Required]
    public string? WordString { get; set; }
}