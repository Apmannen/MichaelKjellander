using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Models.WebGames;

public class WordGuessGameProgress : Model
{
    [Key]
    [StringLength(50)]
    public string? Uuid { get; set; }
    
    [Required]
    public int? GuessesLeft { get; set; }
    
    public Word? Word { get; set; }
    
    [Required]
    //[ForeignKey("Word")]
    public int? WordId { get; set; }
    
    [StringLength(50)]
    [Required]
    public string? WordProgress { get; set; }

    public WordGuessGameProgress CreateDto()
    {
        return new WordGuessGameProgress()
        {
            Uuid = this.Uuid,
            GuessesLeft = this.GuessesLeft,
            WordProgress = this.WordProgress
        };
    }
}