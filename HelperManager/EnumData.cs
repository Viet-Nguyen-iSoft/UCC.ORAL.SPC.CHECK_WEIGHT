using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperManager
{
  public class EnumData
  {
    public enum EnumCheckData
    {
      None,
      Check,
      UnCheck,
    }

    public enum EnumGroup
    {
      None,
      Warehouse,
      QC,
      Other
    }

    public enum EnumTypePO
    {
      None,
      POandOther,
      TestMachine,
      RequestOther
    }
  }
}
