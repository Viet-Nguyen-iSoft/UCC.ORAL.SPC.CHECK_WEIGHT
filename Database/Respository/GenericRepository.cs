using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Respository
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
        await Context.Set<TEntity>().AddAsync(entity);
        return entity;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message, ex.StackTrace);
        throw ex;
      }
    }

    public TEntity Add(TEntity entity)
    {
      try
      {
        Context.Set<TEntity>().Add(entity);
        return entity;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message, ex.StackTrace);
        throw ex;
      }
    }

    public async Task AddRangeAsync(List<TEntity> entities)
    {
      try
      {
        await Context.Set<TEntity>().AddRangeAsync(entities);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void AddRange(List<TEntity> entities)
    {
      try
      {
        Context.Set<TEntity>().AddRange(entities);
        Context.SaveChanges();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }


    public void DeleteAsync(TEntity entity)
    {
      try
      {
        Context.Set<TEntity>().Remove(entity);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void DeleteRangeAsync(List<TEntity> entities)
    {
      try
      {
        Context.Set<TEntity>().RemoveRange(entities);
      }
      catch (Exception)
      {
        Context.Database.RollbackTransaction();
      }
    }

    public virtual bool Update(TEntity entity)
    {
      try
      {
        Context.Set<TEntity>().Update(entity);
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public virtual bool UpdateRangeAsync(List<TEntity> entities)
    {
      try
      {
        Context.Set<TEntity>().UpdateRange(entities);
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

  }
}
