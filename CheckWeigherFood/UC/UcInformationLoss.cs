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
  public partial class UcInformationLoss : UserControl
  {
    public UcInformationLoss()
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
      elipseControl2.TargetControl = lbReject;
      elipseControl2.CornerRadius = 20;

      ElipseControl elipseControl3 = new ElipseControl();
      elipseControl3.TargetControl = lbLoss;
      elipseControl3.CornerRadius = 20;
    }
  }
}
