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
  public class ProductService
  {
    private PgDbContext _context;
    private RepositoryProduct _repositoryProduct;

    public ProductService(PgDbContext dbContext)
    {
      _context = dbContext;
      _repositoryProduct = new RepositoryProduct(_context);
    }

    public async Task<List<Product>> GetAllAsync()
    {
      using (var db = new PgDbContext())
      {
        try
        {
          _repositoryProduct = new RepositoryProduct(db);
          return await _repositoryProduct.GetAllDataAsync();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    public async Task AddRangeAsync(List<Product> products)
    {
      using (var db = new PgDbContext())
      {
        try
        {
          await db.Database.EnsureCreatedAsync();
          await db.Database.BeginTransactionAsync();
          _repositoryProduct = new RepositoryProduct(db);
          await _repositoryProduct.AddRangeAsync(products);
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
