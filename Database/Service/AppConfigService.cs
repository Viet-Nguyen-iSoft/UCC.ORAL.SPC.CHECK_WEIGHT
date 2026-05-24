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
  public class AppConfigService
  {
    private PgDbContext _context;
    private RepositoryAppConfig _repositoryAppConfig;

    public AppConfigService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryAppConfig = new RepositoryAppConfig(_context);
    }

    public async Task<AppConfig> GetFirstlDataAsync()
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryAppConfig = new RepositoryAppConfig(db);
          return await _repositoryAppConfig.GetFirstlDataAsync();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task UpdateAsync(AppConfig appConfig)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryAppConfig = new RepositoryAppConfig(db);
          _repositoryAppConfig.Update(appConfig);
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
