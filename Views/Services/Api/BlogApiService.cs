using MichaelKjellander.Config;
using MichaelKjellander.Domains.ApiResponse;
using MichaelKjellander.Domains.Models.Wordpress;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Views.Services.Api;

public class BlogApiService : InternalApiService
{
    public BlogApiService(HttpClient client, IOptions<AppConfig> options) : base(client, options)
    {
    }

    public async Task<IList<WpCategory>> FetchCategories()
    {
        ApiResponse<WpCategory> response = await FetchModels<WpCategory>(ApiRoutes.Categories);
        return response.Items!;
    }

    public async Task<IList<WpTag>> FetchTags(string categorySlug)
    {
        ApiResponse<WpTag> response = await FetchModels<WpTag>(ApiRoutes.Tags(categorySlug));
        return response.Items!;
    }

    public async Task<IList<WpPage>> FetchPages(PageIdentifier? identifier = null, string? slug = "")
    {
        ApiResponse<WpPage> response = await FetchModels<WpPage>(ApiRoutes.Pages(identifier, slug));
        return response.Items!;
    }

    public async Task<ApiResponse<WpPost>> FetchPosts(int pageNumber = 1, string? categorySlug = null,
        ICollection<int>? tagIds = null, ICollection<int>? metaRatings = null, string? postSlug = null)
    {
        return await FetchModels<WpPost>(ApiRoutes.Posts(pageNumber, categorySlug, tagIds, metaRatings,
            postSlug));
    }
}