using MichaelKjellander.Data;
using MichaelKjellander.Models.WebGames;
using MichaelKjellander.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/webgames")]
public class WebGameController : Controller
{
    private const int OneHour = 3600;
    private readonly WpApiService _wpApiService;
    public WebGameController(WpApiService wpApiService)
    {
        _wpApiService = wpApiService;
    }

    
    [HttpGet]
    [Route("random-word")]
    public async Task<IActionResult> Get()
    {
        using WebGamesDataContext context = new WebGamesDataContext();
        Word word = context.Words.FromSqlRaw("SELECT * FROM words w WHERE LENGTH(w.WordString)>=5  ORDER BY RAND() DESC LIMIT 1")
            .FirstOrDefault();

        return Ok(word!.WordString);
    }
    
}