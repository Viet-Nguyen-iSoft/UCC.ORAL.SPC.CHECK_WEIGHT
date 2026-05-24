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

  }
}
