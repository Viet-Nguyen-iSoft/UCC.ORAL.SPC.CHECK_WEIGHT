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
  internal class RepositoryDatalog : GenericRepository<Datalog, PgDbContext>
  {
    public RepositoryDatalog(PgDbContext context) : base(context)
    {

    }

    public async Task<List<Datalog>> GetAllDataByTimeAsync(DateTime from, DateTime to, long productId)
    {
      try
      {
        return await this.Context.Set<Datalog>()
             .Where(x =>
                 x.DeletedFlag == false &&
                 x.ProductId == productId &&
                 x.CreatedAt >= from &&
                 x.CreatedAt <= to)
             .ToListAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}