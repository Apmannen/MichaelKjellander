using MichaelKjellander.Data;
using MichaelKjellander.Models;
using MichaelKjellander.Models.Wordpress;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Api.ModelFeatures.Posts;

public class PostsGet
{
    private readonly PostsRequest _postsRequest;

    public PostsGet(PostsRequest postsRequest)
    {
        _postsRequest = postsRequest;
    }

    public async Task<ApiResponse<WpPost>> Get()
    {
        int pageNumber = _postsRequest.Page ?? 1;

        await using var context = new BlogDataContext();
        IQueryable<WpPost> query = context.Posts;
        IQueryable<WpPost> tagCountQuery = context.Posts;
        IQueryable<WpPost> ratingCountQuery = context.Posts;

        if (_postsRequest.Slug != null)
        {
            query = SlugFilter(query);
            tagCountQuery = SlugFilter(tagCountQuery);
            ratingCountQuery = SlugFilter(ratingCountQuery);
        }

        if (_postsRequest.CategorySlug != null)
        {
            query = CategorySlugFilter(query);
            tagCountQuery = CategorySlugFilter(tagCountQuery);
            ratingCountQuery = CategorySlugFilter(ratingCountQuery);
        }

        if (_postsRequest.MetaRatings is { Count: > 0 })
        {
            query = MetaRatingsFilter(query);
            tagCountQuery = MetaRatingsFilter(tagCountQuery);
            //Not ratingCountQuery
        }

        if (_postsRequest.TagIds is { Count: > 0 })
        {
            query = TagFilter(query);
            //Not tagCountQuery
            ratingCountQuery = TagFilter(query);
        }

        var tagCountResult = tagCountQuery
            .SelectMany(p => p.PostTags)
            .GroupBy(pt => pt.Tag)
            .Select(g => new KeyValuePair<string, int>(g.Key.Name!, g.Count())).ToList();

        var ratingCountResult = ratingCountQuery.GroupBy(p => p.MetaRating).Select(g =>
            new KeyValuePair<string, int>(g.Key.ToString()!, g.Count())).ToList();


        int totalCount = await query.CountAsync();

        query = query.OrderByDescending(row => row.Date)
            .ThenByDescending(row => row.Id)
            .Include(row => row.Category)
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.FeaturedImage);

        const int perPage = 10;
        query = DataContext.SetPageToQuery(query, pageNumber, perPage);

        IList<WpPost> posts = await query.ToListAsync();

        foreach (WpPost post in posts)
        {
            foreach (WpPostTag postTag in post.PostTags)
            {
                postTag.Post = null;
                postTag.Tag.PostTags = null;
            }
        }

        ApiResponse<WpPost> apiResponse = ModelFactory.CreateApiResponse(posts, currentPage: pageNumber,
            perPage: perPage, totalCount: totalCount);
        apiResponse.PaginationData.AddCounter("tags", tagCountResult);
        apiResponse.PaginationData.AddCounter("ratings", ratingCountResult);

        return apiResponse;
    }

    private IQueryable<WpPost> CategorySlugFilter(IQueryable<WpPost> query)
    {
        return query.Where(row => row.Category!.Slug == _postsRequest.CategorySlug);
    }

    private IQueryable<WpPost> MetaRatingsFilter(IQueryable<WpPost> query)
    {
        return query.Where(row => row.MetaRating != null && _postsRequest.MetaRatings!.Contains((int)row.MetaRating));
    }

    private IQueryable<WpPost> SlugFilter(IQueryable<WpPost> query)
    {
        return query.Where(row => row.Slug == _postsRequest.Slug);
    }

    private IQueryable<WpPost> TagFilter(IQueryable<WpPost> query)
    {
        return query.Where(p => p.PostTags.Any(pt => _postsRequest.TagIds!.Contains(pt.TagId)));
    }
}