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
    [Route("categories")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetCategories()
    {
        IList<WpCategory> items = await _wpApiService.GetCategories();
        return Ok(ApiUtil.CreateApiResponse(items));
    }

    [HttpGet]
    [Route("meta-platforms")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetPlatforms()
    {
        IList<string> platforms = await _wpApiService.GetMetas();
        return Ok(ApiUtil.CreateApiResponse(platforms));
    }

    [HttpGet]
    [Route("pages")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
        VaryByQueryKeys = ["Slug"])]
    public async Task<IActionResult> GetPages([FromQuery] PagesRequest pageRequest)
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
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
        VaryByQueryKeys = ["categorySlug", "metaPlatforms", "metaRatings", "page", "slug"])]
    public async Task<IActionResult> GetPosts([FromQuery] PostsRequest postsRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int page = postsRequest.Page ?? 1;

        (IList<WpPost> posts, int numPages) = await _wpApiService.GetPosts(
            categorySlug: postsRequest.CategorySlug,
            metaPlatforms: postsRequest.MetaPlatforms ?? [],
            metaRatings: postsRequest.MetaRatings ?? [],
            page: postsRequest.Page ?? 1,
            postSlug: postsRequest.Slug
        );

        return Ok(ApiUtil.CreateApiResponse(posts, page, numPages));
    }


    public class PagesRequest
    {
        [Required] public string? Slug { get; set; }
    }

    public class PostsRequest
    {
        public string? CategorySlug { get; set; }
        public string[]? MetaPlatforms { get; set; }
        public int[]? MetaRatings { get; set; }
        public int? Page { get; set; }
        public string? Slug { get; set; }
    }
}