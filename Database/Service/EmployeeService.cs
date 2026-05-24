using Database.DbContexts;
using Database.Models;
using Database.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.Service
{
  public class EmployeeService
  {
    private PgDbContext _context;
    private RepositoryEmployee _repositoryEmployee;

    public EmployeeService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryEmployee = new RepositoryEmployee(_context);
    }

    public async Task<List<Employee>> GetAllAsync(EnumTypeEmployee enumTypeEmployee)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryEmployee = new RepositoryEmployee(db);
          return await _repositoryEmployee.GetAllDataAsync(enumTypeEmployee);
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task AddAsync(Employee employee)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryEmployee = new RepositoryEmployee(db);
          await _repositoryEmployee.AddAsync(employee);
          await db.SaveChangesAsync();
          db.Database.CommitTransaction();
        }
        catch (Exception ex)
        {
          db.Database.RollbackTransaction();
          throw ex;
        }
      }
    }
  }
}
