using CheckWeigherFood.Controls;
using Database.DbContexts;
using Database.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      string procName = Process.GetCurrentProcess().ProcessName;

      Process[] processes = Process.GetProcessesByName(procName);
      if (processes.Length > 1)
      {
        return;
      }

      //Khởi tạo Db
      InitDb().GetAwaiter().GetResult();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      //Application.Run(new FrmMain());
      AppCore.Ins.Init();
    }


    static async Task<bool> InitDb()
    {
      try
      {
        using (var db = new PgDbContext())
        {
          try
          {
            await db.Database.EnsureCreatedAsync();
            await db.Database.BeginTransactionAsync();

            if (db?.AppConfigs?.Count() <= 0)
            {
              await db.AppConfigs.AddAsync(new AppConfig
              {
                PathReport = "",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
              });
            }

            if (db?.Machines?.Count() <= 0)
            {
              await db.Machines.AddAsync(new Machine
              {
                Name = "Line 3",
                Code = "Line 3",
                Description = "Line 3",
                IP = "192.168.1.100",
                Port = 502,
                TimeCheckConnect = 1000,
                Timeout = 500,
                SampleTime = 1000,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
              });

              await db.Machines.AddAsync(new Machine
              {
                Name = "Line 4",
                Code = "Line 4",
                Description = "Line 4",
                IP = "192.168.1.101",
                Port = 502,
                TimeCheckConnect = 1000,
                Timeout = 500,
                SampleTime = 1000,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
              });
            }


            await db.SaveChangesAsync();
            db.Database.CommitTransaction();
          }
          catch (Exception)
          {
            db.Database.RollbackTransaction();
          }
          return true;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}
