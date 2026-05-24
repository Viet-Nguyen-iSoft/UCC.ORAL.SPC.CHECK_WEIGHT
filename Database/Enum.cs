using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
  public class Enum
  {
    public enum EnumTypeEmployee
    {
      [Description("N/A")]
      None = 0,

      [Description("Vận hành máy")]
      OP = 1,

      [Description("Chất lượng")]
      QC = 2,

      [Description("Trưởng ca")]
      ShiftLeader = 3,

    }
  }
}
