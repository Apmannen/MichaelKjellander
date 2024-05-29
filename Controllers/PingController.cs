using MichaelKjellander.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api")]
public class PingController : Controller
{
    [HttpGet]
    public string Get()
    {
        return "pong";
    }
}
