using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.UC
{
  public partial class UcFilterTime : UserControl
  {
    public UcFilterTime()
    {
      InitializeComponent();
    }

    public TimeSpan From
    {
      set
      {
        hourStart.Value = value.Hours;
        minuteStart.Value = value.Minutes;
      }
      get
      {
        return new TimeSpan((int)hourStart.Value, (int)minuteStart.Value, 0);
      }
    }

    public TimeSpan To
    {
      set
      {
        hourEnd.Value = value.Hours;
        minuteEnd.Value = value.Minutes;
      }
      get
      {
        return new TimeSpan((int)hourEnd.Value, (int)minuteEnd.Value, 0);
      }
    }
  }
}
