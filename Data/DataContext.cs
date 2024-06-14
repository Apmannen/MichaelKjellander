using MichaelKjellander.Config;
using MichaelKjellander.SharedUtils;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MichaelKjellander.Data;

public abstract class DataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ServerVersion serverVersion = ServerVersion.Create(8, 4, 0, ServerType.MySql);
        string connectionString = EnvironmentUtil.Get(EnvVariable.SG_MYSQLCONNSTRING);
        optionsBuilder.UseMySql(
            connectionString: connectionString,
            serverVersion: serverVersion,
            mySqlOptionsAction: null);
    }

    protected bool IsCalledByPopulateScript
    {
        get
        {
            return EnvironmentUtil.ParseEnum(EnvVariable.SG_APPENVIRONMENT, AppEnvironment.Unknown) ==
                   AppEnvironment.Unknown;
        }
    }
}