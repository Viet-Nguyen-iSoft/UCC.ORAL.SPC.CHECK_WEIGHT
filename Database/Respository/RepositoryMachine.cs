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
  public class RepositoryMachine : GenericRepository<Machine, PgDbContext>
  {
    public RepositoryMachine(PgDbContext context) : base(context)
    {
    }

    public async Task<Machine> GetFirstlDataAsync()
    {
      try
      {
        return await this.Context.Set<Machine>()
                            .Where(x => x.DeletedFlag == false && x.EnableFlag == true)
                            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}