using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Models
{
  public class DataReject
  {
    public DateTime DateTime { get; set; }
    public string FGs { get; set; }
    public double Actual { get; set; }
    public double Target { get; set; }
  }
}
