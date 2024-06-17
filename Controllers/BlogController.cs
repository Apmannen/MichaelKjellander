using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Communicators;
using MichaelKjellander.Data;
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

    [Obsolete("Use internal DB instead")] private readonly WpApiCommunicator _wpApiCommunicator;

    public BlogController(WpApiCommunicator wpApiCommunicator)
    {
        _wpApiCommunicator = wpApiCommunicator;
    }

    [HttpGet]
    [Route("categories")]
    [ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetCategories()
    {
        await using var context = new BlogDataContext();
        List<WpCategory> items = context.Categories.ToList();
        return Ok(ApiUtil.CreateSimpleApiResponse(items));
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
            .Where(t => t.PostTags
                .Any(pt => pt.Post.Category!.Slug == tagsRequest.CategorySlug))
            .Select(t => new
            {
                Tag = t,
                Posts = t.PostTags.Select(pt => pt.Post)
            })
            .ToList();
        List<WpTag> tags = [];
        foreach (var row in result)
        {
            //TODO: use those post counters
            if (row.Tag.Slug != "fran-samlingen")
            {
                tags.Add(row.Tag);
            }
        }
        
        tags.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
 
        return Ok(ApiUtil.CreateSimpleApiResponse(tags));

        //SELECT * FROM wp_tags t LEFT JOIN wp_post_tags pt ON pt.TagId=t.id LEFT JOIN wp_posts p ON p.id=pt.PostId LEFT JOIN wp_categories c ON c.Id = p.CategoryId WHERE c.Slug="tv-spelrecensioner";
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

        return Ok(ApiUtil.CreateSimpleApiResponse([page]));
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

        int pageNumber = postsRequest.Page ?? 1;

        await using var context = new BlogDataContext();
        IQueryable<WpPost> query = context.Posts;

        if (postsRequest.Slug != null)
        {
            query = query.Where(row => row.Slug == postsRequest.Slug);
        }

        if (postsRequest.MetaRatings is { Count: > 0 })
        {
            query = query.Where(row =>
                row.MetaRating != null && postsRequest.MetaRatings.Contains((int)row.MetaRating));
        }

        if (postsRequest.CategorySlug != null)
        {
            query = query.Where(row => row.Category!.Slug == postsRequest.CategorySlug);
        }

        if (postsRequest.TagIds is { Count: > 0 })
        {
            query = query.Where(
                p => p.PostTags.Any(pt => postsRequest.TagIds.Contains(pt.TagId))
            );
        }

        int totalCount = await query.CountAsync();

        query = query.OrderByDescending(row => row.Date)
            .ThenByDescending(row => row.Id)
            .Include(row => row.Category)
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.FeaturedImage);

        int perPage = 10;
        query = DataContext.SetPageToQuery(query, pageNumber, perPage);

        Console.WriteLine("************** totalCount=" + totalCount);


        IList<WpPost> posts = await query.ToListAsync();

        foreach (WpPost post in posts)
        {
            //post.PostTags = null;
            foreach (WpPostTag postTag in post.PostTags)
            {
                postTag.Post = null;
                postTag.Tag.PostTags = null;
            }
        }

        return Ok(ApiUtil.CreateApiResponse(posts, currentPage: pageNumber, perPage: perPage, totalCount: totalCount));
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
        public List<int>? TagIds { get; set; }
    }

    public class TagsRequest
    {
        [Required] public string? CategorySlug { get; set; }
    }
}