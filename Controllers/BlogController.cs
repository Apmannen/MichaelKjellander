using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Services;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : Controller
{
    private const int OneHour = 3600;
    private readonly WpApiService _wpApiService;
    public BlogController(WpApiService wpApiService)
    {
        _wpApiService = wpApiService;
    }
    
    [HttpGet]
    [Route("pages")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = ["Slug"])]
    public async Task<IActionResult> Get([FromQuery] PagesRequest pageRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        WpPage? page = await _wpApiService.GetPage(slug: pageRequest.Slug!);
        if (page == null)
        {
            return NotFound();
        }
        
        return Ok(ApiUtil.CreateApiResponse([page], 1, 1));
    }
    
    [HttpGet]
    [Route("posts")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = ["categorySlug", "page", "slug"])]
    public async Task<IActionResult> Get([FromQuery] PostsRequest postsRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int page = postsRequest.Page ?? 1;
        string? categorySlug = GetStringValue(postsRequest.CategorySlug);
        string? postSlug = GetStringValue(postsRequest.Slug);

        (IList<WpPost> posts, int numPages) = await _wpApiService.GetPosts(page: page, categorySlug: categorySlug, postSlug);
            
        return Ok(ApiUtil.CreateApiResponse(posts, page, numPages));
    }

    private static string? GetStringValue(string? s)
    {
        return string.IsNullOrEmpty(s) ? null : s;
    }
    
    public class PagesRequest
    {
        [Required]
        public string? Slug { get; set; }
    }
    public class PostsRequest
    {
        public string? CategorySlug { get; set; }
        public int? Page { get; set; }
        public string? Slug { get; set; }
    }
    
}