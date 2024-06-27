using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Domains.Models.WebGames;

[Table("word_guess_game_progresses")]
public class WordGuessGameProgress : DbModel
{
    [StringLength(50)]
    public string? Uuid { get; set; }
    
    [Required]
    public int? GuessesLeft { get; set; }
    
    public virtual Word? Word { get; set; }
    
    [Required]
    //[ForeignKey("Word")]
    public int? WordId { get; set; }
    
    [StringLength(50)]
    [Required]
    public string? WordProgress { get; set; }

    public bool IsCorrectlyGuessed => WordProgress != null && LettersLeft == 0;

    public int LettersLeft
    {
        get
        {
            if (WordProgress == null)
            {
                return 0;
            }
            int count = 0;
            foreach (char letter in WordProgress)
            {
                if (letter == '_')
                {
                    count++;
                }
            }
            return count;
        }
    }
    
    public WordGuessGameProgress CreateDto(bool includeCorrectWord)
    {
        WordGuessGameProgress wordProgress = new WordGuessGameProgress()
        {
            Uuid = this.Uuid,
            GuessesLeft = this.GuessesLeft,
            WordProgress = this.WordProgress,
            Word = includeCorrectWord ? new Word{WordString = this.Word!.WordString} : null
        };

        return wordProgress;
    }
}