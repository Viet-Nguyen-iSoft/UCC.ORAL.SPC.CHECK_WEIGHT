using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Models
{
  public class Datalog:BaseModel
  {
    public ulong STT { get; set; }
    public int ProductId { get; set; }
    public int ShiftId { get;set; }
    public double Gross { get; set; }
    public double Net { get; set; }
    public string OP { get; set; }
    public string QC { get; set; }
    public string TC { get; set; }
    public string LoBB { get; set; }
    public string Status { get; set; }
  }
}
