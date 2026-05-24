using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.DTO
{
  public class EmployeeDTO
  {
    [DisplayName("Stt")]
    public int No { get; set; }

    [DisplayName("Nhóm")]
    public string TypeEmployee { get; set; }


    [DisplayName("Họ và tên")]
    public string FullName { get; set; }

    
    [DisplayName("Thời gian tạo")]
    public string DatetimeCreate { get; set; }

    [DisplayName("Thời gian cập nhật mới nhất")]
    public string DatetimeUpdate { get; set; }
  }
}
