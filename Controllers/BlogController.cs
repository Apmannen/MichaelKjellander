using MichaelKjellander.Data;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : Controller
{
    [HttpGet]
    [Route("posts")]
    public async Task<IActionResult> Get()
    {
        using HttpClient client = new HttpClient();
        WpContext context = new WpContext(client);

        (ICollection<WpPost> posts, int numPages) = await context.GetPosts();
            
        return Ok(ApiUtil.CreateApiResponse(posts, 1, numPages));
    }
}
