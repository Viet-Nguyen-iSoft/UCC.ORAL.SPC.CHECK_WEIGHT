using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Models
{
  public class Synthetic
  {
    public int STT { get; set; }
    public string DateTime { get; set; }
    public string Shift { get; set; }
    public string OP { get; set; }
    public string QC { get; set; }
    public string TC { get; set; }
    public string CodeFGs { get; set; }
    public string Description { get; set; }
    public string LoBB { get; set; }
    public double Gross { get; set; }
    public double Target { get; set; }

    public string Status { get; set; }

  }
}
