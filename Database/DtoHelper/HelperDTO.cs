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
      var rs = roleGroups.OrderBy(x => x.CreatedAt)
          .Select((e, index) => new ProductDTO
          {
            No = index + 1, // STT bắt đầu từ 1
            ProName = e.ProName,
            Code = e.Code,
            Description = e.Description,
            Type = e.Type,
            WeightOnPack = e.WeightOnPack,
            Unit = e.WeightOnPack,
            USL = e.USL,
            UCL = e.UCL,
            Target = e.Target,
            LCL = e.LCL,
            LSL = e.LSL,
            Density = e.Density,
            T = e.T,
            Tare = e.Tare,
            Note = e.Note,

            Datetime = ((DateTime)e.CreatedAt).AddHours(TimeZoom).ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A",
          })
          .ToList();

      return rs;
    }

    public static List<EmployeeDTO> ConvertEmployeeDTO(List<Employee> employees)
    {
      if (employees == null || employees.Count == 0)
        return new List<EmployeeDTO>();

      return employees.Select((x, index) => new EmployeeDTO
      {
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
  }
}
