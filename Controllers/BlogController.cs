using MichaelKjellander.Data;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils;
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

        ApiUtil.ApiResponse<WpPost> response = ApiUtil.CreateApiResponse(posts, 1, numPages);
        //Console.WriteLine("**** response"+response.Items);
            
        return Ok(response);
    }
}
