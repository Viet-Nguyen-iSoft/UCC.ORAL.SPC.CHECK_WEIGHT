using Database.DTO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.DtoHelper
{
  public static class HelperDTO
  {
    public const int TimeZoom = 7;

    public static List<ProductDTO> ConvertProductDTO(List<Product> roleGroups, string txtSearch)
    {
      IEnumerable<Product> query = roleGroups;

      if (!string.IsNullOrWhiteSpace(txtSearch))
      {
        txtSearch = txtSearch.Trim();

        query = query.Where(x =>
         (!string.IsNullOrEmpty(x.Code) &&
          x.Code.IndexOf(txtSearch, StringComparison.OrdinalIgnoreCase) >= 0)
          || (!string.IsNullOrEmpty(x.ProName) &&
              x.ProName.IndexOf(txtSearch, StringComparison.OrdinalIgnoreCase) >= 0)
          || (!string.IsNullOrEmpty(x.Description) &&
              x.Description.IndexOf(txtSearch, StringComparison.OrdinalIgnoreCase) >= 0)
          || (!string.IsNullOrEmpty(x.Type) &&
              x.Type.IndexOf(txtSearch, StringComparison.OrdinalIgnoreCase) >= 0)
          || (!string.IsNullOrEmpty(x.Note) &&
              x.Note.IndexOf(txtSearch, StringComparison.OrdinalIgnoreCase) >= 0)
        );
      }

      return query
          .OrderBy(x => x.Code)
          .Select((e, index) => new ProductDTO
          {
            No = index + 1,
            ProName = e.ProName,
            Code = e.Code,
            Description = e.Description,
            Type = e.Type,
            WeightOnPack = e.WeightOnPack,
            Unit = e.Unit, // sửa lại, đang gán nhầm WeightOnPack
            USL = e.USL,
            UCL = e.UCL,
            Target = e.Target,
            LCL = e.LCL,
            LSL = e.LSL,
            Density = e.Density,
            T = e.T,
            Tare = e.Tare,
            Note = e.Note,
            Datetime = e.CreatedAt?.AddHours(TimeZoom).ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"
          })
          .ToList();
    }

    public static List<EmployeeDTO> ConvertEmployeeDTO(List<Employee> employees)
    {
      if (employees == null || employees.Count == 0)
        return new List<EmployeeDTO>();

      return employees
      .OrderBy(x => x.EnumTypeEmployee)
      .ThenBy(x => x.FullName).ThenBy(x=>x.CreatedAt)
      .Select((x, index) => new EmployeeDTO
      {
        Employee = x,
        No = index + 1,
        FullName = x.FullName,
        DatetimeCreate = ((DateTime)x.CreatedAt).AddHours(TimeZoom).ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A",
        DatetimeUpdate = ((DateTime)x.UpdatedAt).AddHours(TimeZoom).ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A",
        TypeEmployee = GetEnumDescription(x.EnumTypeEmployee)
      }).ToList();
    }


    private static string GetEnumDescription(EnumTypeEmployee value)
    {
      FieldInfo field = value.GetType().GetField(value.ToString());

      if (field != null)
      {
        DescriptionAttribute attribute = field
            .GetCustomAttribute<DescriptionAttribute>();

        if (attribute != null)
          return attribute.Description;
      }

      return value.ToString();
    }

    public static List<DatalogDTO> ConvertDatalogDTO(List<Datalog> datalogs)
    {
      return datalogs
          .OrderBy(x => x.CreatedAt)
          .Select((e, index) => new DatalogDTO
          {
            No = index + 1,
            Gross = e.Gross,
            TareTube = e.TareTube,
            TareTailTube = e.TareTailTube,
            TareCarton = e.TareCarton,
            LotTube = e.LotTube,
            LotCarton = e.LotCarton,
            Status = GetEnumDescription(e.EnumStatusRecord),
            EnumStatusRecord = e.EnumStatusRecord,
            DateTime = e.CreatedAt?.AddHours(TimeZoom).ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"
          })
          .ToList();
    }

    private static string GetEnumDescription(EnumStatusRecord value)
    {
      FieldInfo field = value.GetType().GetField(value.ToString());

      if (field != null)
      {
        DescriptionAttribute attribute = field
            .GetCustomAttribute<DescriptionAttribute>();

        if (attribute != null)
          return attribute.Description;
      }

      return value.ToString();
    }

  }
}
