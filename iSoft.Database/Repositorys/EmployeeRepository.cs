using iSoft.Database.DbContexts;
using iSoft.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace iSoft.Database.Repositorys
{
  public class EmployeeRepository : GenericRepository<Employee, CommonDbContext>
  {
    public EmployeeRepository(DbContext context) : base(context)
    {

    }
    //public async Task<List<Employee>> GetAllAsync(string textSearch = "", bool isContainDelete = false)
    //{
    //  if (isContainDelete)
    //  {
    //    if (!string.IsNullOrWhiteSpace(textSearch))
    //    {
    //      string searchLower = textSearch.ToLower();
    //      return await this.Context.Set<Employee>()
    //      .Include(x => x.Department)
    //      .Where(e =>
    //         (e.FullName != null && e.FullName.ToLower().Contains(searchLower)) ||
    //         (e.Code != null && e.Code.ToLower().Contains(searchLower)) ||
    //         (e.IdCardCode != null && e.IdCardCode.ToLower().Contains(searchLower)) ||
    //         (e.IdCardName != null && e.IdCardName.ToLower().Contains(searchLower)) ||
    //         (e.Account != null && e.Account.ToLower().Contains(searchLower))
    //      ).ToListAsync();
    //    }
    //    else
    //    {
    //      return await this.Context.Set<Employee>()
    //        .Include(x => x.Department)
    //        .ToListAsync();
    //    }
    //  }
    //  else
    //  {
    //    if (!string.IsNullOrWhiteSpace(textSearch))
    //    {
    //      string searchLower = textSearch.ToLower();
    //      return await this.Context.Set<Employee>()
    //        .Include(x => x.Department)
    //        .Where(e =>
    //         (e.FullName != null && e.FullName.ToLower().Contains(searchLower)) ||
    //         (e.Code != null && e.Code.ToLower().Contains(searchLower)) ||
    //         (e.IdCardCode != null && e.IdCardCode.ToLower().Contains(searchLower)) ||
    //         (e.IdCardName != null && e.IdCardName.ToLower().Contains(searchLower)) ||
    //         (e.Account != null && e.Account.ToLower().Contains(searchLower))
    //        )
    //        .ToListAsync();
    //    }
    //    else
    //    {
    //      return await this.Context.Set<Employee>()
    //        .Where(x=>!x.DeletedFlag)
    //        .Include(x => x.Department)
    //        .ToListAsync();
    //    }
    //  }
    //}

    //public async Task<List<Employee>> GetAllAsync(bool isContainDelete = false)
    //{
    //  if (isContainDelete)
    //  {
    //    return await this.Context.Set<Employee>()
    //        .Include(x => x.Departments)
    //        .ToListAsync();
    //  }
    //  else
    //  {
    //    return await this.Context.Set<Employee>()
    //        .Where(x => !x.DeletedFlag)
    //        .Include(x => x.Departments)
    //        .ToListAsync();
    //  }
    //}
    //public async Task<List<Employee>> GetAllNotIncludeAsync(bool isContainDelete = false)
    //{
    //  if (isContainDelete)
    //  {
    //    return await this.Context.Set<Employee>()
    //        .ToListAsync();
    //  }
    //  else
    //  {
    //    return await this.Context.Set<Employee>()
    //        .Where(x => !x.DeletedFlag)
    //        .ToListAsync();
    //  }
    //}

    //public async Task<Employee> UpdateFlagDeleteAsync(long id)
    //{
    //  var rs = await this.Context.Set<Employee>()
    //    .FirstOrDefaultAsync(x => x.Id == id);

    //  if (rs != null)
    //  {
    //    rs.DeletedFlag = true;
    //    await this.Context.SaveChangesAsync();
    //  }

    //  return rs;
    //}

    //public async Task<Employee?> GetByIdAsync(long? id)
    //{
    //  return await this.Context.Set<Employee>()
    //    .FirstOrDefaultAsync(x => x.Id == id);
    //}

    //public async Task<Employee?> GetEmployeeByAccount(string account, string passwords)
    //{
    //  return await this.Context.Set<Employee>()
    //    .Where(x => x.DeletedFlag == false && (x.Account == account && x.Passwords == passwords))
    //    .Include(x => x.Departments)
    //    .FirstOrDefaultAsync();
    //}
  }
}
