using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbContexts
{
  public class PgDbContext : CommonDbContext
  {
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
