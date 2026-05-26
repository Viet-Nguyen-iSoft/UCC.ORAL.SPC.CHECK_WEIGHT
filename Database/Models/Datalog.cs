using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.Models
{
  public class Datalog : BaseModel
  {
    public double Net { get; set; }
    public double TareTube { get; set; }
    public double TareCarton { get; set; }

    public EnumStatusRecord EnumStatusRecord { get; set; }

    public string NameEmployeeOP { get; set; }
    public string NameEmployeeQC { get; set; }
    public string NameEmployeeShiftLeader { get; set; }

    #region Mapping
    [ForeignKey(nameof(EmployeeOPId))]
    public long? EmployeeOPId { get; set; }
    public Employee EmployeeOP { get; set; }


    [ForeignKey(nameof(EmployeeQCId))]
    public long? EmployeeQCId { get; set; }
    public Employee EmployeeQC { get; set; }


    [ForeignKey(nameof(EmployeeSLId))]
    public long? EmployeeSLId { get; set; }
    public Employee EmployeeSL { get; set; }


    public long? MachineId { get; set; }
    public Machine Machine { get; set; }

    public long? ProductId { get; set; }
    public Product Product { get; set; }

    #endregion
  }
}
