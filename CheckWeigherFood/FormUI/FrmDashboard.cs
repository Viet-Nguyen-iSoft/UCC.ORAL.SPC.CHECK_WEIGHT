using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.Popup;
using CheckWeigherFood.RJControl;
using Database.DTO;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Database.Enum;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmDashboard : Form
  {
    public delegate void SendKawaTV(object sender, ulong OW, ulong NumberReject);
    public event SendKawaTV OnSendKawaTV;

    public delegate void SendChangeOver(object sender, string FGs, string Name, double normalSpeed);
    public event SendChangeOver OnSendChangeOver;

    public delegate void SendSaveLoBB(object sender, string LoBB);
    public event SendSaveLoBB OnSendSaveLoBB;

    public delegate void SendSaveOP(object sender, string OP);
    public event SendSaveOP OnSendSaveOP;
    public delegate void SendSaveQC(object sender, string OP);
    public event SendSaveQC OnSendSaveQC;
    public delegate void SendSaveTC(object sender, string OP);
    public event SendSaveTC OnSendSaveTC;

    public FrmDashboard()
    {
      InitializeComponent();
      CustomUI();
      RegisterService();
    }

    #region Singleton parttern
    private static FrmDashboard _Instance = null;
    public static FrmDashboard Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmDashboard();
        }
        return _Instance;
      }
    }
    #endregion


    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = tableLayoutPanel20;
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
      elipseControl4.TargetControl = tableLayoutPanel7;
      elipseControl4.CornerRadius = 20;

      ElipseControl elipseControl5 = new ElipseControl();
      elipseControl5.TargetControl = tableLayoutPanel16;
      elipseControl5.CornerRadius = 20;

      ElipseControl elipseControl6 = new ElipseControl();
      elipseControl6.TargetControl = tableLayoutPanel24;
      elipseControl6.CornerRadius = 20;

      lbOverWeight.ValueTilte = "OW (%)";
      lbTLTB.ValueTilte = "TL trung bình (g)";
      ucInformationLoss1.ValueTitle = "Thông tin loss";
    }


    private OperationSettingService _operationSettingService { get; set; }
    private void RegisterService()
    {
      _operationSettingService = AppFactory.CreateOperationSettingService();
    }

    private System.Timers.Timer timer_UpdateUI = new System.Timers.Timer();
    private DataChart _dataChart = new DataChart();
    private List<string> dataTimeData = new List<string>();
    private List<DataRejectDTO> reject = new List<DataRejectDTO>();
    private SumaryDTO sumaryDTO = new SumaryDTO();
    private void FrmDashboard_Load(object sender, EventArgs e)
    {
      //Init chart
      _dataChart.ChartControlInit(chartControl);
      _dataChart.ChartHistogramInit(chartHistogram);

      //Show thông tin cài đặt
      ShowInforOperator(AppCore.Ins._operationSettingCurrent?.OP,
        AppCore.Ins._operationSettingCurrent?.QC,
        AppCore.Ins._operationSettingCurrent?.ShiftLeader);

      ShowInforProduct(AppCore.Ins._productCurrent, AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0);
      ShowInforLotAndTare(AppCore.Ins._tareSettingCurrent);

      //Tạo timer load data 2s
      timer_UpdateUI.Interval = 2000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      timer_UpdateUI.Start();

      //Sự kiện
      AppCore.Ins.OnSendAutoReport += Ins_OnSendAutoReport1;
    }

    private void Ins_OnSendAutoReport1(object sender, int shiftId, int productId)
    {
      ResetDashboard();
    }

    private void ResetDashboard()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetDashboard();
        }));
        return;
      }

      try
      {
        sumaryDTO = new SumaryDTO();
        dataTimeData = new List<string>();
        reject = new List<DataRejectDTO>();
        UpdateDataUI(true);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
    }

    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      this.timer_UpdateUI.Stop();

      try
      {
        LoadDataDashBoard();
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
      finally
      {
        timer_UpdateUI.Start();
      }
    }

    private void LoadDataDashBoard()
    {
      try
      {
        if (AppCore.Ins._datalogsInShiftCurrent == null || AppCore.Ins._datalogsInShiftCurrent?.Count() == 0)
        {
          ResetDashboard();
          return;
        }

        var list = AppCore.Ins._datalogsInShiftCurrent;
        sumaryDTO = AppCore.Ins.SumaryDTOData(list, AppCore.Ins._productCurrent, AppCore.Ins._tareSettingCurrent);

        //Data time
        dataTimeData = sumaryDTO.DatalogPass.Select(x => x.CreatedAt.ToString()).ToList();

        //Data reject
        reject = new List<DataRejectDTO>();
        if (sumaryDTO.DatalogReject?.Count() > 0)
        {
          foreach (var data in sumaryDTO.DatalogReject)
          {
            DataRejectDTO dataReject = new DataRejectDTO();
            dataReject.DateTime = (DateTime)data.CreatedAt;
            dataReject.FGs = AppCore.Ins._productCurrent.Code;
            dataReject.Actual = data.Net;
            dataReject.Target = sumaryDTO.Target;
            reject.Add(dataReject);
          }
        }

        UpdateDataUI(true);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    private void UpdateDataUI(bool isUpdateChart)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          UpdateDataUI(isUpdateChart);
        }));
        return;
      }

      if (isUpdateChart)
      {
        _dataChart.AddChartControlDashboard(chartControl, sumaryDTO, dataTimeData, 0);
        _dataChart.AddChartHistogram(chartHistogram, sumaryDTO);
        ucChartPie1.SetDataChartPie(sumaryDTO);
      }

      lbDataNumberReject.ValueStr = sumaryDTO.DatalogReject.Count().ToString();
      ucInformationDataSumary1.SetSumaryDTO(sumaryDTO);
      SetDataOW_Mean(sumaryDTO);
      UpdateInforLoss(sumaryDTO);
      UpdateDataReject(reject);
    }

    private void UpdateInforLoss(SumaryDTO sumaryDTO)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          UpdateInforLoss(sumaryDTO);
        }));
        return;
      }

      double lossByReject = sumaryDTO.DatalogReject.Sum(x => x.Net);
      double lossByOW = sumaryDTO.OW * sumaryDTO.Target;

      ucInformationLoss1.ValueLossReject = Math.Round( (lossByReject/1000.0),2).ToString();
      ucInformationLoss1.ValueLossOW = Math.Round( (lossByOW / 1000.0),2).ToString();
    }


    private int numberRejectLast = 0;
    private void UpdateDataReject(List<DataRejectDTO> dataRejects)
    {
      try
      {
        if (this.InvokeRequired)
        {
          this.Invoke(new Action(() =>
          {
            UpdateDataReject(dataRejects);
          }));
          return;
        }

        if (dataRejects == null)
        {
          dgvReject.Rows.Clear();
          return;
        }

        if (numberRejectLast != dataRejects.Count)
        {
          dataRejects = dataRejects.OrderByDescending(x => x.DateTime).ToList();
          dgvReject.Rows.Clear();
          foreach (var item in dataRejects)
          {
            int indexOfFirstSpace = item.DateTime.ToString().IndexOf(' ');
            string timeOnly = item.DateTime.ToString().Substring(indexOfFirstSpace + 1);

            dgvReject.Rows.Add(timeOnly, item.FGs, item.Target, item.Actual);
          }
          numberRejectLast = dataRejects.Count();
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    private void SetDataOW_Mean(SumaryDTO sumaryDTO)
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



    private void btnSendLoBB_Click(object sender, EventArgs e)
    {
      //if (Properties.Settings.Default.LoBB != ucTextBoxLoBB.Texts)
      //{
      //  if (OnSendSaveLoBB != null)
      //  {
      //    OnSendSaveLoBB(this, this.ucTextBoxLoBB.Texts);
      //  }
      //  Properties.Settings.Default.LoBB = this.ucTextBoxLoBB.Texts;
      //  Properties.Settings.Default.Save();
      //  isLoBBSendPLC = true;
      //}

      //new FrmInformation().ShowMessage($"Lô bao bì: {this.ucTextBoxLoBB.Texts} đã được thay đổi !", eImage.Information);
      //AppCore.Ins.LogActiveAppToFileLog($"Thay đổi lô BB: {this.ucTextBoxLoBB.Texts}");
    }

    private void label3_Click(object sender, EventArgs e)
    {

    }

    private void label9_Click(object sender, EventArgs e)
    {

    }

    private void btnChangeOperator_Click(object sender, EventArgs e)
    {
      PopupChangeOperator popupChangeOperator = new PopupChangeOperator();
      popupChangeOperator.OnSelectedEmployees += PopupChangeOperator_OnSelectedEmployees;
      popupChangeOperator.ShowDialog();
    }

    private async void PopupChangeOperator_OnSelectedEmployees(Employee arg1, Employee arg2, Employee arg3)
    {
      //Save cài đặt
      OperationSetting operationSetting = new OperationSetting();
      operationSetting.OP = arg1.FullName;
      operationSetting.QC = arg2.FullName;
      operationSetting.ShiftLeader = arg3.FullName;
      operationSetting.CreatedAt = DateTime.UtcNow;
      AppCore.Ins._operationSettingCurrent = await _operationSettingService.AddAsync(operationSetting);

      ShowInforOperator(arg1.FullName, arg2.FullName, arg3.FullName);
    }

    private void ShowInforOperator(string op, string qc, string shiftleader)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforOperator(op, qc, shiftleader); }));
        return;
      }

      lbOP.ValueStr = op;
      lbQC.ValueStr = qc;
      lbShiftLeader.ValueStr = shiftleader;
    }

    private void btnChangeOver_Click(object sender, EventArgs e)
    {
      PopupChangeFGs popupChangeFGs = new PopupChangeFGs();
      popupChangeFGs.OnSelectedProduct += PopupChangeFGs_OnSelectedProduct;
      popupChangeFGs.ShowDialog();
    }

    private async void PopupChangeFGs_OnSelectedProduct(Product obj)
    {
      AppCore.Ins._productCurrent = obj;
      AppCore.Ins._appConfig.ProductId = obj.Id;
      AppCore.Ins._appConfig.UpdatedAt = DateTime.UtcNow;
      await AppCore.Ins.UpdateAppConfig(AppCore.Ins._appConfig);

      ShowInforProduct(obj, AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0);
    }

    private void ShowInforProduct(Product product, double tube, double carton)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforProduct(product, tube, carton); }));
        return;
      }

      lbFGs.ValueStr = product?.Code ?? string.Empty;
      lbNameProduct.ValueStr = product?.Description ?? string.Empty;
      ucInformationDataSumary1.SetInforProduct(product, AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0);
    }

    private void btnSettingTareAndLot_Click(object sender, EventArgs e)
    {
      PopupChangeTareAndLot popupChangeTareAndLot = new PopupChangeTareAndLot(AppCore.Ins._tareSettingCurrent);
      popupChangeTareAndLot.OnChangeTareSetting += PopupChangeTareAndLot_OnChangeTareSetting;
      popupChangeTareAndLot.ShowDialog();
    }

    private void PopupChangeTareAndLot_OnChangeTareSetting(TareSetting obj)
    {
      ShowInforLotAndTare(obj);
      AppCore.Ins._tareSettingCurrent = obj;
    }

    private void ShowInforLotAndTare(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforLotAndTare(tareSetting); }));
        return;
      }

      lbLot.ValueStr = tareSetting?.Lot ?? string.Empty;
      lbTube.ValueStr = tareSetting?.Tube.ToString() ?? string.Empty;
      lbTailTube.ValueStr = tareSetting?.TailTube.ToString() ?? string.Empty;
      lbCarton.ValueStr = tareSetting?.Carton.ToString() ?? string.Empty;
    }

    private void label8_Click(object sender, EventArgs e)
    {
      AppCore.Ins.ChangeShiftTest();
    }
  }
}
