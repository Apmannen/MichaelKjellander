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
    [Route("random-word")]
    [System.Obsolete("Not used")]
    public async Task<IActionResult> GetRandomWord()
    {
        await using WebGamesDataContext context = new WebGamesDataContext();
        Word word = context.Words.FromSqlRaw("SELECT * FROM words w WHERE LENGTH(w.WordString)>=5  ORDER BY RAND() DESC LIMIT 1")
            .FirstOrDefault();

        return Ok(ApiUtil.CreateApiResponse<Word>([word]));
    }
    
    [HttpGet]
    [Route("random-words")]
    [System.Obsolete("Not used")]
    public async Task<IActionResult> GetWords()
    {
        await using WebGamesDataContext context = new WebGamesDataContext();
        List<Word> words = await context.Words.Where(row => row.WordString.Length >= 5).ToListAsync();
        CollectionUtil.Shuffle(words);

        List<Word> filteredWords = [];
        for (int i = 0; i < 1000; i++)
        {
            filteredWords.Add(words[i]);
        }
        filteredWords.Sort((word1, word2) => ((int)word1.Id!).CompareTo((int)word2.Id!));

        return Ok(ApiUtil.CreateApiResponse(filteredWords));
    }

    [HttpGet]
    [Route("init-guess-words-game")]
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
            WordProgress = maskedWord
        };
        context.Add(progress);
        

        return Ok(progress);
    }
    
}