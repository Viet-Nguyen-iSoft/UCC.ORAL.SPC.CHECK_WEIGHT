using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Database.Models
{
  public class AppConfig: BaseModel
  {
    public string? PathReport { get; set; }
  }
}
