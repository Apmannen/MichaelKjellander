using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Data;
using MichaelKjellander.Services;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : Controller
{
    private const int OneHour = 3600;
    [Obsolete("Use internal DB instead")] private readonly WpApiService _wpApiService;

    public BlogController(WpApiService wpApiService)
    {
        _wpApiService = wpApiService;
    }

    [HttpGet]
    [Route("categories")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetCategories()
    {
        await using var context = new BlogDataContext();
        IList<WpCategory> items = context.Categories.ToList();
        return Ok(ApiUtil.CreateApiResponse(items));
    }

    [HttpGet]
    [Route("meta-platforms")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    [Obsolete("Replace with tags")] //TODO: note! obsolete!
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

        await using var context = new BlogDataContext();
        WpPage? page = context.Pages.FirstOrDefault(p => p.Slug == pageRequest.Slug);
        if (page == null)
        {
            return NotFound();
        }

        return Ok(ApiUtil.CreateApiResponse([page], 1, 1));
    }

    [HttpGet]
    [Route("posts")]
    //[ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
     //   VaryByQueryKeys = ["categorySlug", "metaPlatforms", "metaRatings", "page", "slug"])]
    public async Task<IActionResult> GetPosts([FromQuery] PostsRequest postsRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int pageNumber = postsRequest.Page ?? 1;
        
        await using var context = new BlogDataContext();
        IQueryable<WpPost> query = context.Posts;
        query = query.Include(row => row.Category);
        if (postsRequest.Slug != null)
        {
            query = query.Where(row => row.Slug == postsRequest.Slug);
        }
        if (postsRequest.MetaRatings is { Count: > 0 })
        {
            query = query.Where(row => row.MetaRating != null && postsRequest.MetaRatings.Contains((int)row.MetaRating));
        }

        IList<WpPost> posts = query.ToList();
        
        return Ok(ApiUtil.CreateApiResponse(posts, 1, 1));
        /*(IList<WpPost> posts, int numPages) = await _wpApiService.GetPosts(
            categorySlug: postsRequest.CategorySlug,
            metaPlatforms: postsRequest.MetaPlatforms ?? [],
            metaRatings: postsRequest.MetaRatings ?? [],
            page: postsRequest.Page ?? 1,
            postSlug: postsRequest.Slug
        );

        return Ok(ApiUtil.CreateApiResponse(posts, page, numPages));*/
    }


    public class PagesRequest
    {
        [Required] public string? Slug { get; set; }
    }

    public class PostsRequest
    {
        public string? CategorySlug { get; set; }
        public string[]? MetaPlatforms { get; set; }
        public List<int>? MetaRatings { get; set; }
        public int? Page { get; set; }
        public string? Slug { get; set; }
    }
}