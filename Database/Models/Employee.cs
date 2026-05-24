using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.Models
{
  public class Employee: BaseModel
  {
    public string FullName { get; set; }
    public string Code { get; set; }
    public EnumTypeEmployee EnumTypeEmployee { get; set; }


    #region Mapping
    public ICollection<Datalog> DatalogOP { get; set; } = new List<Datalog>();
    public ICollection<Datalog> DatalogQC { get; set; } = new List<Datalog>();
    public ICollection<Datalog> DatalogSL { get; set; } = new List<Datalog>();

    #endregion
  }
}
