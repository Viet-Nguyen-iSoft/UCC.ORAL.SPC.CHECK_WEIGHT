using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
  public class Machine : BaseModel
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }


    public string IP { get; set; }
    public int? Port { get; set; }
    public int? Timeout { get; set; }
    public int? TimeCheckConnect { get; set; }
    public int? SampleTime { get; set; }


    #region Mapping
    public ICollection<Datalog> Datalogs { get; set; }
    #endregion
  }
}
