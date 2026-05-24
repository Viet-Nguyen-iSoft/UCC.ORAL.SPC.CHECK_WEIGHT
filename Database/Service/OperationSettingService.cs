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
  public class OperationSettingService
  {
    private PgDbContext _context;
    private RepositoryOperationSetting _repositoryOperationSetting;

    public OperationSettingService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryOperationSetting = new RepositoryOperationSetting(_context);
    }

    public async Task<OperationSetting> GetFirstDataAsync()
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryOperationSetting = new RepositoryOperationSetting(db);
          return await _repositoryOperationSetting.GetFirstDataAsync();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task<OperationSetting> AddAsync(OperationSetting operationSetting)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryOperationSetting = new RepositoryOperationSetting(db);
          await _repositoryOperationSetting.AddAsync(operationSetting);
          await db.SaveChangesAsync();
          db.Database.CommitTransaction();
          return operationSetting;
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
