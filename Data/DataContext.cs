using MichaelKjellander.Config;
using MichaelKjellander.Models;
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

        if (EnvironmentUtil.GetAppEnvironment() == AppEnvironment.Local)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }

    protected static bool IsCalledByPopulateScript => EnvironmentUtil.GetAppEnvironment() == AppEnvironment.Unknown;

    public static void AddIfIdDoesntExist<T>(DbSet<T> dbSet, T newRow) where T : DbModel
    {
        T? existingRow = dbSet.FirstOrDefault(row => row.Id == newRow.Id);
        if (existingRow == null)
        {
            dbSet.Add(newRow);
        }
    }
    
    public static void ClearTable<T>(DbSet<T> dbSet) where T : DbModel
    {
        dbSet.RemoveRange(dbSet);
    }

    public static IQueryable<T> SetPageToQuery<T>(IQueryable<T> query, int page, int perPage) where T : DbModel
    {
        if (page > 1)
        {
            query = query.Skip(perPage * (page - 1));
        }
        query = query.Take(perPage);
        return query;
    }
}