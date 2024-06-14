using MichaelKjellander.Data;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.Services;

namespace MichaelKjellander.Scripts.Startup;

public class CleanWpDbScript
{
    public async Task Run(BlogDataContext context, WpApiService service)
    {
        DataContext.ClearTable(context.Categories);
        DataContext.ClearTable(context.Images);
        DataContext.ClearTable(context.Pages);
        DataContext.ClearTable(context.Posts);

        //Categories
        IList<WpCategory> categories = await service.GetCategories();
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
        
        //Posts
        int currentPage = 1;
        while (true)
        {
            (IList<WpPost> posts, int numPages) = await service.GetPosts(page: currentPage);
            foreach (WpPost post in posts)
            {
                if (post.FeaturedImage != null)
                {
                    //Images
                    DataContext.AddIfIdDoesntExist(context.Images, post.FeaturedImage);
                    await context.SaveChangesAsync();
                }
                post.Category = null;
                post.FeaturedImage = null;
            }
            
            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();

            currentPage++;
            if (currentPage > numPages || posts.Count == 0)
            {
                break;
            }
        }
        await context.SaveChangesAsync();
        
        //Pages
        IList<WpPage> pages = await service.GetPages();
        context.Pages.AddRange(pages);
        await context.SaveChangesAsync();
    }
}