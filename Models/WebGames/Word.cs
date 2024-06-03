using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Models.WebGames;

[Table("words")]
public class Word : Model
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    [StringLength(50)]
    [Required]
    public string? WordString { get; set; }

    public virtual ICollection<WordGuessGameProgress>? GuessGameProgresses { get; set; }
    
    public static readonly string ValidLetters = "abcdefghijklmnopqrstuvxyzåäö";
    
}