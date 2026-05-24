using Microsoft.EntityFrameworkCore;
using System;

namespace Database.DbContexts
{
  public static class ModelBuilderExtensions
  {
    public static void ConfigureDateTimeProperties(this ModelBuilder modelBuilder, string columnType)
    {
      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
        foreach (var property in entityType.ClrType.GetProperties())
        {
          if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
          {
            modelBuilder.Entity(entityType.Name).Property(property.Name).HasColumnType(columnType);
          }
        }
      }
    }
  }
}
