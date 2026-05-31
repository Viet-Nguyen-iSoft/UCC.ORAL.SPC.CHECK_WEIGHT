using Database.Models;
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
    public double Target { get; set; }
    public double USL { get; set; }
    public double LSL { get; set; }
    public double UCL { get; set; }
    public double LCL { get; set; }
    public EnumResult EnumResult { get; set; } = EnumResult.None;
    public List<string> ReasonFail { get; set; } = new List<string>();

    public List<Datalog> DatalogPass { get; set; } = new List<Datalog>();
    public List<Datalog> DatalogAccept { get; set; } = new List<Datalog>();
    public List<Datalog> DatalogOver { get; set; } = new List<Datalog>();
    public List<Datalog> DatalogReject { get; set; } = new List<Datalog>();
  }

  public enum EnumResult
  {
    None,
    Pass,
    Fail,
  }
}
