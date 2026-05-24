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
  public class ResponsitoryLine : GenericRepository<Line, ConfigDBContext>
  {
    public ResponsitoryLine() : base()
    {

    }
    public ResponsitoryLine(DbContext context) : base(context)
    {

    }

    public override async Task<bool> UpdateInfoLine(Line line)
    {
      Context.Database.BeginTransaction();
      try
      {
        this.Context.Set<Line>().Update(line);
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
