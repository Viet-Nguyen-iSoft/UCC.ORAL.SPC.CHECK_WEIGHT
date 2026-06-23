using CheckWeigherFood.RJControl;
using CheckWeigherFood.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.FormUI
{
  public partial class UcOverview : UserControl
  {
    public UcOverview()
    {
      InitializeComponent();
      CustomUI();
    }

    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = ucInformationDataSumary1;
      elipseControl0.CornerRadius = 20;

      ElipseControl elipseControl1 = new ElipseControl();
      elipseControl1.TargetControl = tableLayoutPanel23;
      elipseControl1.CornerRadius = 20;

      ElipseControl elipseControl2 = new ElipseControl();
      elipseControl2.TargetControl = tableLayoutPanel1;
      elipseControl2.CornerRadius = 20;

      ElipseControl elipseControl3 = new ElipseControl();
      elipseControl3.TargetControl = tableLayoutPanel3;
      elipseControl3.CornerRadius = 20;

      ElipseControl elipseControl4 = new ElipseControl();
      elipseControl4.TargetControl = tableLayoutPanel5;
      elipseControl4.CornerRadius = 20;

      ElipseControl elipseControl5 = new ElipseControl();
      elipseControl5.TargetControl = tableLayoutPanel10;
      elipseControl5.CornerRadius = 20;

      ElipseControl elipseControl6 = new ElipseControl();
      elipseControl6.TargetControl = tableLayoutPanel24;
      elipseControl6.CornerRadius = 20;

      //ElipseControl elipseControl7 = new ElipseControl();
      //elipseControl7.TargetControl = tableLayoutPanel14;
      //elipseControl7.CornerRadius = 20;

      //ElipseControl elipseControl8 = new ElipseControl();
      //elipseControl8.TargetControl = panelContent;
      //elipseControl8.CornerRadius = 20;

      lbOverWeight.ValueTilte = "OW (%)";
      lbTLTB.ValueTilte = "TL trung bình (g)";

      lbOP.SetBackColor = Color.White;
      lbQC.SetBackColor = Color.White;
      lbShiftLeader.SetBackColor = Color.White;
      lbTailTube.SetBackColor = Color.White;
      lbTube.SetBackColor = Color.White;
      lbCarton.SetBackColor = Color.White;
      lbLotTube.SetBackColor = Color.White;
      lbFGs.SetBackColor = Color.White;
      lbNameProduct.SetBackColor = Color.White;
      lbLotCarton.SetBackColor = Color.White;

      lbOP.SetForeColor = Color.Black;
      lbQC.SetForeColor = Color.Black;
      lbShiftLeader.SetForeColor = Color.Black;
      lbTailTube.SetForeColor = Color.Black;
      lbTube.SetForeColor = Color.Black;
      lbCarton.SetForeColor = Color.Black;
      lbLotTube.SetForeColor = Color.Black;
      lbFGs.SetForeColor = Color.Black;
      lbNameProduct.SetForeColor = Color.Black;
      lbLotCarton.SetForeColor = Color.Black;
    }
  }
}
