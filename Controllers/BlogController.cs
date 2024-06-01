using System.Text.Json;
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
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = ["Page"])]
    public async Task<IActionResult> Get([FromQuery] PostsRequest postsRequest)
    {
        int page = postsRequest.Page ?? 1;
        
        using HttpClient client = new HttpClient();
        WpContext context = new WpContext(client);

        (ICollection<WpPost> posts, int numPages) = await context.GetPosts(page: page);
            
        return Ok(ApiUtil.CreateApiResponse(posts, page, numPages));
    }

    public class PostsRequest
    {
        public int? Page { get; set; }
    }
}