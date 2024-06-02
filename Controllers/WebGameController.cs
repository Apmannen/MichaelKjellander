using MichaelKjellander.Data;
using MichaelKjellander.Models.WebGames;
using MichaelKjellander.Services;
using MichaelKjellander.SharedUtils.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/webgames")]
public class WebGameController : Controller
{
    private const int OneHour = 3600;
    public WebGameController()
    {
    }

    
    [HttpGet]
    [Route("random-word")]
    public async Task<IActionResult> GetRandomWord()
    {
        using WebGamesDataContext context = new WebGamesDataContext();
        Word word = context.Words.FromSqlRaw("SELECT * FROM words w WHERE LENGTH(w.WordString)>=5  ORDER BY RAND() DESC LIMIT 1")
            .FirstOrDefault();

        return Ok(ApiUtil.CreateApiResponse<Word>([word]));
    }
    
    [HttpGet]
    [Route("words")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = ["category_slug", "page"])]
    public async Task<IActionResult> GetWords()
    {
        using WebGamesDataContext context = new WebGamesDataContext();
        List<Word> words = await context.Words.Where(row => row.WordString.Length >= 5).ToListAsync();

        return Ok(ApiUtil.CreateApiResponse<Word>(words));
    }
    
}