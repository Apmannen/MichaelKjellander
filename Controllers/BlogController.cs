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
    private readonly WpContext _wpContext;
    public BlogController(WpContext wpContext)
    {
        _wpContext = wpContext;
    }
    
    [HttpGet]
    [Route("posts")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = ["Page"])]
    public async Task<IActionResult> Get([FromQuery] PostsRequest postsRequest)
    {
        int page = postsRequest.Page ?? 1;

        (ICollection<WpPost> posts, int numPages) = await _wpContext.GetPosts(page: page);
            
        return Ok(ApiUtil.CreateApiResponse(posts, page, numPages));
    }

    public class PostsRequest
    {
        public int? Page { get; set; }
    }
}