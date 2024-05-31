using Microsoft.AspNetCore.Mvc;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : Controller
{
    [HttpGet]
    public string Get()
    {
        return "pong";
    }
}
