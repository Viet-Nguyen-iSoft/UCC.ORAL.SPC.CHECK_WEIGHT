using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
  public class BaseModel
  {
    [Browsable(false)]
    [DisplayName("Id")]
    public long Id { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime? CreatedAt { get; set; }

    [DisplayName("Cập nhật")]
    public DateTime? UpdatedAt { get; set; }

    [Browsable(false)]
    public bool DeletedFlag { get; set; } = false;

    [Browsable(false)]
    public bool EnableFlag { get; set; } = false;
  }
}
