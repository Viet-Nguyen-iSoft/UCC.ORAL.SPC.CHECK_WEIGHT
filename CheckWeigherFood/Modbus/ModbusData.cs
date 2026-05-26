using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Modbus
{
  public class ModbusData
  {
    public class ModbusDataEventArgs : EventArgs
    {
      public ushort[] Registers { get; set; }
    }
  }
}
