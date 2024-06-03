using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MichaelKjellander.Data;
using MichaelKjellander.Models.WebGames;
using MichaelKjellander.SharedUtils;
using MichaelKjellander.SharedUtils.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/webgames")]
public class WebGameController : Controller
{
    private readonly ILogger _logger;

    public WebGameController(ILogger<WebGameController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("word-guess/init")]
    public async Task<IActionResult> InitGame()
    {
        _logger.LogInformation("**** Initing game!");
        
        Guid uuid = Guid.NewGuid();
        string uuidString = uuid.ToString();
        
        await using WebGamesDataContext context = new WebGamesDataContext();
        Word randomWord = context.Words.FromSqlRaw("SELECT * FROM words w WHERE LENGTH(w.WordString)>=5  ORDER BY RAND() DESC LIMIT 1")
            .FirstOrDefault()!;

        string maskedWord = "";
        for (int i = 0; i < randomWord.WordString!.Length; i++)
        {
            maskedWord += randomWord.WordString[i] == '-' ? '-' : '_';
        }
        
        WordGuessGameProgress progress = new WordGuessGameProgress
        {
            Uuid = uuidString,
            GuessesLeft = 5,
            WordId = randomWord.Id,
            WordProgress = maskedWord
        };
        context.Add(progress);
        await context.SaveChangesAsync();

        WordGuessGameProgress progressReturn = new WordGuessGameProgress
        {
            Uuid = progress.Uuid,
            GuessesLeft = progress.GuessesLeft,
            WordProgress = progress.WordProgress
        };
        
        return Ok(ApiUtil.CreateApiResponse<WordGuessGameProgress>([progressReturn]));
    }

    [HttpGet]
    [Route("word-guess/guess")]
    public async Task<IActionResult> Guess([FromQuery] GuessRequest guessRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Regex regex = new Regex("[abcdefghijklmnopqrstuvxyzåäö]");
        bool isValidLetter = regex.IsMatch(guessRequest.letter!);
        if (!isValidLetter)
        {
            return BadRequest("Invalid letter");
        }
        
        return Ok();
    }
    
    public class GuessRequest
    {
        [Required]
        [StringLength(1)]
        public string? letter { get; set; }
    }
}