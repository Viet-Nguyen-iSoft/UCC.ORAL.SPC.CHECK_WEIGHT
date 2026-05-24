using Database.DbContexts;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Controls
{
  public static class AppFactory
  {

    public static ProductService CreateProductService()
    {
      var context = new PgDbContext();
      return new ProductService(context);
    }

    public static EmployeeService CreateEmployeeService()
    {
      var context = new PgDbContext();
      return new EmployeeService(context);
    }
  }
}
