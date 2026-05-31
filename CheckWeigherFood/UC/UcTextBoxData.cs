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
  public partial class UcTextBoxData : UserControl
  {
    public UcTextBoxData()
    {
      InitializeComponent();
      CustomUI();
    }

    public string ValueStr
    {
      set
      {
        lbData.Text = value;
      }
      get
      {
        return lbData.Text;
      }
    }
    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = this;
      elipseControl0.CornerRadius = 20;

      ElipseControl elipseControl1 = new ElipseControl();
      elipseControl1.TargetControl = lbData;
      elipseControl1.CornerRadius = 20;
    }

    public void SetLabelAlign(ContentAlignment alignment)
    {
      if (lbData == null)
        return;

      lbData.TextAlign = alignment;
    }
  }
}
