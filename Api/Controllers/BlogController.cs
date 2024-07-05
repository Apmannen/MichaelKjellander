using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Api.Features.Posts;
using MichaelKjellander.Data;
using MichaelKjellander.Domains.ApiResponse;
using MichaelKjellander.Domains.Models.Wordpress;
using Microsoft.AspNetCore.Mvc;

namespace MichaelKjellander.Api.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : Controller
{
    private const int OneHour = 3600;

    public BlogController()
    {
    }

    [HttpGet]
    [Route("categories")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetCategories()
    {
        await using var context = new BlogDataContext();
        List<WpCategory> items = context.Categories.ToList();
        return Ok(ApiResponseFactory.CreateSimpleApiResponse(items));
    }

    [HttpGet]
    [Route("tags")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
        VaryByQueryKeys = ["CategorySlug"])]
    public async Task<IActionResult> GetTags([FromQuery] TagsRequest tagsRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await using var context = new BlogDataContext();
        var result = context.Tags
            .Where(t => t.PostTags!
                .Any(pt => pt.Post!.Category!.Slug == tagsRequest.CategorySlug))
            .Select(t => new
            {
                Tag = t,
                Posts = t.PostTags!.Select(pt => pt.Post)
            })
            .ToList();
        List<WpTag> tags = [];
        tags.AddRange(result.Where(row => row.Tag.ShortName != "").Select(row => row.Tag));
        tags.Sort((a, b) => string.Compare(a.ShortName, b.ShortName, StringComparison.Ordinal));

        return Ok(ApiResponseFactory.CreateSimpleApiResponse(tags));

        //SELECT * FROM wp_tags t LEFT JOIN wp_post_tags pt ON pt.TagId=t.id LEFT JOIN wp_posts p ON p.id=pt.PostId LEFT JOIN wp_categories c ON c.Id = p.CategoryId WHERE c.Slug="tv-spelrecensioner";
    }

    [HttpGet]
    [Route("pages")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
        VaryByQueryKeys = ["PageIdentifier", "Slug"])]
    public async Task<IActionResult> GetPages([FromQuery] PagesRequest pageRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await using var context = new BlogDataContext();
        IQueryable<WpPage> query = context.Pages;
        if (pageRequest.Slug != null)
        {
            query = query.Where(p => p.Slug == pageRequest.Slug);
        }

        if (pageRequest.PageIdentifier != null)
        {
            query = query.Where(p => p.MetaIdName == pageRequest.PageIdentifier);
        }

        List<WpPage> pages = query.ToList();

        return Ok(ApiResponseFactory.CreateSimpleApiResponse(pages));
    }

    [HttpGet]
    [Route("posts")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
        VaryByQueryKeys = ["categorySlug", "tagIds", "metaRatings", "page", "slug"])]
    public async Task<IActionResult> GetPosts([FromQuery] PostsRequest postsRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ApiResponse<WpPost> postsResponse = await new PostsGet(postsRequest).Get();
        return Ok(postsResponse);
    }


    [HttpGet]
    [Route("translations")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetPages()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await using var context = new BlogDataContext();
        IQueryable<WpTranslationEntry> query = context.TranslationEntries;
        List<WpTranslationEntry> translationEntries = query.ToList();
        return Ok(ApiResponseFactory.CreateSimpleApiResponse(translationEntries));
    }


    public class PagesRequest
    {
        public string? PageIdentifier { get; set; }
        public string? Slug { get; set; }
    }


    public class TagsRequest
    {
        [Required] public string? CategorySlug { get; set; }
    }
}