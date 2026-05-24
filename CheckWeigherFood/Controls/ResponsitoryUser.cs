using CheckWeigherFood.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CheckWeigherFood.Models.DbStore;

namespace CheckWeigherFood.Controls
{
  public class ResponsitoryUser : GenericRepository<User, ConfigDBContext>
  {
    public ResponsitoryUser() : base()
    {

    }
    public ResponsitoryUser(DbContext context) : base(context)
    {

    }

    public override async Task<bool> CheckExitUser(string user)
    {
      bool isExit = false;
      Context.Database.BeginTransaction();
      try
      {
        var existed = await this.Context.Set<User>().Where(s => s.Name == user).ToListAsync();
        if (existed != null && existed.Count > 0) isExit = true;
      }
      catch (Exception)
      {
      }
      return isExit;
    }

    public override async Task ClearActiveUser(string Role)
    {
      try
      {
        Context.Database.BeginTransaction();
        var existed = await this.Context.Set<User>().Where(s => s.Role == Role).ToListAsync();
        if (existed != null && existed.Count > 0)
        {
          foreach (var item in existed)
          {
            item.isEnable = false;
          }
        }
        await this.Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
    }



    public override async Task ActiveUser(string User, string Role)
    {
      try
      {
        Context.Database.BeginTransaction();
        var existed = await this.Context.Set<User>().Where(s => s.Name == User && s.Role == Role).ToListAsync();
        if (existed != null && existed.Count > 0)
        {
          foreach (var item in existed)
          {
            item.isEnable = true;
          }
        }
        await this.Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
    }



  }
}
