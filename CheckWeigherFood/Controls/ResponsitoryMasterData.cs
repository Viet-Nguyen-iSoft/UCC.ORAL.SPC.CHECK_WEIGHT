using CheckWeigherFood.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CheckWeigherFood.Models.DbStore;

namespace CheckWeigherFood.Controls
{
  public class ResponsitoryMasterData : GenericRepository<MasterData, ConfigDBContext>
  {
    public ResponsitoryMasterData() : base()
    {

    }
    public ResponsitoryMasterData(DbContext context) : base(context)
    {

    }

    public override async Task<bool> UpdateRangeMasterDataOld()
    {
      Context.Database.BeginTransaction();
      try
      {
        var existed = await this.Context.Set<MasterData>().Where(s => s.isDelete == false).ToListAsync();
        if (existed != null)
        {
          foreach (var item in existed)
          {
            item.isDelete = true;
          }
        }
        //this.Context.Set<MasterData>().Update(entity);
        await this.Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
        return true;
      }
      catch (Exception ex)
      {
        Context.Database.RollbackTransaction();
        throw ex;
      }
    }


    public override async Task<bool> ClearActiveProductCurrent()
    {
      Context.Database.BeginTransaction();
      try
      {
        var existed = await this.Context.Set<MasterData>().Where(s => s.isDelete == false).ToListAsync();
        if (existed != null)
        {
          foreach (var item in existed)
          {
            item.isEnable = false;
          }
        }
        //this.Context.Set<MasterData>().Update(entity);
        await this.Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
      return true;
    }

    public override async Task<bool> ActiveProductCurrent(string FGs)
    {
      Context.Database.BeginTransaction();
      try
      {
        var existed = await this.Context.Set<MasterData>().Where(s => s.isDelete == false & s.FGs == FGs).ToListAsync();
        if (existed != null)
        {
          foreach (var item in existed)
          {
            item.isEnable = true;
          }
        }
        //this.Context.Set<MasterData>().Update(entity);
        await this.Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
      return true;
    }






  }
}
