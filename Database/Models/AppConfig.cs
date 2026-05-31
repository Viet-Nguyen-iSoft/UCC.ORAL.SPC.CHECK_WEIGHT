using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
  public class AppConfig : BaseModel
  {
    public string PathReport { get; set; }
    public long ProductId { get; set; }
    public long MachineId { get; set; }
    public long ChangeOverId { get; set; }
  }
}
