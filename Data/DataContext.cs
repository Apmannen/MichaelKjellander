using MichaelKjellander.Config;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MichaelKjellander.Data;

public abstract class DataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ServerVersion serverVersion = ServerVersion.Create(8, 4, 0, ServerType.MySql);
        optionsBuilder.UseMySql(
            connectionString: EnvironmentUtil.GetMysqlConnectionString(),
            serverVersion: serverVersion,
            mySqlOptionsAction: null);
    }

    protected static bool IsCalledByPopulateScript => EnvironmentUtil.GetAppEnvironment() == AppEnvironment.Unknown;
    
    protected static void ClearTable<T>(DbSet<T> dbSet) where T : class
    {
        dbSet.RemoveRange(dbSet);
    }
}