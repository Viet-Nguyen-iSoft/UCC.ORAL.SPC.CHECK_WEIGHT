using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbContexts
{
  public class CommonDbContext : DbContext
  {
    public virtual DbSet<AppConfig> AppConfigs { get; set; }
    public virtual DbSet<Datalog> Datalogs { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Machine> Machines { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<TareSetting> TareSettings { get; set; }
    public virtual DbSet<OperationSetting> OperationSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //Employee
      modelBuilder.Entity<Datalog>()
             .HasOne(l => l.EmployeeOP)
             .WithMany(u => u.DatalogOP)
             .HasForeignKey(l => l.EmployeeOPId)
             .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Datalog>()
         .HasOne(l => l.EmployeeQC)
         .WithMany(u => u.DatalogQC)
         .HasForeignKey(l => l.EmployeeQCId)
         .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Datalog>()
         .HasOne(l => l.EmployeeSL)
         .WithMany(u => u.DatalogSL)
         .HasForeignKey(l => l.EmployeeSLId)
         .OnDelete(DeleteBehavior.Restrict);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
  }
}
