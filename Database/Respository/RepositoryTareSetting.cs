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
  public class RepositoryTareSetting : GenericRepository<TareSetting, PgDbContext>
  {
    public RepositoryTareSetting(PgDbContext context) : base(context)
    {
    }

    public async Task<TareSetting> GetFirstlDataAsync()
    {
      try
      {
        return await this.Context.Set<TareSetting>()
                            .Where(x => x.DeletedFlag == false)
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
