using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
  public class OperationSetting: BaseModel
  {
    public string OP { get; set; }
    public string QC { get; set; }
    public string ShiftLeader { get; set; }

    #region Mapping

    #endregion
  }
}
