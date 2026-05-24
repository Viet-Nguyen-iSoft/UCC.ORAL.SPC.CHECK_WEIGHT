using CheckWeigherFood.Models;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace CheckWeigherFood.InitChart
{
  public class DataChart
  {
    public void ChartControlInit(Chart nameChart)
    {
      //nameChart.Series[0].Color = Color.Blue;
      //nameChart.Series[1].Color = Color.Red;
      //nameChart.Series[2].Color = Color.Orange;
      //nameChart.Series[3].Color = Color.Green;
      //nameChart.Series[4].Color = Color.Orange;
      //nameChart.Series[5].Color = Color.Red;

      nameChart.Series[0].Color = Color.FromArgb(51, 204, 255);
      //nameChart.Series[0].Color = Color.White;
      nameChart.Series[1].Color = Color.Red;
      nameChart.Series[2].Color = Color.FromArgb(255, 204, 51);
      nameChart.Series[3].Color = Color.FromArgb(0, 255, 51);
      nameChart.Series[4].Color = Color.FromArgb(255, 204, 51);
      nameChart.Series[5].Color = Color.Red;

      nameChart.Series[0].BorderWidth = 3;
      nameChart.Series[1].BorderWidth = 3;
      nameChart.Series[2].BorderWidth = 3;
      nameChart.Series[3].BorderWidth = 3;
      nameChart.Series[4].BorderWidth = 3;
      nameChart.Series[5].BorderWidth = 3;

      nameChart.ChartAreas[0].AxisX.Title = "Thời gian";
      nameChart.ChartAreas[0].AxisY.Title = "Giá trị cân (g)";
    }

    public void ChartHistogramInit(Chart nameChart)
    {
      //nameChart.Series[0].Color = Color.FromArgb(0, 205, 102);//51, 204, 255
      nameChart.Series[0].Color = Color.FromArgb(51, 204, 255);// Màu cột


      nameChart.Series[1].Color = Color.Red;
      nameChart.Series[2].Color = Color.FromArgb(255, 165, 0);
      nameChart.Series[3].Color = Color.FromArgb(0, 0, 255);
      nameChart.Series[4].Color = Color.FromArgb(255, 165, 0);
      nameChart.Series[5].Color = Color.Red;
      nameChart.Series[6].Color = Color.FromArgb(255, 51, 255);
      nameChart.Series[3].Color = Color.FromArgb(0, 0, 221);

      nameChart.Series[7].Color = Color.FromArgb(0, 205, 102);


      nameChart.Series[1].BorderWidth = 3;
      nameChart.Series[2].BorderWidth = 3;
      nameChart.Series[3].BorderWidth = 3;
      nameChart.Series[4].BorderWidth = 3;
      nameChart.Series[5].BorderWidth = 3;
      nameChart.Series[6].BorderWidth = 3;

      nameChart.Series[7].BorderWidth = 3;
    }

    public void ChartPieInit(Chart nameChart)
    {
      nameChart.Series[0].Points.AddXY($"No Data", 100);
      nameChart.Series[0].Points[0].Color = Color.Gray;
      nameChart.Series[0].Points[0].LabelForeColor = Color.White;
    }


    #region Chart Control
    public void AddChartControl(Chart chartName, List<double> dataY, List<string> dataX, double up2T, double up1T, double target, double lo1T, double lo2T, double max)
    {
      try
      {
        if (dataX == null || dataY == null || dataX.Count == 0 || dataY.Count == 0)
        {
          chartName.Series[0].Points.Clear();
          chartName.Series[1].Points.Clear();
          chartName.Series[2].Points.Clear();
          chartName.Series[3].Points.Clear();
          chartName.Series[4].Points.Clear();
          chartName.Series[5].Points.Clear();
          return;
        }

        //if (dataX.Count>500)
        //{
        //  //dataY = dataX.
        //  dataX = dataX.Skip(Math.Max(0, dataX.Count - 500)).ToList();
        //  dataY = dataY.Skip(Math.Max(0, dataY.Count - 500)).ToList();
        //}



        chartName.ChartAreas[0].AxisY.Maximum = (max < up2T) ? up2T + 5 : up2T * 1.01;
        chartName.ChartAreas[0].AxisY.Minimum = lo2T - 5;

        chartName.Series[0].Points.Clear();
        chartName.Series[1].Points.Clear();
        chartName.Series[2].Points.Clear();
        chartName.Series[3].Points.Clear();
        chartName.Series[4].Points.Clear();
        chartName.Series[5].Points.Clear();

        for (int i = 0; i < dataX.Count(); i++)
        {
          int indexOfFirstSpace = dataX[i].IndexOf(' ');
          string timeOnly = dataX[i].Substring(indexOfFirstSpace + 1);

          //string timeOnly = DateTime.ParseExact(dataX[i], "yyyy/MM/dd HH:mm:ss", null).ToString("HH:mm:ss");
          //string dateString = dataX[i];

          // Chuyển đổi chuỗi thành đối tượng DateTime
          //DateTime dateTime = DateTime.ParseExact(dateString, "dd/MM/yyyy HH:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
          //string timeOnly = dateTime.ToString("HH:mm:ss tt");

          //string timeOnly = dataX[i].Substring(11, 8);
          chartName.Series[0].Points.AddXY(timeOnly, dataY[i]);

          chartName.Series[1].Points.AddXY(dataX[i], up2T);
          chartName.Series[2].Points.AddXY(dataX[i], up1T);
          chartName.Series[3].Points.AddXY(dataX[i], target);
          chartName.Series[4].Points.AddXY(dataX[i], lo1T);
          chartName.Series[5].Points.AddXY(dataX[i], lo2T);
        }
        chartName.Invalidate();
      }
      catch (Exception)
      {
      }

    }

    public void AddChartControlDashboard(Chart chartName, List<double> dataY, List<string> dataX, double up2T, double up1T, double target, double lo1T, double lo2T, double max)
    {
      try
      {
        if (dataX == null || dataY == null || dataX.Count == 0 || dataY.Count == 0)
        {
          chartName.Series[0].Points.Clear();
          chartName.Series[1].Points.Clear();
          chartName.Series[2].Points.Clear();
          chartName.Series[3].Points.Clear();
          chartName.Series[4].Points.Clear();
          chartName.Series[5].Points.Clear();
          return;
        }


        chartName.ChartAreas[0].AxisY.Maximum = (max < up2T) ? up2T + 5 : up2T * 1.01;
        chartName.ChartAreas[0].AxisY.Minimum = lo2T - 5;

        chartName.Series[0].Points.Clear();
        chartName.Series[1].Points.Clear();
        chartName.Series[2].Points.Clear();
        chartName.Series[3].Points.Clear();
        chartName.Series[4].Points.Clear();
        chartName.Series[5].Points.Clear();
        chartName.ChartAreas[0].AxisX.CustomLabels.Clear();

        for (int i = 0; i < dataX.Count(); i++)
        {
          chartName.Series[0].Points.AddXY(i, dataY[i]);
        }


        chartName.Series[1].Points.AddXY(0, up2T);
        chartName.Series[1].Points.AddXY(dataX.Count() - 1, up2T);

        chartName.Series[2].Points.AddXY(0, up1T);
        chartName.Series[2].Points.AddXY(dataX.Count() - 1, up1T);

        chartName.Series[3].Points.AddXY(0, target);
        chartName.Series[3].Points.AddXY(dataX.Count() - 1, target);

        chartName.Series[4].Points.AddXY(0, lo1T);
        chartName.Series[4].Points.AddXY(dataX.Count() - 1, lo1T);

        chartName.Series[5].Points.AddXY(0, lo2T);
        chartName.Series[5].Points.AddXY(dataX.Count() - 1, lo2T);

        List<int> selectedPoints = GetEquallySpacedPoints(chartName.Series[0].Points.Count(), 10);
        chartName.ChartAreas[0].AxisX.MajorGrid.Enabled = false;


        int width = (int)(dataY.Count / 10);
        width = (int)(width * 0.7);
        for (int i = 0; i < selectedPoints.Count; i++)
        {
          int indexOfFirstSpace = dataX[selectedPoints[i]].IndexOf(' ');
          string timeOnly = dataX[selectedPoints[i]].Substring(indexOfFirstSpace + 1);
          chartName.ChartAreas[0].AxisX.CustomLabels.Add(selectedPoints[i], selectedPoints[i] + width, timeOnly);
        }

        chartName.Invalidate();
      }
      catch (Exception)
      {
      }







      //try
      //{
      //  if (dataX == null || dataY == null || dataX.Count == 0 || dataY.Count == 0)
      //  {
      //    chartName.Series[0].Points.Clear();
      //    chartName.Series[1].Points.Clear();
      //    chartName.Series[2].Points.Clear();
      //    chartName.Series[3].Points.Clear();
      //    chartName.Series[4].Points.Clear();
      //    chartName.Series[5].Points.Clear();
      //    return;
      //  }

      //  if (dataX.Count > 1000)
      //  {
      //    dataX = dataX.Skip(Math.Max(0, dataX.Count - 500)).ToList();
      //    dataY = dataY.Skip(Math.Max(0, dataY.Count - 500)).ToList();
      //  }



      //  chartName.ChartAreas[0].AxisY.Maximum = (max < up2T) ? up2T + 5 : up2T * 1.01;
      //  chartName.ChartAreas[0].AxisY.Minimum = lo2T - 5;

      //  chartName.Series[0].Points.Clear();
      //  chartName.Series[1].Points.Clear();
      //  chartName.Series[2].Points.Clear();
      //  chartName.Series[3].Points.Clear();
      //  chartName.Series[4].Points.Clear();
      //  chartName.Series[5].Points.Clear();

      //  for (int i = 0; i < dataX.Count(); i++)
      //  {
      //    int indexOfFirstSpace = dataX[i].IndexOf(' ');
      //    string timeOnly = dataX[i].Substring(indexOfFirstSpace + 1);

      //    chartName.Series[0].Points.AddXY(timeOnly, dataY[i]);

      //    chartName.Series[1].Points.AddXY(dataX[i], up2T);
      //    chartName.Series[2].Points.AddXY(dataX[i], up1T);
      //    chartName.Series[3].Points.AddXY(dataX[i], target);
      //    chartName.Series[4].Points.AddXY(dataX[i], lo1T);
      //    chartName.Series[5].Points.AddXY(dataX[i], lo2T);
      //  }
      //  chartName.Invalidate();
      //}
      //catch (Exception ex)
      //{
      //  Console.WriteLine(ex.Message);
      //}

    }
    private List<int> GetEquallySpacedPoints(int Total, int numberOfPointsToSelect)
    {
      List<int> selectedPoints = new List<int>();

      double step = (double)Total / numberOfPointsToSelect;

      for (int i = 0; i < numberOfPointsToSelect; i++)
      {
        int index = (int)Math.Round(i * step);
        selectedPoints.Add(index);
      }
      return selectedPoints;
    }
    #endregion


    #region Chart Histogram
    public void AddChartHistogram(Chart nameChart, List<double> dataWeigher, double up2T, double up1T, double mean, double std, double lo1T, double lo2T, double min, double max, double target)
    {
      try
      {
        if (dataWeigher == null)
        {
          nameChart.Series[0].Points.Clear();
          nameChart.Series[1].Points.Clear();
          nameChart.Series[2].Points.Clear();
          nameChart.Series[3].Points.Clear();
          nameChart.Series[4].Points.Clear();
          nameChart.Series[5].Points.Clear();
          nameChart.Series[6].Points.Clear();
          nameChart.Series[7].Points.Clear();
          return;
        }
        List<double> histogramValueNet = dataWeigher.OrderBy(x => x).ToList();
        histogramValueNet = histogramValueNet.Where(x => x < (up2T * 1.01)).ToList();
        max = (max > up2T) ? (up2T * 1.01) : max;

        double DonViNhoNhatChoPhep = 0.1;
        int numbersCol = (int)Math.Round(Math.Sqrt(histogramValueNet.Count), MidpointRounding.AwayFromZero);

        double widthCol = (max - min) / numbersCol;
        double valueStart = min - 0.5 * DonViNhoNhatChoPhep;

        double[] arrayDataStartCol = new double[numbersCol + 1];
        double[] arrayDataCenterCol = new double[numbersCol + 1];
        int[] arrayFrequency = new int[numbersCol];

        arrayDataStartCol[0] = valueStart;
        for (int i = 1; i <= numbersCol; i++)
        {
          arrayDataStartCol[i] = valueStart + widthCol * i;
          arrayDataCenterCol[i] = (arrayDataStartCol[i - 1] + arrayDataStartCol[i]) / 2;
        }

        for (int i = 0; i < numbersCol; i++)
        {
          arrayFrequency[i] = histogramValueNet.Where(x => x >= arrayDataStartCol[i] && x < arrayDataStartCol[i + 1]).Count();
        }

        double[] arrayNormalDensityProbabilityFunction = new double[histogramValueNet.Count];
        double[] arrayZ = new double[histogramValueNet.Count];

        for (int i = 0; i < histogramValueNet.Count; i++)
        {
          arrayNormalDensityProbabilityFunction[i] = CalNormalDensityProbabilityFunction(histogramValueNet[i], mean, std);
          arrayZ[i] = (histogramValueNet[i] - mean) / std;
        }

        int sum = 0;
        foreach (var item in arrayFrequency)
        {
          sum += item;
        }


        nameChart.Series[0].Points.Clear();
        nameChart.Series[1].Points.Clear();
        nameChart.Series[2].Points.Clear();
        nameChart.Series[3].Points.Clear();
        nameChart.Series[4].Points.Clear();
        nameChart.Series[5].Points.Clear();
        nameChart.Series[6].Points.Clear();
        nameChart.Series[7].Points.Clear();


        //Col
        for (int i = 1; i <= numbersCol; i++)
        {
          nameChart.Series[0].Points.AddXY(Math.Round(arrayDataCenterCol[i], 2), arrayFrequency[i - 1]);
        }


        // Lower2T
        nameChart.Series[1].Points.AddXY(lo2T, 0);
        nameChart.Series[1].Points.AddXY(lo2T, arrayFrequency.Max() * 1.1);

        // Lower1T
        nameChart.Series[2].Points.AddXY(lo1T, 0);
        nameChart.Series[2].Points.AddXY(lo1T, arrayFrequency.Max() * 1.1);

        // Méan
        nameChart.Series[3].Points.AddXY(mean, 0);
        nameChart.Series[3].Points.AddXY(mean, arrayFrequency.Max() * 1.1);

        // Lower1T
        nameChart.Series[4].Points.AddXY(up1T, 0);
        nameChart.Series[4].Points.AddXY(up1T, arrayFrequency.Max() * 1.1);

        // Lower1T
        nameChart.Series[5].Points.AddXY(up2T, 0);
        nameChart.Series[5].Points.AddXY(up2T, arrayFrequency.Max() * 1.1);

        // Target
        nameChart.Series[7].Points.AddXY(target, 0);
        nameChart.Series[7].Points.AddXY(target, arrayFrequency.Max() * 1.2);

        // Vẽ Chart Bell
        List<double> arrayBell_X = new List<double>();
        List<double> arrayBell_Y = new List<double>();

        double topBell = (std != 0) ? (1 / (2 * Math.PI)) * Math.Exp(-Math.Pow(mean - mean, 2) / (2 * std)) : 0;
        double YY = (topBell != 0) ? Math.Round(arrayFrequency.Max() / topBell, 2) : 0;

        for (double i = histogramValueNet.Min(); i <= histogramValueNet.Max(); i += 0.1)
        {
          double y = (std != 0) ? (1 / (2 * Math.PI)) * Math.Exp(-Math.Pow(i - mean, 2) / (2 * std)) : 0;
          arrayBell_Y.Add(Math.Round(y * YY, 2));
          arrayBell_X.Add(i);
        }

        for (int i = 0; i < arrayBell_X.Count; i++)
        {
          nameChart.Series[6].Points.AddXY(arrayBell_X[i], arrayBell_Y[i]);
        }


        //nameChart.ChartAreas[0].AxisX.Minimum = min - 5;
        nameChart.ChartAreas[0].AxisX.Minimum = (min < lo2T) ? min - 5 : lo2T - 5;
        nameChart.ChartAreas[0].AxisX.Maximum = max + 5;
      }
      catch (Exception)
      {
      }

    }
    #endregion

    private double CalNormalDensityProbabilityFunction(double x_value, double mean, double stdDev)
    {
      double ret = 0;
      ret = (stdDev != 0) ? (1.0 / (stdDev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-(Math.Pow(x_value - mean, 2) / (2 * Math.Pow(stdDev, 2)))) : 0;
      return Math.Round(ret, 2);
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
