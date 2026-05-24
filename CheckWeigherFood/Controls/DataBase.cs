using CheckWeigherFood.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.Models.DbStore;

namespace CheckWeigherFood.Controls
{
  public class DataBase
  {
    private static DataBase _ins;

    public static DataBase Ins
    {
      get { return _ins == null ? _ins = new DataBase() : _ins; }
    }
    public static string DailyDbPath { get; set; }
    public static string ConfigDbPath { get; set; } = $"./configDb.sqlite";

    //public static 
    public static async Task<int> Init()
    {
      var folder = Application.StartupPath + $"\\DataBase";
      if (!Directory.Exists($"{folder}"))
      {
        Directory.CreateDirectory($"{folder}");
      }

      using (var db = new ConfigDBContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();

          if (db.Lines.Count() <= 0)
          {
            await db.Lines.AddRangeAsync(new Line[]
            {
              new Line(){ NameLine="Kawa2", PathReport="abc", PathDataBase="effg",IpPLC="192.168.1.120",Port=2000,CreatedAt=DateTime.Now,UpdatedAt=DateTime.Now, IsEnable = true},
              new Line(){NameLine = "Kawa1", PathReport = "abc", PathDataBase = "effg", IpPLC = "192.168.1.120", Port = 2000, CreatedAt = DateTime.Now, UpdatedAt= DateTime.Now, IsEnable = false},
            });
          }
          await db.SaveChangesAsync();
          db.Database.CommitTransaction();
        }
        catch (Exception ex)
        {
          db.Database.RollbackTransaction();
          AppCore.Ins.LogErrorToFileLog(ex.ToString());
        }

        DateTime dt = DateTime.Now;
        using (var daily = new DailyDBContext(dt.AddDays(-1).ToString("yyMMdd")))
        {
          await daily.Database.EnsureCreatedAsync();
        }
        using (var daily = new DailyDBContext(dt.ToString("yyMMdd")))
        {
          await daily.Database.EnsureCreatedAsync();
        }
        using (var daily = new DailyDBContext(dt.AddDays(1).ToString("yyMMdd")))
        {
          await daily.Database.EnsureCreatedAsync();
        }
      }
      return 1;
    }



  }
}
