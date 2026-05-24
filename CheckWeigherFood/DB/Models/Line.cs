using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Models
{
  public class Line:BaseModel
  {
    public string NameLine { get; set; }
    public string PathReport { get; set; }
    public string PathDataBase { get; set; }
    public string IpPLC { get; set; }
    public int Port { get; set; }
    public bool IsEnable { get; set; }
  }
}
