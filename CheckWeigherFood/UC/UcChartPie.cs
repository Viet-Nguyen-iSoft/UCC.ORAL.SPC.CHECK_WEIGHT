using CheckWeigherFood.RJControl;
using Database.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CheckWeigherFood.UC
{
  public partial class UcChartPie : UserControl
  {
    public UcChartPie()
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
      elipseControl1.TargetControl = tableLayoutPanel21;
      elipseControl1.CornerRadius = 20;
    }

    #region Chart Pie
    public void SetDataChartPie(SumaryDTO  sumaryDTO)
    {
      double accept = (double) sumaryDTO.DatalogAccept.Count();
      double over = (double) sumaryDTO.DatalogOver.Count();
      double reject = (double) sumaryDTO.DatalogReject.Count();
      double total = accept + over + reject;
      chartPie.Series[0].Points.Clear();
      try
      {
        if (accept == 0 && over == 0 && reject == 0)
        {
          chartPie.Series[0].Points.AddXY($"No Data", 100);
          chartPie.Series[0].Points[0].Color = Color.Gray;
          chartPie.Series[0].Points[0].LabelForeColor = Color.White;
          return;
        }
        if (accept > 0)
        {
          chartPie.Series[0].Points.AddXY($"{Math.Round(accept * 100 / total, 2)} %", accept);
          //nameChart.Series[0].Points.AddXY($"", valueOK);
        }
        if (over > 0)
        {
          chartPie.Series[0].Points.AddXY($"{Math.Round(over * 100 / total, 2)} %", over);
          //nameChart.Series[0].Points.AddXY($"", valueOver);
        }
        if (reject > 0)
        {
          chartPie.Series[0].Points.AddXY($"{Math.Round(reject * 100 / total, 2)} %", reject);
          //nameChart.Series[0].Points.AddXY($"", valueReject);
        }

      }
      catch (Exception)
      {
      }
      finally
      {
        if (accept > 0 && over > 0 && reject > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          chartPie.Series[0].Points[1].Color = Color.FromArgb(255, 128, 0); // Cam
          chartPie.Series[0].Points[2].Color = Color.Red; // ĐỎ
          chartPie.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[2].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
          chartPie.Series[0].Points[1].LabelForeColor = Color.Transparent;
          chartPie.Series[0].Points[2].LabelForeColor = Color.Transparent;
        }

        else if (accept > 0 && over > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          chartPie.Series[0].Points[1].Color = Color.FromArgb(255, 128, 0); // Cam

          chartPie.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
          chartPie.Series[0].Points[1].LabelForeColor = Color.Transparent;
        }
        else if (accept > 0 && reject > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          chartPie.Series[0].Points[1].Color = Color.Red; // ĐỎ

          chartPie.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
          chartPie.Series[0].Points[1].LabelForeColor = Color.Transparent;
        }
        else if (over > 0 && reject > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.FromArgb(255, 128, 0); // Cam
          chartPie.Series[0].Points[1].Color = Color.Red; // ĐỎ
          chartPie.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
          chartPie.Series[0].Points[1].LabelForeColor = Color.Transparent;
        }

        else if (accept > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
        }
        else if (over > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.FromArgb(255, 128, 0); // Cam
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
        }
        else if (reject > 0)
        {
          chartPie.Series[0].Points[0].Color = Color.Red; // ĐỎ
          chartPie.Series[0].Points[0].LabelForeColor = Color.Transparent;
        }


      }

    }
    #endregion
  }
}
