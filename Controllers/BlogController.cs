using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Data;
using MichaelKjellander.Models;
using MichaelKjellander.Models.Wordpress;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Controllers;

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
        return Ok(ModelFactory.CreateSimpleApiResponse(items));
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

        return Ok(ModelFactory.CreateSimpleApiResponse(tags));

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

        return Ok(ModelFactory.CreateSimpleApiResponse(pages));
    }

    [HttpGet]
    [Route("posts")]
    /*[ResponseCache(Duration = OneHour, Location = ResponseCacheLocation.Any, NoStore = false,
        VaryByQueryKeys = ["categorySlug", "tagIds", "metaRatings", "page", "slug"])]*/
    public async Task<IActionResult> GetPosts([FromQuery] PostsRequest postsRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int pageNumber = postsRequest.Page ?? 1;

        await using var context = new BlogDataContext();
        IQueryable<WpPost> query = context.Posts;
        //IQueryable<WpPost> tagCountQuery;

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

        var tagCountQuery = query
            .SelectMany(p => p.PostTags)
            .GroupBy(pt => pt.Tag)
            .Select(g => new
            {
                Tag = g.Key,
                Count = g.Count()
            });
        var tagCounts = await tagCountQuery.ToListAsync();
        Dictionary<string, int> fieldCounts = new Dictionary<string, int>();
        //AddToDictionary(tagCounts, fieldCounts);

        foreach (var tagCount in tagCounts)
        {
            fieldCounts.Add(tagCount.Tag.Name!, tagCount.Count);
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

        const int perPage = 10;
        //query = DataContext.SetPageToQuery(query, pageNumber, perPage);
        query = query.Take(perPage);

        IList<WpPost> posts = await query.ToListAsync();

        foreach (WpPost post in posts)
        {
            foreach (WpPostTag postTag in post.PostTags)
            {
                postTag.Post = null;
                postTag.Tag.PostTags = null;
            }
        }

        return Ok(ModelFactory.CreateApiResponse(posts, currentPage: pageNumber, perPage: perPage,
            totalCount: totalCount, fieldCounts: fieldCounts));
    }

    private static void AddToDictionary<T>(List<T> list, Dictionary<string, int> dictionary)
    {
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
        return Ok(ModelFactory.CreateSimpleApiResponse(translationEntries));
    }


    public class PagesRequest
    {
        public string? PageIdentifier { get; set; }
        public string? Slug { get; set; }
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