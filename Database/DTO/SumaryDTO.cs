using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DTO
{
  public class SumaryDTO
  {
    public int Sample { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double Cpk { get; set; }
    public double Cp { get; set; }
    public double Mean { get; set; }
    public double Stdev { get; set; }
    public double OW { get; set; }
  }
}
