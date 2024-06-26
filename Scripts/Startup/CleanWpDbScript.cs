﻿using MichaelKjellander.Data;
using MichaelKjellander.Domains.Models.Wordpress;
using MichaelKjellander.Services;

namespace MichaelKjellander.Scripts.Startup;

public class CleanWpDbScript
{
    private readonly BlogDataContext _context;
    private readonly WpApiService _service;

    public CleanWpDbScript(BlogDataContext context, WpApiService service)
    {
        _context = context;
        _service = service;
    }

    private static void GenerateTKeysFile(List<WpTranslationEntry> translationEntries)
    {
        StreamWriter writer = new StreamWriter("Domains/Generated/TKeys.cs");
        writer.WriteLine("namespace MichaelKjellander.Domains.Generated;");
        writer.WriteLine("//Generated by CleanWpDbScript");
        writer.WriteLine("public enum TKey {");
        writer.WriteLine("Illegal,");
        foreach (WpTranslationEntry entry in translationEntries)
        {
            writer.WriteLine($"{entry.Key},");
        }
        writer.WriteLine("}");
        writer.Close();
    }
    
    public async Task Run()
    {
        DataContext.ClearTable(_context.PostTags);
        DataContext.ClearTable(_context.Tags);
        DataContext.ClearTable(_context.Categories);
        DataContext.ClearTable(_context.Images);
        DataContext.ClearTable(_context.Pages);
        DataContext.ClearTable(_context.Posts);
        DataContext.ClearTable(_context.TranslationEntries);
        
        //Translations
        List<WpTranslationEntry> translationEntries = await _service.GetTranslations();
        _context.TranslationEntries.AddRange(translationEntries);
        await _context.SaveChangesAsync();
        GenerateTKeysFile(translationEntries);
        
        //Tags
        IList<WpTag> tags = await _service.GetTags();
        _context.Tags.AddRange(tags);
        await _context.SaveChangesAsync();

        //Categories
        IList<WpCategory> categories = await _service.GetCategories();
        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();
        
        //Posts
        int currentPage = 1;
        while (true)
        {
            (IList<WpPost> posts, int numPages) = await _service.GetPosts(page: currentPage);
            foreach (WpPost post in posts)
            {
                if (post.FeaturedImage != null)
                {
                    //Images
                    DataContext.AddIfIdDoesntExist(_context.Images, post.FeaturedImage);
                    await _context.SaveChangesAsync();
                }
                post.Category = null;
                post.FeaturedImage = null;
            }
            
            _context.Posts.AddRange(posts);
            await _context.SaveChangesAsync();

            currentPage++;
            if (currentPage > numPages || posts.Count == 0)
            {
                break;
            }
        }
        await _context.SaveChangesAsync();
        
        //Pages
        IList<WpPage> pages = await _service.GetPages();
        _context.Pages.AddRange(pages);
        await _context.SaveChangesAsync();
    }
}