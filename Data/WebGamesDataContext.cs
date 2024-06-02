using MichaelKjellander.Models.WebGames;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MichaelKjellander.Data;

public class WebGamesDataContext : DbContext
{
    public DbSet<Word> Words { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ServerVersion serverVersion = ServerVersion.Create(8, 4, 0, ServerType.MySql);
        //TODO: configurable connect string
        string connectionString =
            "Server=127.0.0.1;Database=kjelle_db;User=root;Password=test;Port=3306;SslMode=none;";

        optionsBuilder.UseMySql(
            connectionString: connectionString, 
            serverVersion: serverVersion,
            mySqlOptionsAction: null);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<Word>().HasData(
            new Word { Id = 1, WordString = "Product1" },
            new Word { Id = 2, WordString = "Product2" }
        );
    }
}