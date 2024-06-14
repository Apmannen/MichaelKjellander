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
    public DbSet<WpTag> Tags { get; set; }
    public DbSet<WpPostTag> PostTags { get; set; }
}