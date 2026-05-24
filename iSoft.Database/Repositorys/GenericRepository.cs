using iSoft.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Database.Repositorys
{
  public class GenericRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
  {
    public DbContext Context { get; set; }
    public GenericRepository(DbContext context)
    {
      this.Context = context;
    }

    public GenericRepository()
    {

    }

    public async Task<List<TEntity>> GetAllAsync()
    {
      return await this.Context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetFirstOrDefaultAsync()
    {
      return await this.Context.Set<TEntity>().FirstOrDefaultAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
      try
      {
        await Context.Database.BeginTransactionAsync();
        await Context.Database.EnsureCreatedAsync();
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
        return entity;
      }
      catch (Exception ex)
      {
        Context.Database.RollbackTransaction();
        Console.WriteLine(ex.Message, ex.StackTrace);
        throw ex;
      }
    }

    public TEntity Add(TEntity entity)
    {
      try
      {
        Context.Database.BeginTransaction();
        Context.Database.EnsureCreated();
        Context.Set<TEntity>().Add(entity);
        Context.SaveChanges();
        Context.Database.CommitTransaction();
        return entity;
      }
      catch (Exception ex)
      {
        Context.Database.RollbackTransaction();
        Console.WriteLine(ex.Message, ex.StackTrace);
        throw ex;
      }
    }

    public async Task AddRangeAsync(List<TEntity> entities)
    {
      try
      {
        await Context.Database.EnsureCreatedAsync();
        await Context.Database.BeginTransactionAsync();
        await Context.Set<TEntity>().AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception ex)
      {
        Context.Database.RollbackTransaction();
        throw ex;
      }
    }

    public void AddRange(List<TEntity> entities)
    {
      try
      {
        Context.Database.EnsureCreated();
        Context.Database.BeginTransaction();
        Context.Set<TEntity>().AddRange(entities);
        Context.SaveChanges();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
    }


    public async Task DeleteAsync(TEntity entity)
    {
      try
      {
        await Context.Database.BeginTransactionAsync();
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
    }

    public async Task DeleteRangeAsync(List<TEntity> entities)
    {
      try
      {
        await Context.Database.BeginTransactionAsync();
        Context.Set<TEntity>().RemoveRange(entities);
        await Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
    }

    public async virtual Task<bool> UpdateAsync(TEntity entity)
    {
      try
      {
        await Context.Database.EnsureCreatedAsync();
        await Context.Database.BeginTransactionAsync();
        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
        return true;
      }
      catch (Exception ex)
      {
        Context.Database.RollbackTransaction();
        return false;
      }
    }

    public async virtual Task<bool> UpdateRangeAsync(List<TEntity> entities)
    {
      try
      {
        await Context.Database.EnsureCreatedAsync();
        await Context.Database.BeginTransactionAsync();
        Context.Set<TEntity>().UpdateRange(entities);
        await Context.SaveChangesAsync();
        Context.Database.CommitTransaction();
        return true;
      }
      catch (Exception ex)
      {
        Context.Database.RollbackTransaction();
        return false;
      }
    }

  }
}
