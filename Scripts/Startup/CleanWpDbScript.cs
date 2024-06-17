using MichaelKjellander.Communicators;
using MichaelKjellander.Data;
using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.Scripts.Startup;

public class CleanWpDbScript
{
    public async Task Run(BlogDataContext context, WpApiCommunicator communicator)
    {
        DataContext.ClearTable(context.PostTags);
        DataContext.ClearTable(context.Tags);
        DataContext.ClearTable(context.Categories);
        DataContext.ClearTable(context.Images);
        DataContext.ClearTable(context.Pages);
        DataContext.ClearTable(context.Posts);
        DataContext.ClearTable(context.TranslationEntries);
        
        //Translations
        List<WpTranslationEntry> translationEntries = await communicator.GetTranslations();
        context.TranslationEntries.AddRange(translationEntries);
        await context.SaveChangesAsync();
        
        //Tags
        IList<WpTag> tags = await communicator.GetTags();
        context.Tags.AddRange(tags);
        await context.SaveChangesAsync();

        //Categories
        IList<WpCategory> categories = await communicator.GetCategories();
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
        
        //Posts
        int currentPage = 1;
        while (true)
        {
            (IList<WpPost> posts, int numPages) = await communicator.GetPosts(page: currentPage);
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
        IList<WpPage> pages = await communicator.GetPages();
        context.Pages.AddRange(pages);
        await context.SaveChangesAsync();
    }
}