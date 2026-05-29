using CheckWeigherFood.RJControl;
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
  public partial class UcData : UserControl
  {
    public UcData()
    {
      InitializeComponent();
      CustomUI();
    }

    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = this;
      elipseControl0.CornerRadius = 20;

      ElipseControl elipseControl1 = new ElipseControl();
      elipseControl1.TargetControl = tableLayoutPanel1;
      elipseControl1.CornerRadius = 20;

      ElipseControl elipseControl2 = new ElipseControl();
      elipseControl2.TargetControl = lbData;
      elipseControl2.CornerRadius = 20;
    }

    public string ValueTilte
    {
      set
      {
        lbTitle.Text = value;
      }
    }

    public string ValueData
    {
      set
      {
        lbData.Text = value;
      }
    }

    public Color SetColor
    {
      set
      {
        lbData.BackColor = value;
      }
    }
  }
}
