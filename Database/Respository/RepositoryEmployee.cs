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
  public class RepositoryEmployee : GenericRepository<Employee, PgDbContext>
  {
    public RepositoryEmployee(PgDbContext context) : base(context)
    {
    }

    public async Task<List<Employee>> GetAllDataAsync(EnumTypeEmployee enumTypeEmployee)
    {
      try
      {
        if (enumTypeEmployee != EnumTypeEmployee.None)
        {
          return await this.Context.Set<Employee>()
                            .Where(x => x.DeletedFlag == false && x.EnumTypeEmployee == enumTypeEmployee)
                            .ToListAsync();
        }

        return await this.Context.Set<Employee>()
                            .Where(x => x.DeletedFlag == false)
                            .ToListAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
      try
      {
        var data = await this.Context.Set<Employee>()
                            .Where(x => x.Id == employee.Id)
                            .FirstOrDefaultAsync();

        if (data!=null)
        {
          data.FullName = employee.FullName;
          data.EnumTypeEmployee = employee.EnumTypeEmployee;
          data.UpdatedAt = employee.UpdatedAt;

          this.Context.Set<Employee>().Update(data);
        }
        return data;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<Employee> RemoveAsync(Employee employee)
    {
      try
      {
        var data = await this.Context.Set<Employee>()
                            .Where(x => x.Id == employee.Id)
                            .FirstOrDefaultAsync();

        if (data != null)
        {
          data.DeletedFlag = true;
          data.UpdatedAt = DateTime.UtcNow;

          this.Context.Set<Employee>().Update(data);
        }
        return data;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}

