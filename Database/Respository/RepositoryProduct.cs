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
  public class RepositoryProduct : GenericRepository<Product, PgDbContext>
  {
    public RepositoryProduct(PgDbContext context) : base(context)
    {
    }

    public async Task<List<Product>> GetAllDataAsync()
    {
      try
      {
        return await this.Context.Set<Product>()
                            .Where(x =>x.DeletedFlag == false)
                            .OrderBy(x=>x.Code)
                            .ToListAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<Product> GetDataByIdAsync(long id)
    {
      try
      {
        return await this.Context.Set<Product>()
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
