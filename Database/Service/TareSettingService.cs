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
  public class TareSettingService
  {
    private PgDbContext _context;
    private RepositoryTareSetting _repositoryTareSetting;

    public TareSettingService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryTareSetting = new RepositoryTareSetting(_context);
    }

    public async Task<TareSetting> GetFirstDataAsync()
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryTareSetting = new RepositoryTareSetting(db);
          return await _repositoryTareSetting.GetFirstlDataAsync();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task AddAsync(TareSetting tareSetting)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryTareSetting = new RepositoryTareSetting(db);
          await _repositoryTareSetting.AddAsync(tareSetting);
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
