using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.Services;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Data;

public class BlogDataContext : DataContext
{
    public DbSet<WpCategory> Categories { get; set; }
    public DbSet<WpImage> Images { get; set; }
    public DbSet<WpPage> Pages { get; set; }
    public DbSet<WpPost> Posts { get; set; }

    //TODO: move to a new file? Scripts? 
    public async Task FillData(WpApiService service)
    {
        ClearTable(Categories);
        ClearTable(Images);
        ClearTable(Pages);
        ClearTable(Posts);

        //Categories
        IList<WpCategory> categories = await service.GetCategories();
        this.Categories.AddRange(categories);
        await SaveChangesAsync();
        
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
                    this.AddIfIdDoesntExist(Images, post.FeaturedImage);
                    await SaveChangesAsync();
                }
                post.Category = null;
                post.FeaturedImage = null;
            }
            
            this.Posts.AddRange(posts);
            await SaveChangesAsync();

            currentPage++;
            if (currentPage > numPages || posts.Count == 0)
            {
                break;
            }
        }
        await SaveChangesAsync();
        
        //Pages
        IList<WpPage> pages = await service.GetPages();
        this.Pages.AddRange(pages);
        await SaveChangesAsync();
    }
}