using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
  public class Product : BaseModel
  {
    public string ProName { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string WeightOnPack { get; set; }
    public string Unit { get; set; }
    public double? USL { get; set; }
    public double? UCL { get; set; }
    public double? Target { get; set; }
    public double? LCL { get; set; }
    public double? LSL { get; set; }
    public double? Density { get; set; }
    public double? T { get; set; }
    public double? Tare { get; set; }
    public string Note { get; set; }


    [NotMapped]
    public string DisplayText => $"{Code} - {Description}";

    #region Mapping
    public ICollection<Datalog> Datalogs { get; set; } = new List<Datalog>();
    #endregion

  }
}
