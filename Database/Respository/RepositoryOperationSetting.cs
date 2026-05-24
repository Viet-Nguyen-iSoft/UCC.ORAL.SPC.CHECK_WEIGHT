using Database.DbContexts;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Respository
{
  public class RepositoryOperationSetting : GenericRepository<OperationSetting, PgDbContext>
  {
    public RepositoryOperationSetting(PgDbContext context) : base(context)
    {
    }

    public async Task<OperationSetting> GetFirstDataAsync()
    {
      try
      {
        return await this.Context.Set<OperationSetting>()
                            .Where(x => x.DeletedFlag == false)
                            .OrderByDescending(x=>x.Id)
                            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}

