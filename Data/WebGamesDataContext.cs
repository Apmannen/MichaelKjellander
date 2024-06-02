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
}