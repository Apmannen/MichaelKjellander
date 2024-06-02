using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MichaelKjellander.Data;

public class WebGamesDataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ServerVersion serverVersion = ServerVersion.Create(8, 4, 0, ServerType.MySql);
        string connectionString =
            "server=localhost:3306;database=kjelle_db;user=mickj;password=test;SslMode=none";
        optionsBuilder.UseMySql(
            connectionString: connectionString, 
            serverVersion: serverVersion,
            mySqlOptionsAction: null);
    }
}