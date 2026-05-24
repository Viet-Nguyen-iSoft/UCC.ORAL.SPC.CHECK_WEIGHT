using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Database.DbContexts
{
  public class PostgresDbContext: CommonDbContext
  {
    //8wEzA1TMKby9eQsWXBaupj52
    //100.101.165.42
    //6921
    //DB name : DB_HSF
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      DotNetEnv.Env.Load();
      string server = Environment.GetEnvironmentVariable("DB_CONFIG_ADDRESS");
      string port = Environment.GetEnvironmentVariable("DB_CONFIG_PORT");
      string user = Environment.GetEnvironmentVariable("DB_CONFIG_USERNAME");
      string passwords = Environment.GetEnvironmentVariable("DB_CONFIG_PASSWORD");
      string name_db = Environment.GetEnvironmentVariable("DB_CONFIG_DATABASE_NAME");

      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseNpgsql(
                                      $"Server={server};" +
                                      $"Port={port};" +
                                      $"Database={name_db};" +
                                      $"User Id={user};" +
                                      $"Password={passwords};"
                                      );
    }
    }
  }
}
