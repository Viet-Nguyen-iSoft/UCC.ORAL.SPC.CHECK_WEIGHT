using iSoft.Database.DbContexts;
using iSoft.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Database.Repositorys
{
  public class MachineRepository : GenericRepository<Machine, CommonDbContext>
  {
    public MachineRepository(DbContext context) : base(context)
    {

    }

    //public async Task<List<Machine>> GetAllAsync(bool isContainDelete = false)
    //{
    //  if (isContainDelete)
    //  {
    //    return await this.Context.Set<Machine>()
    //      .Include(x=>x.Factory)
    //      .ToListAsync();
    //  }
    //  else
    //  {
    //    return await this.Context.Set<Machine>()
    //      .Where(x => !x.DeletedFlag)
    //      .Include(x => x.Factory)
    //      .ToListAsync();
    //  }  
    //}
  }
}
