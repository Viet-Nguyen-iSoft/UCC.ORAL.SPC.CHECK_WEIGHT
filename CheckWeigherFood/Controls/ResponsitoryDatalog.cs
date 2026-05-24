using CheckWeigherFood.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CheckWeigherFood.Models.DbStore;

namespace CheckWeigherFood.Controls
{
  public class ResponsitoryDatalog: GenericRepository<Datalog, DailyDBContext>
  {
    public ResponsitoryDatalog(DbContext context) : base(context)
    {

    }


    public override async Task<List<Datalog>> GetDataReportByFilter(int productId)
    {
      List<Datalog> data = new List<Datalog>();
      try
      {
        data = await this.Context.Set<Datalog>().ToListAsync();
        if (data != null)
        {
          if (productId == -1)
          {
            //Nothing
          }
          else
          {
            data = data.Where(x => x.ProductId == productId).ToList();
          }
        }
      }
      catch (Exception ex)
      {
      }
      return data;
    }

    public override async Task<List<Datalog>> GetDataReportByFilterShift12(int productId)
    {
      List<Datalog> data = new List<Datalog>();
      try
      {
        data = await this.Context.Set<Datalog>().ToListAsync();
        if (data != null)
        {
          if (productId == -1)
          {
            //Nothing
            data = data.Where(x => x.CreatedAt.Value.Hour >=6).ToList();
          }
          else
          {
            data = data.Where(x => x.ProductId == productId & x.ShiftId != 3).ToList();
          }
        }
      }
      catch (Exception ex)
      {
      }
      return data;
    }

    public override async Task<List<Datalog>> GetDataReportByFilterShift3(int productId)
    {
      List<Datalog> data = new List<Datalog>();
      try
      {
        data = await this.Context.Set<Datalog>().ToListAsync();
        if (data != null)
        {
          if (productId == -1)
          {
            data = data.Where(x => x.ShiftId == 3).ToList();
          }
          else
          {
            data = data.Where(x => x.ProductId == productId & x.ShiftId == 3).ToList();
          }
        }
      }
      catch (Exception ex)
      {
      }
      return data;
    }






  }
}
