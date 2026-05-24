using Database.DbContexts;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.Respository
{
  public class RepositoryAppConfig : GenericRepository<AppConfig, PgDbContext>
  {
    public RepositoryAppConfig(PgDbContext context) : base(context)
    {
    }

    public async Task<AppConfig> GetFirstlDataAsync()
    {
      try
      {
        return await this.Context.Set<AppConfig>()
                            .Where(x => x.DeletedFlag == false)
                            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}


