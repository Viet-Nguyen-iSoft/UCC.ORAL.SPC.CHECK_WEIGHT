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
    public event Action<TimeSpan, TimeSpan> OnSendChangTime;
    public UcFilterTime()
    {
      InitializeComponent();
      this.hourStart.ValueChanged += Time_ValueChanged;
      this.minuteStart.ValueChanged += Time_ValueChanged;
    }

    private void Time_ValueChanged(object sender, EventArgs e)
    {
      OnSendChangTime?.Invoke(From, To);
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

    public void RangeFrom(int min, int max)
    {
      hourStart.Minimum = min;
      hourStart.Maximum = max;
    }

    public void RangeTo(int min, int max)
    {
      hourEnd.Minimum = min;
      hourEnd.Maximum = max;
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
