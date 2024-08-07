﻿using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Data;
using MichaelKjellander.Domains.ApiResponse;
using MichaelKjellander.Domains.Models.WebGames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Api.Controllers;

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
        Word randomWord = context.Words.FromSqlRaw("SELECT * FROM words w WHERE LENGTH(w.WordString)>=5 AND LENGTH(w.WordString)<=50  ORDER BY RAND() DESC LIMIT 1")
            .First();

        string maskedWord = "";
        for (int i = 0; i < randomWord.WordString!.Length; i++)
        {
            maskedWord += randomWord.WordString[i] == '-' ? '-' : '_';
        }
        
        WordGuessGameProgress progress = new WordGuessGameProgress
        {
            Uuid = uuidString,
            GuessesLeft = 8,
            WordId = randomWord.Id,
            WordProgress = maskedWord
        };
        context.Add(progress);
        await context.SaveChangesAsync();
        
        return Ok(ApiResponseFactory.CreateSimpleApiResponse<WordGuessGameProgress>([progress.CreateDto(false)]));
    }

    [HttpGet]
    [Route("word-guess/guess")]
    public async Task<IActionResult> Guess([FromQuery] GuessRequest guessRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        char letter = guessRequest.Letter![0];

        bool isValidLetter = Word.ValidLetters.Contains(letter);
        if (!isValidLetter)
        {
            return BadRequest("Invalid letter");
        }
        await using WebGamesDataContext context = new WebGamesDataContext();
        WordGuessGameProgress? gameProgress = context.WordGuessGameProgresses
            .Include(row => row.Word)
            .FirstOrDefault(row => row.Uuid == guessRequest.GameId);
        if (gameProgress == null)
        {
            return BadRequest("Invalid game ID");
        }

        if (gameProgress.GuessesLeft <= 0)
        {
            return BadRequest("No guesses left");
        }

        string fullWord = gameProgress.Word!.WordString!;
        string wordProgress = gameProgress.WordProgress!;
        if (fullWord.Contains(letter))
        {
            string newWord = "";
            for (int i = 0; i < wordProgress.Length; i++)
            {
                if (fullWord[i] == letter)
                {
                    newWord += letter;
                }
                else
                {
                    newWord += wordProgress[i];
                }
            }
            gameProgress.WordProgress = newWord;
        }
        else
        {
            gameProgress.GuessesLeft--;
        }
        context.Update(gameProgress);
        await context.SaveChangesAsync();

        bool includeCorrectWord = gameProgress.GuessesLeft == 0 || gameProgress.IsCorrectlyGuessed;
        return Ok(ApiResponseFactory.CreateSimpleApiResponse<WordGuessGameProgress>([gameProgress.CreateDto(includeCorrectWord)]));
    }
    
    public class GuessRequest
    {
        [Required]
        [StringLength(1)]
        public string? Letter { get; set; }
        
        [Required]
        public string? GameId { get; set; }
    }
}