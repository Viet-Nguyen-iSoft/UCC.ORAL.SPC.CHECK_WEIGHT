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
  public class DatalogService
  {
    private PgDbContext _context;
    private RepositoryDatalog _repositoryDatalog;

    public DatalogService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryDatalog = new RepositoryDatalog(_context);
    }

    public async Task<List<Datalog>> GetAllDataByTimeAsync(DateTime from, DateTime to)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryDatalog = new RepositoryDatalog(db);
          return await _repositoryDatalog.GetAllDataByTimeAsync(from, to);
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task AddAsync(Datalog datalog)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryDatalog = new RepositoryDatalog(db);
          await _repositoryDatalog.AddAsync(datalog);
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
