using MichaelKjellander.Data;
using MichaelKjellander.Models;
using MichaelKjellander.Models.Wordpress;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Api.ModelFeatures.Posts;

public class PostsDbService
{
    public static async Task<ApiResponse<WpPost>> GetPosts(PostsRequest postsRequest)
    {
        
        int pageNumber = postsRequest.Page ?? 1;

        await using var context = new BlogDataContext();
        IQueryable<WpPost> query = context.Posts;
        IQueryable<WpPost> tagCountQuery = context.Posts;
        IQueryable<WpPost> ratingCountQuery = context.Posts;
        FieldCounters fieldCounters = new FieldCounters();

        if (postsRequest.Slug != null)
        {
            query = query.Where(row => row.Slug == postsRequest.Slug);
        }

        if (postsRequest.CategorySlug != null)
        {
            query = query.Where(row => row.Category!.Slug == postsRequest.CategorySlug);
        }

        ratingCountQuery = query;
        if (postsRequest.MetaRatings is { Count: > 0 })
        {
            query = query.Where(row =>
                row.MetaRating != null && postsRequest.MetaRatings.Contains((int)row.MetaRating));
        }

        tagCountQuery = query;

        if (postsRequest.TagIds is { Count: > 0 })
        {
            IQueryable<WpPost> TagFilter(IQueryable<WpPost> q)
            {
                return q.Where(p => p.PostTags.Any(pt => postsRequest.TagIds.Contains(pt.TagId)));
            }

            query = TagFilter(query);
            ratingCountQuery = TagFilter(ratingCountQuery);
        }

        var tagCountResult = tagCountQuery
            .SelectMany(p => p.PostTags)
            .GroupBy(pt => pt.Tag)
            .Select(g =>
                new //TODO: make an interface here to ease the dictionary conversion process!! Or just use KeyValue.
                {
                    Tag = g.Key,
                    Count = g.Count()
                }).ToList();
        var ratingCountResult = ratingCountQuery.GroupBy(p => p.MetaRating).Select(g =>
            new
            {
                Rating = g.Key,
                Count = g.Count()
            }).ToList();
        fieldCounters.AddCounter("tags", tagCountResult, tagCount => tagCount.Tag.Name!,
            tagCount => tagCount.Count);
        fieldCounters.AddCounter("ratings", ratingCountResult, ratingCount => ratingCount.Rating.ToString()!,
            ratingCount => ratingCount.Count);

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

        return ModelFactory.CreateApiResponse(posts, currentPage: pageNumber, perPage: perPage,
            totalCount: totalCount, fieldCounts: fieldCounters);
    }
}


