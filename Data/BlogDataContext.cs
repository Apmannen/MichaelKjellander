using MichaelKjellander.Components.Pages;
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

    public async Task CheckFillData(WpApiService service)
    {
        WpPost? aPost = this.Posts.FirstOrDefault();
        if (aPost != null)
        {
            return;
        }
        
        int currentPage = 1;
        while (true)
        {
            (IList<WpPost> posts, int numPages) = await service.GetPosts();
            this.AddRange(posts);

            currentPage++;
            if (currentPage > numPages || posts.Count == 0)
            {
                break;
            }
        }
        await this.SaveChangesAsync();
    }
}