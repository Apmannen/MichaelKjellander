using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Domains.Models.WebGames;

[Table("words")]
public class Word : DbModel
{
    [StringLength(50)]
    [Required]
    public string? WordString { get; set; }

    public virtual ICollection<WordGuessGameProgress>? GuessGameProgresses { get; set; }
    
    public static readonly string ValidLetters = "abcdefghijklmnopqrstuvxyzåäö";
    
}