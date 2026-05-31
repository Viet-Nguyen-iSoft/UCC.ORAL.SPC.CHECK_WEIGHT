using Database.DbContexts;
using Database.Models;
using Database.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
  public class MachineService
  {
    private PgDbContext _context;
    private RepositoryMachine _repositoryMachine;

    public MachineService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryMachine = new RepositoryMachine(_context);
    }

    public async Task<Machine> GetFirstlDataAsync()
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryMachine = new RepositoryMachine(db);
          return await _repositoryMachine.GetFirstlDataAsync();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task<List<Machine>> GetDataAsync()
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryMachine = new RepositoryMachine(db);
          return await _repositoryMachine.GetAllAsync();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task UpdateAsync(Machine machine)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryMachine = new RepositoryMachine(db);
          _repositoryMachine.Update(machine);
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
