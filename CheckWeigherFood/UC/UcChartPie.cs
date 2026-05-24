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
    public void SetDataChartPie(Chart nameChart, double valueOK, double valueOver, double valueReject)
    {
      double total = valueOK + valueOver + valueReject;
      nameChart.Series[0].Points.Clear();
      try
      {
        if (valueOK == 0 && valueOver == 0 && valueReject == 0)
        {
          nameChart.Series[0].Points.AddXY($"No Data", 100);
          nameChart.Series[0].Points[0].Color = Color.Gray;
          nameChart.Series[0].Points[0].LabelForeColor = Color.White;
          return;
        }
        if (valueOK > 0)
        {
          nameChart.Series[0].Points.AddXY($"{Math.Round(valueOK * 100 / total, 2)} %", valueOK);
          //nameChart.Series[0].Points.AddXY($"", valueOK);
        }
        if (valueOver > 0)
        {
          nameChart.Series[0].Points.AddXY($"{Math.Round(valueOver * 100 / total, 2)} %", valueOver);
          //nameChart.Series[0].Points.AddXY($"", valueOver);
        }
        if (valueReject > 0)
        {
          nameChart.Series[0].Points.AddXY($"{Math.Round(valueReject * 100 / total, 2)} %", valueReject);
          //nameChart.Series[0].Points.AddXY($"", valueReject);
        }

      }
      catch (Exception)
      {
      }
      finally
      {
        if (valueOK > 0 && valueOver > 0 && valueReject > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          nameChart.Series[0].Points[1].Color = Color.FromArgb(255, 128, 0); // Cam
          nameChart.Series[0].Points[2].Color = Color.Red; // ĐỎ
          nameChart.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[2].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
          nameChart.Series[0].Points[1].LabelForeColor = Color.Transparent;
          nameChart.Series[0].Points[2].LabelForeColor = Color.Transparent;
        }

        else if (valueOK > 0 && valueOver > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          nameChart.Series[0].Points[1].Color = Color.FromArgb(255, 128, 0); // Cam

          nameChart.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
          nameChart.Series[0].Points[1].LabelForeColor = Color.Transparent;
        }
        else if (valueOK > 0 && valueReject > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          nameChart.Series[0].Points[1].Color = Color.Red; // ĐỎ

          nameChart.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
          nameChart.Series[0].Points[1].LabelForeColor = Color.Transparent;
        }
        else if (valueOver > 0 && valueReject > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.FromArgb(255, 128, 0); // Cam
          nameChart.Series[0].Points[1].Color = Color.Red; // ĐỎ
          nameChart.Series[0].Points[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[1].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
          nameChart.Series[0].Points[1].LabelForeColor = Color.Transparent;
        }

        else if (valueOK > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.FromArgb(0, 192, 0); //Xanh
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
        }
        else if (valueOver > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.FromArgb(255, 128, 0); // Cam
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
        }
        else if (valueReject > 0)
        {
          nameChart.Series[0].Points[0].Color = Color.Red; // ĐỎ
          nameChart.Series[0].Points[0].LabelForeColor = Color.Transparent;
        }


      }

    }
    #endregion
  }
}
