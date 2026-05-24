using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.Models
{
  public class DbStore
  {
    public class DailyDBContext : DbContext
    {
      public string DailyDbPath = Application.StartupPath;
      DbSet<Datalog> Datalogs { get; set; }
      public DailyDBContext(string fileName)
      {
        //DailyDbPath = $"DataBase/{fileName}.sqlite";
        DailyDbPath = Application.StartupPath + $"\\DataBase\\{fileName}.sqlite";
        //DailyDbPath = Application.StartupPath + $"\\Database\\{fileName}.sqlite";
      }
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        if (!optionsBuilder.IsConfigured)
        {
          optionsBuilder.UseSqlite($"Data Source={DailyDbPath}");
        }
      }

      public DailyDBContext()
      {
        DailyDbPath += $"\\Database\\{DateTime.Now.ToString("yyMMdd")}.sqlite";
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {

      }
    }

    public class ConfigDBContext : DbContext
    {
      public DbSet<MasterData> MasterDatas { get; set; }
      public DbSet<Line> Lines { get; set; }
      public DbSet<User> Users { get; set; }

      public string DebugPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      public ConfigDBContext()
      {
        DebugPath += $"\\ConfigDB.sqlite";
      }
      public ConfigDBContext(string path)
      {
        DebugPath += $"\\ConfigDB.sqlite";
      }
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        if (!optionsBuilder.IsConfigured)
        {
          optionsBuilder.UseSqlite($"Data Source={DebugPath}");
        }
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
       
      }
    }
  }
}
