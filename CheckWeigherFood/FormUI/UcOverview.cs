using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.RJControl;
using CheckWeigherFood.UC;
using Database.DTO;
using Database.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Database.Enum;
using Color = System.Drawing.Color;

namespace CheckWeigherFood.FormUI
{
  public partial class UcOverview : UserControl
  {
    private long KeyMachine {  get; set; }
    public event Action<long> OnSendChangeProduct;
    public UcOverview()
    {
      InitializeComponent();
      CustomUI();
    }

    private DataChart _dataChart = new DataChart();
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

      ElipseControl elipseControl4 = new ElipseControl();
      elipseControl4.TargetControl = tableLayoutPanel5;
      elipseControl4.CornerRadius = 20;

      //ElipseControl elipseControl5 = new ElipseControl();
      //elipseControl5.TargetControl = tableLayoutPanel10;
      //elipseControl5.CornerRadius = 20;

      ElipseControl elipseControl6 = new ElipseControl();
      elipseControl6.TargetControl = tableLayoutPanel24;
      elipseControl6.CornerRadius = 20;

      ElipseControl elipseControl7 = new ElipseControl();
      elipseControl7.TargetControl = tableLayoutPanel7;
      elipseControl7.CornerRadius = 20;

      //ElipseControl elipseControl8 = new ElipseControl();
      //elipseControl8.TargetControl = panelContent;
      //elipseControl8.CornerRadius = 20;

      lbOverWeight.ValueTilte = "OW (%)";
      lbTLTB.ValueTilte = "TL trung bình (g)";

      lbTailTube.SetBackColor = Color.White;
      lbTube.SetBackColor = Color.White;
      lbCarton.SetBackColor = Color.White;
      lbLotTube.SetBackColor = Color.White;
      lbFGs.SetBackColor = Color.White;
      lbNameProduct.SetBackColor = Color.White;
      lbLotCarton.SetBackColor = Color.White;

      lbTailTube.SetForeColor = Color.Black;
      lbTube.SetForeColor = Color.Black;
      lbCarton.SetForeColor = Color.Black;
      lbLotTube.SetForeColor = Color.Black;
      lbFGs.SetForeColor = Color.Black;
      lbNameProduct.SetForeColor = Color.Black;
      lbLotCarton.SetForeColor = Color.Black;
    }


    public void InitChart()
    {
      //Init chart
      _dataChart.ChartControlInit(chartControl);
    }


    public void SetKeyMachine(long keyMachine)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { SetKeyMachine(keyMachine); }));
        return;
      }

      KeyMachine = keyMachine;

      lbLine.Text = keyMachine.ToString("D2");
    }

    private void btnChangeOver_Click(object sender, EventArgs e)
    {
      OnSendChangeProduct?.Invoke(KeyMachine);
    }

    public void ShowInforProduct(Product product, TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforProduct(product, tareSetting); }));
        return;
      }

      lbFGs.ValueStr = product?.Code ?? string.Empty;
      lbNameProduct.ValueStr = product?.Description ?? string.Empty;
      ucInformationDataSumary1.SetInforProduct(product, tareSetting?.Tube ?? 0.0, tareSetting?.TailTube ?? 0.0, tareSetting?.Carton ?? 0.0);

      lbCarton.ValueStr = tareSetting?.Carton.ToString()??string.Empty;
      lbTailTube.ValueStr = tareSetting?.TailTube.ToString()??string.Empty;
      lbTube.ValueStr = tareSetting?.Tube.ToString()??string.Empty;

      lbLotTube.ValueStr = tareSetting?.LotTube??string.Empty;
      lbLotCarton.ValueStr = tareSetting?.LotCarton??string.Empty;

    }

    public void ChartLine(SumaryDTO sumaryDTO, List<Datalog> dataChartline)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ChartLine(sumaryDTO, dataChartline); }));
        return;
      }

      _dataChart.AddChartControlDashboard(chartControl, sumaryDTO, dataChartline, 0);
    }


    public (DateTime From, DateTime To) GetDt()
    {
      TimeSpan timeSpanFrom = ucFilterTime1.From;
      TimeSpan timeSpanTo = ucFilterTime1.To;

      DateTime from = DateTime.Today.Add(timeSpanFrom);
      DateTime to = DateTime.Today.Add(timeSpanTo);

      if (timeSpanTo < timeSpanFrom)
      {
        DateTime now = DateTime.Now;

        if (now.Hour >= 0 && now.Hour <= 6)
        {
          from = from.AddDays(-1);
        }
        else
        {
          to = to.AddDays(1);
        }
      }

      return (from, to);
    }

    public void SetTimeFilterChart(int shift)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { SetTimeFilterChart(shift); }));
        return;
      }

      if (shift == 1)
      {
        ucFilterTime1.From = new TimeSpan(6, 0, 0);
        ucFilterTime1.To = new TimeSpan(14, 0, 0);
      }
      else if (shift == 2)
      {
        ucFilterTime1.From = new TimeSpan(14, 0, 0);
        ucFilterTime1.To = new TimeSpan(22, 0, 0);
      }
      else if (shift == 3)
      {
        ucFilterTime1.From = new TimeSpan(22, 0, 0);
        ucFilterTime1.To = new TimeSpan(6, 0, 0);
      }
    }

    public void SetDataOW_Mean(SumaryDTO sumaryDTO)
    {
      try
      {
        if (this.InvokeRequired)
        {
          this.Invoke(new Action(() =>
          {
            SetDataOW_Mean(sumaryDTO);
          }));
          return;
        }

        lbOverWeight.ValueData = sumaryDTO.OW.ToString();
        lbTLTB.ValueData = sumaryDTO.Mean.ToString();

        lbOverWeight.SetColor = (sumaryDTO.OW > 0.5) ? Color.Tomato : Color.DarkGreen;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    public void SetSumaryDTO(SumaryDTO sumaryDTO)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetSumaryDTO(sumaryDTO);
        }));
        return;
      }

      ucInformationDataSumary1.SetSumaryDTO(sumaryDTO);
    }

    public void SetValueWeightRealtime(double value)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetValueWeightRealtime(value);
        }));
        return;
      }

      ucInformationDataSumary1.SetValueWeightRealtime(value);
    }

    public void SetStatusMachine(EnumStatusMachine enumStatusMachine)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetStatusMachine(enumStatusMachine);
        }));
        return;
      }

      if (enumStatusMachine == EnumStatusMachine.Run)
      {
        lbStatusMachine.Text = "MÁY CHẠY";
        lbStatusMachine.ForeColor = Color.LightGreen;
      }
      else if (enumStatusMachine == EnumStatusMachine.Stop)
      {
        lbStatusMachine.Text = "MÁY DỪNG";
        lbStatusMachine.ForeColor = Color.Tomato;
      }
      else if (enumStatusMachine == EnumStatusMachine.Disconnect)
      {
        lbStatusMachine.Text = "MẤT KẾT NỐI";
        lbStatusMachine.ForeColor = Color.Gray;
      }
    }

    private void btnSettingTareAndLot_Click(object sender, EventArgs e)
    {

    }

    private void btnChangeLot_Click(object sender, EventArgs e)
    {

    }
  }
}
