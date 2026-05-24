using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Models
{
  public class ListDataLogs
  {
    public int STT { get; set; }
    public DateTime DateTime { get; set; }
    public string Shift { get; set; }
    [DisplayName("OP Name")]
    public string OP { get; set; }
    [DisplayName("QC Name")]
    public string QC { get; set; }
    [DisplayName("TC Name")]
    public string TC { get; set; }

    [DisplayName("FGs")]
    public string CodeFGs { get; set; }
    public string Description { get; set; }
    [DisplayName("Lô Bao Bì")]
    public string LoBB { get; set; }
    public double Net { get; set; }
    public double Target { get; set; }
    //public double OW { get; set; }
    //public double Cpk { get; set; }
    //public double Cp { get; set; }
    //public int In { get; set; }
    //public int Out { get; set; }
    public int Over { get; set; }
    public int Reject { get; set; }
  }
}
