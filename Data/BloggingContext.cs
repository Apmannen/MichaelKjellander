namespace MichaelKjellander.Data;

using MichaelKjellander.Models;
using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string connectionString = "server=localhost;user=root;password=;database=kjelledb";
        MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 3, 0));
        options.UseMySql(connectionString, serverVersion);
    }
}