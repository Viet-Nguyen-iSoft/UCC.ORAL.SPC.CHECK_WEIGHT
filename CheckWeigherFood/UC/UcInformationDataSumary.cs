using CheckWeigherFood.InitChart;
using Database.Models;
using DocumentFormat.OpenXml.Math;
using Irony.Parsing;
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
  public partial class UcInformationDataSumary : UserControl
  {
    public UcInformationDataSumary()
    {
      InitializeComponent();
    }


    private void ResetDashBoard()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetDashBoard();
        }));
        return;
      }

      this.lbSample.Text = "0";
      this.lbXtb.Text = "0";
      this.lbMin.Text = "0";
      this.lbMax.Text = "0";
      this.lbCp.Text = "0";
      this.lbCpk.Text = "0";
    }

    public void SetInforProduct(Product product)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetDashBoard();
        }));
        return;
      }

      this.lbT.Text = $"{product.T}";
      this.lbUpper.Text = $"{product.USL}";
      this.lbUpperControl.Text = $"{product.UCL}";
      this.lbTarget.Text = $"{product.Target}";
      this.lbLowerControl.Text = $"{product.LCL}";
      this.lbLower.Text = $"{product.LSL}";
    }
  }
}
