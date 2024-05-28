using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.Data;

using MichaelKjellander.Models;
using Microsoft.EntityFrameworkCore;

[System.Obsolete("DB is not accessed anymore")]
public class WordpressContext : DbContext
{
    public DbSet<WpPost> WpPosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string connectionString = "server=localhost;user=root;password=;database=kjelle_wordpress";
        MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 3, 0));
        options.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WpPost>()
            .HasMany(p => p.TermRelationships)
            .WithOne(tr => tr.Post)
            .HasForeignKey(tr => tr.ObjectId);

        modelBuilder.Entity<WpTermRelationship>()
            .HasKey(tr => new { tr.ObjectId, tr.TermTaxonomyId });

        modelBuilder.Entity<WpTermRelationship>()
            .HasOne(tr => tr.Post)
            .WithMany(p => p.TermRelationships)
            .HasForeignKey(tr => tr.ObjectId);
    }
}