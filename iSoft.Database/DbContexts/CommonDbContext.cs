using iSoft.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace iSoft.Database.DbContexts
{
  public class CommonDbContext : DbContext
  {
    public virtual DbSet<AppConfig>? AppConfigs { get; set; }
    public virtual DbSet<Datalog>? Datalogs { get; set; }
    public virtual DbSet<Employee>? Employees { get; set; }
    public virtual DbSet<Machine>? Machines { get; set; }
    public virtual DbSet<Product>? Products { get; set; }
    public virtual DbSet<Tare>? Tares { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasPostgresExtension("unaccent");
      modelBuilder.ConfigureDateTimeProperties("timestamp with time zone");

      AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


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
