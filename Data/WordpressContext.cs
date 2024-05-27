using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.Data;

using MichaelKjellander.Models;
using Microsoft.EntityFrameworkCore;

public class WordpressContext : DbContext
{
    public DbSet<WpPost> WpPosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string connectionString = "server=localhost;user=root;password=;database=kjelle_wordpress";
        MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 3, 0));
        options.UseMySql(connectionString, serverVersion);
    }
}