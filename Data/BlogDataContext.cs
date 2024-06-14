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

    public async Task FillData(WpApiService service)
    {
        ClearTable(Categories);
        
        IList<WpCategory> categories = await service.GetCategories();
        this.Categories.AddRange(categories);
        
        int currentPage = 1;
        while (true)
        {
            
            
            (IList<WpPost> posts, int numPages) = await service.GetPosts();
            /*
            IList<WpCategory> categories = [];
            foreach (WpPost post in posts)
            {
                WpCategory? existingCategory = categories.FirstOrDefault(c => c.Id == post.Category!.Id);
                if (existingCategory != null)
                {
                    categories.Add(post.Category!);
                }
            }
            this.Categories.AddRange(categories);*/

            currentPage++;
            if (currentPage > numPages || posts.Count == 0)
            {
                break;
            }
        }
        await this.SaveChangesAsync();
        
    }
}