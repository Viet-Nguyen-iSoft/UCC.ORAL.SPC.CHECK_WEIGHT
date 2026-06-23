using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.Popup;
using CheckWeigherFood.RJControl;
using CheckWeigherFood.UC;
using Database.DTO;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;
using static CheckWeigherFood.eNum.eNumUI;
using static Database.Enum;
using Timer = System.Windows.Forms.Timer;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmDashboard : Form
  {
    public delegate void SendChangeOver(object sender, string FGs, string Name, double normalSpeed);
    public event SendChangeOver OnSendChangeOver;

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

    private EnumStatusMachine _enumStatusMachine { get; set; }
    private Timer timerMarquee = new Timer();
    private Timer timerBlinkStatus = new Timer();
    private Timer timerBlinkStatusInforline = new Timer();
    private bool _statusConnectOpcUa = false;

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

      ElipseControl elipseControl7 = new ElipseControl();
      elipseControl7.TargetControl = tableLayoutPanel14;
      elipseControl7.CornerRadius = 20;

      ElipseControl elipseControl8 = new ElipseControl();
      elipseControl8.TargetControl = panelContent;
      elipseControl8.CornerRadius = 20;

      lbOverWeight.ValueTilte = "OW (%)";
      lbTLTB.ValueTilte = "TL trung bình (g)";
      ucInformationLoss1.ValueTitle = "Thông tin loss";

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


    private OperationSettingService _operationSettingService { get; set; }
    private TareSettingService _tareSettingService { get; set; }
    private void RegisterService()
    {
      _operationSettingService = AppFactory.CreateOperationSettingService();
      _tareSettingService = AppFactory.CreateTareSettingService();
    }

    private System.Timers.Timer timer_UpdateUI = new System.Timers.Timer();
    private DataChart _dataChart = new DataChart();
    //private List<string> dataTimeData = new List<string>();
    private List<Datalog> dataChartline = new List<Datalog>();
    private List<DataRejectDTO> reject = new List<DataRejectDTO>();
    private SumaryDTO sumaryDTO = new SumaryDTO();

    private System.Threading.Timer _watchdogTimer;
    private readonly object _lockObj = new object();
    private void FrmDashboard_Load(object sender, EventArgs e)
    {
      //Init chart
      _dataChart.ChartControlInit(chartControl);
      _dataChart.ChartHistogramInit(chartHistogram);


      

      //Lấy tgian ca hiện tại set filter chart
      SetTimeFilterChart(AppCore.Ins._shiftCurrent);
      GetDt();

      //Tạo timer load data 2s
      timer_UpdateUI.Interval = 2000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      timer_UpdateUI.Start();


      lbContent.AutoSize = true;
      lbContent.Left = panel1.Width;

      timerMarquee.Interval = 100;
      timerMarquee.Tick += TimerMarquee_Tick;
      timerMarquee.Start();

      timerBlinkStatus.Interval = 500;
      timerBlinkStatus.Tick += TimerBlinkStatus_Tick;
      //timerBlinkStatus.Start();

      //timerBlinkStatusInforline.Interval = 500;
      //timerBlinkStatusInforline.Tick += TimerBlinkStatusInforline_Tick;
      //timerBlinkStatusInforline.Start();

      
      cbbLine.SelectedIndexChanged += CbbLine_SelectedIndexChanged;
      cbbLine.SelectedIndex = 0;

      //Sự kiện
      AppCore.Ins.OnSendAutoReport += Ins_OnSendAutoReport1;
      AppCore.Ins.OnSendValueWeight += Ins_OnSendValueWeight;
      AppCore.Ins.OnSendReSetInforShift += Ins_OnSendReSetInforShift;
      AppCore.Ins.OnSendMsgRead += Ins_OnSendMsgRead;
      AppCore.Ins.OnSendDebug += Ins_OnSendDebug;

      InitWatchdog();
    }

    private void CbbLine_SelectedIndexChanged(object sender, EventArgs e)
    {
      int line = int.Parse( cbbLine.SelectedItem.ToString());
      if (line==3)
      {
        //Show thông tin cài đặt
        ShowInforOperator(AppCore.Ins._operationSettingCurrent03?.OP,
          AppCore.Ins._operationSettingCurrent03?.QC,
          AppCore.Ins._operationSettingCurrent03?.ShiftLeader);

        ShowInforProduct(AppCore.Ins._productCurrent03, AppCore.Ins._tareSettingCurrent03?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent03?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent03?.Carton ?? 0.0);
        ShowInforLotAndTare(AppCore.Ins._tareSettingCurrent03);
      }
      else if (line == 4)
      {
        //Show thông tin cài đặt
        ShowInforOperator(AppCore.Ins._operationSettingCurrent04?.OP,
          AppCore.Ins._operationSettingCurrent04?.QC,
          AppCore.Ins._operationSettingCurrent04?.ShiftLeader);

        ShowInforProduct(AppCore.Ins._productCurrent04, AppCore.Ins._tareSettingCurrent04?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.Carton ?? 0.0);
        ShowInforLotAndTare(AppCore.Ins._tareSettingCurrent04);
      }
    }

    public void ResfreshOperation()
    {
      if (cbbLine.SelectedIndex!=-1)
      {
        int line = int.Parse(cbbLine.SelectedItem.ToString());
        if (line == 3)
        {
          //Show thông tin cài đặt
          ShowInforOperator(AppCore.Ins._operationSettingCurrent03?.OP,
            AppCore.Ins._operationSettingCurrent03?.QC,
            AppCore.Ins._operationSettingCurrent03?.ShiftLeader);
        }
        else if (line == 4)
        {
          //Show thông tin cài đặt
          ShowInforOperator(AppCore.Ins._operationSettingCurrent04?.OP,
            AppCore.Ins._operationSettingCurrent04?.QC,
            AppCore.Ins._operationSettingCurrent04?.ShiftLeader);
        }
      } 
    }

    private void Ins_OnSendDebug(string msg)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          Ins_OnSendDebug(msg);
        }));
        return;
      }

      label4.Text = msg;
    }

    private void Ins_OnSendMsgRead(string msg)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          Ins_OnSendMsgRead(msg);
        }));
        return;
      }

      label28.Text = msg;
    }

    private void TimerBlinkStatusInforline_Tick(object sender, EventArgs e)
    {
      try
      {
        timerBlinkStatusInforline.Stop();
        CheckInforLine();
      }
      catch (Exception)
      {

      }
      finally
      {
        timerBlinkStatusInforline.Start();
      }
    }

    public void InitWatchdog()
    {
      _watchdogTimer = new System.Threading.Timer(
          WatchdogTimeout,
          null,
          Timeout.Infinite,
          Timeout.Infinite);
    }

    private void ResetWatchdog()
    {
      lock (_lockObj)
      {
        _watchdogTimer?.Change(
            TimeSpan.FromSeconds(60),
            Timeout.InfiniteTimeSpan);

        _enumStatusMachine = EnumStatusMachine.Run;
      }
    }

    private void WatchdogTimeout(object state)
    {
      _enumStatusMachine = EnumStatusMachine.Stop;
    }

    private void CheckInforLine()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { CheckInforLine(); }));
        return;
      }

      if (string.IsNullOrEmpty(lbOP.ValueStr.Trim()))
      {
        lbOP.Visible = !lbOP.Visible;
        lbOP.SetBackColor = Color.Yellow;
      }
      else
      {
        lbOP.Visible = true;
        lbOP.SetBackColor = Color.White;
      }


      if (string.IsNullOrEmpty(lbQC.ValueStr.Trim()))
      {
        lbQC.Visible = !lbQC.Visible;
        lbQC.SetBackColor = Color.Yellow;
      }
      else
      {
        lbQC.Visible = true;
        lbQC.SetBackColor = Color.White;
      }


      if (string.IsNullOrEmpty(lbShiftLeader.ValueStr.Trim()))
      {
        lbShiftLeader.Visible = !lbShiftLeader.Visible;
        lbShiftLeader.SetBackColor = Color.Yellow;
      }
      else
      {
        lbShiftLeader.Visible = true;
        lbShiftLeader.SetBackColor = Color.White;
      }

      if (string.IsNullOrEmpty(lbLotTube.ValueStr.Trim()))
      {
        lbLotTube.Visible = !lbLotTube.Visible;
        lbLotTube.SetBackColor = Color.Yellow;
      }
      else
      {
        lbLotTube.Visible = true;
        lbLotTube.SetBackColor = Color.White;
      }
    }

    private async void Ins_OnSendReSetInforShift()
    {
      SetTimeFilterChart(AppCore.Ins._shiftCurrent);
      GetDt();

      ShowInforOperator(string.Empty, string.Empty, string.Empty);
      ClearLot();

      ////Save cài đặt OP, QC, TC
      OperationSetting operationSetting = new OperationSetting();
      operationSetting.OP = "";
      operationSetting.QC = "";
      operationSetting.ShiftLeader = "";
      operationSetting.CreatedAt = DateTime.UtcNow;
      //AppCore.Ins._operationSettingCurrent = await _operationSettingService.AddAsync(operationSetting);

      ////Rst lot
      //TareSetting tareSetting = new TareSetting();
      //tareSetting.Lot = "";
      //tareSetting.Carton = AppCore.Ins._tareSettingCurrent.Carton;
      //tareSetting.Tube = AppCore.Ins._tareSettingCurrent.Tube;
      //tareSetting.TailTube = AppCore.Ins._tareSettingCurrent.TailTube;
      //tareSetting.CreatedAt = DateTime.UtcNow;
      //AppCore.Ins._tareSettingCurrent = await _tareSettingService.AddAsync(tareSetting);

      //Cắt qua ca
      //AppCore.Ins._appConfig.ChangeOverId = AppCore.Ins._appConfig.ChangeOverId + 1;
      //AppCore.Ins._appConfig.UpdatedAt = DateTime.UtcNow;
      //await AppCore.Ins.UpdateAppConfig(AppCore.Ins._appConfig);
    }

    private void TimerBlinkStatus_Tick(object sender, EventArgs e)
    {
      try
      {
        timerBlinkStatus.Stop();
        BlinkStatusMachine();
      }
      catch (Exception)
      {

      }
      finally
      {
        timerBlinkStatus.Start();
      }
    }

    private void BlinkStatusMachine()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          BlinkStatusMachine();
        }));
        return;
      }
      lbStatusMachine.Visible = !lbStatusMachine.Visible;
    }

    private void Ins_OnSendValueWeight(double value, bool success, string msg)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          Ins_OnSendValueWeight(value, success, msg);
        }));
        return;
      }

      //label4.Text = msg;
      _statusConnectOpcUa = success;
      ucInformationDataSumary1.SetWeightRealtime(value);

      ResetWatchdog();
    }

    private void TimerMarquee_Tick(object sender, EventArgs e)
    {
      try
      {
        timerMarquee.Stop();
        ShowMsg();
      }
      catch (Exception)
      {

      }
      finally
      {
        timerMarquee.Start();
      }
    }

    private void ShowMsg()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ShowMsg();
        }));
        return;
      }

      lbContent.Left -= 5;

      // Khi chạy hết bên trái thì quay lại bên phải
      if (lbContent.Right < 20)
      {
        lbContent.Left = panel1.Width;
      }

      picAlarm.Visible = !picAlarm.Visible;
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
        SetStatusMachine();
        SetContent();
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
      //try
      //{
      //  if (AppCore.Ins._datalogsInShiftCurrent == null || AppCore.Ins._datalogsInShiftCurrent?.Count() == 0)
      //  {
      //    ResetDashboard();
      //    return;
      //  }

      //  GetDt();

      //  var list = AppCore.Ins._datalogsInShiftCurrent;
      //  sumaryDTO = AppCore.Ins.SumaryDTOData(list, AppCore.Ins._productCurrent04, AppCore.Ins._tareSettingCurrent04);

      //  //Data time
      //  dataChartline = sumaryDTO.DatalogPass
      //                  .Where(x => x.CreatedAt >= _from && x.CreatedAt <= _to)
      //                  .OrderBy(x=>x.CreatedAt)
      //                  .ToList();
      //  //dataTimeData = sumaryDTO.DatalogPass.Select(x => x.CreatedAt.ToString()).ToList();
        

      //  //dataTimeData = sumaryDTO.DatalogPass.Select(x => x.CreatedAt.ToString()).ToList();

      //  //Data reject
      //  reject = new List<DataRejectDTO>();
      //  if (sumaryDTO.DatalogReject?.Count() > 0)
      //  {
      //    foreach (var data in sumaryDTO.DatalogReject)
      //    {
      //      DataRejectDTO dataReject = new DataRejectDTO();
      //      dataReject.DateTime = (DateTime)data.CreatedAt;
      //      dataReject.FGs = AppCore.Ins._productCurrent04.Code;
      //      dataReject.Actual = data.Gross;
      //      dataReject.Target = sumaryDTO.Target;
      //      reject.Add(dataReject);
      //    }
      //  }

      //  UpdateDataUI(true);
      //}
      //catch (Exception ex)
      //{
      //  Debug.WriteLine(ex.Message);
      //}
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

      try
      {
        if (isUpdateChart)
        {
          _dataChart.AddChartControlDashboard(chartControl, sumaryDTO, dataChartline, 0);
          _dataChart.AddChartHistogram(chartHistogram, sumaryDTO);
          ucChartPie1.SetDataChartPie(sumaryDTO);
        }

        lbDataNumberReject.ValueStr = sumaryDTO.DatalogReject.Count().ToString();
        ucInformationDataSumary1.SetSumaryDTO(sumaryDTO);
        SetDataOW_Mean(sumaryDTO);
        UpdateInforLoss(sumaryDTO);
        UpdateDataReject(reject);
      }
      catch (Exception ex)
      {

      }
    }

    private void SetStatusMachine()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetStatusMachine();
        }));
        return;
      }

      _statusConnectOpcUa = true;
      if (_statusConnectOpcUa)
      {
        timerBlinkStatus.Stop();
        if (_enumStatusMachine == EnumStatusMachine.Run)
        {
          lbStatusMachine.Text = "MÁY CHẠY";
          lbStatusMachine.ForeColor = Color.LightGreen;
        }
        else if (_enumStatusMachine == EnumStatusMachine.Stop)
        {
          lbStatusMachine.Text = "MÁY DỪNG";
          lbStatusMachine.ForeColor = Color.Yellow;
        }
      } 
      else
      {
        if (_enumStatusMachine!= EnumStatusMachine.Disconnect)
        {
          _enumStatusMachine = EnumStatusMachine.Disconnect;
          timerBlinkStatus.Start();
        }  
        
        lbStatusMachine.Text = "MẤT KẾT NỐI OPC UA";
        lbStatusMachine.ForeColor = Color.Red;
      }  
    }

    private void SetContent()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetContent();
        }));
        return;
      }

      if (sumaryDTO.OW > 0.5)
      {
        double value = Math.Round( sumaryDTO.Mean - sumaryDTO.Target, 2);
        string msg = $"OW cao cần giảm trọng lượng {value}g";
        lbContent.Text = msg;
        lbContent.ForeColor = Color.Red;
        panelContent.Visible = true;
      } 
      else
      {
        //Kết quả
        if (sumaryDTO.EnumResult == EnumResult.Pass)
        {
          //lbContent.ForeColor = Color.DarkGreen;
          //lbContent.Text = "Line sản xuất ĐẠT trọng lượng tiêu chuẩn";
          //lbContent.Visible = true;
          panelContent.Visible = false;
        }
        else if (sumaryDTO.EnumResult == EnumResult.Fail)
        {
          string mgs = "Line sản xuất KHÔNG ĐẠT trọng lượng tiêu chuẩn";
          lbContent.ForeColor = Color.Red;
          lbContent.Text = mgs;

          panelContent.Visible = true;
          //lbContent.Visible = true;
        }
        else
        {
          //string mgs = "   KHÔNG CÓ MẪU CỦA SẢN PHẨM TRONG CA HIỆN TẠI";
          //lbContent.ForeColor = Color.Black;
          //lbContent.Text = mgs;

          panelContent.Visible = false;
          //lbContent.Visible = false;
        }
      }

      
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

      double cnt = (double)(sumaryDTO.DatalogAccept.Count());
      double lossByReject = sumaryDTO.DatalogReject.Sum(x => x.Gross);
      double lossByOW = (sumaryDTO.OW/100.0) * sumaryDTO.targetSrc* cnt;

      ucInformationLoss1.ValueLossReject = Math.Round((lossByReject / 1000.0), 2).ToString();
      ucInformationLoss1.ValueLossOW = Math.Round((lossByOW / 1000.0), 2).ToString();
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
      try
      {
        //Save cài đặt
        int line = int.Parse(cbbLine.SelectedItem.ToString());
        OperationSetting operationSetting = new OperationSetting();
        operationSetting.OP = arg1.FullName;
        operationSetting.QC = arg2.FullName;
        operationSetting.ShiftLeader = arg3.FullName;
        operationSetting.KeyMachine = line;
        operationSetting.CreatedAt = DateTime.UtcNow;

        if (line == 3)
        {
          OperationSetting operationSetting04 = new OperationSetting();
          operationSetting04.OP = AppCore.Ins._operationSettingCurrent04.OP;
          operationSetting04.QC = arg2.FullName;
          operationSetting04.ShiftLeader = arg3.FullName;
          operationSetting04.KeyMachine = 4;
          operationSetting04.CreatedAt = DateTime.UtcNow;
          AppCore.Ins._operationSettingCurrent04 = await _operationSettingService.AddAsync(operationSetting04);

          AppCore.Ins._operationSettingCurrent03 = await _operationSettingService.AddAsync(operationSetting);

          ShowInforOperator(arg1.FullName, arg2.FullName, arg3.FullName);
        }
        else if (line == 4)
        {
          OperationSetting operationSetting03 = new OperationSetting();
          operationSetting03.OP = AppCore.Ins._operationSettingCurrent03.OP;
          operationSetting03.QC = arg2.FullName;
          operationSetting03.ShiftLeader = arg3.FullName;
          operationSetting03.KeyMachine = 3;
          operationSetting03.CreatedAt = DateTime.UtcNow;
          AppCore.Ins._operationSettingCurrent03 = await _operationSettingService.AddAsync(operationSetting03);

          AppCore.Ins._operationSettingCurrent04 = await _operationSettingService.AddAsync(operationSetting);

          ShowInforOperator(arg1.FullName, arg2.FullName, arg3.FullName);
        }
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
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
      int line = int.Parse(cbbLine.SelectedItem.ToString());
      PopupChangeFGs popupChangeFGs = new PopupChangeFGs(line);
      popupChangeFGs.OnSelectedProduct += PopupChangeFGs_OnSelectedProduct;
      popupChangeFGs.ShowDialog();
    }

    private async void PopupChangeFGs_OnSelectedProduct(long line, Product obj)
    {
      try
      {
        if (line == 3)
        {
          AppCore.Ins._productCurrent03 = obj;
          AppCore.Ins._machineCurrent03.ChangeOverId = AppCore.Ins._machineCurrent03.ChangeOverId + 1;

          AppCore.Ins._machineCurrent03.ProductId = obj.Id;
          AppCore.Ins._machineCurrent03.UpdatedAt = DateTime.UtcNow;
          await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent03);
          AppCore.Ins._datalogsInShiftCurrent_Line3 = new List<Datalog>();
          ShowInforProduct(obj, AppCore.Ins._tareSettingCurrent03?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent03?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent03?.Carton ?? 0.0);
        }
        else if (line == 4)
        {
          AppCore.Ins._productCurrent04 = obj;
          AppCore.Ins._machineCurrent04.ChangeOverId = AppCore.Ins._machineCurrent04.ChangeOverId + 1;

          AppCore.Ins._machineCurrent04.ProductId = obj.Id;
          AppCore.Ins._machineCurrent04.UpdatedAt = DateTime.UtcNow;
          await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent04);
          AppCore.Ins._datalogsInShiftCurrent_Line4 = new List<Datalog>();
          ShowInforProduct(obj, AppCore.Ins._tareSettingCurrent04?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.Carton ?? 0.0);
        }
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

    private void ShowInforProduct(Product product, double tube, double tailTube, double carton)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforProduct(product, tube, tailTube, carton); }));
        return;
      }

      lbFGs.ValueStr = product?.Code ?? string.Empty;
      lbNameProduct.ValueStr = product?.Description ?? string.Empty;
      ucInformationDataSumary1.SetInforProduct(product, AppCore.Ins._tareSettingCurrent04?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.Carton ?? 0.0);
    }

    private void btnSettingTareAndLot_Click(object sender, EventArgs e)
    {
      PopupChangeTare  popupChangeTare = new PopupChangeTare(AppCore.Ins._tareSettingCurrent04);
      popupChangeTare.OnChangeTareSetting += PopupChangeTare_OnChangeTareSetting;
      popupChangeTare.ShowDialog();
    }

    private void PopupChangeTare_OnChangeTareSetting(TareSetting obj)
    {
      try
      {
        //Save cài đặt
        int line = int.Parse(cbbLine.SelectedItem.ToString());

        if (line == 3)
        {
          AppCore.Ins._tareSettingCurrent03 = obj;
          ShowInforTare(AppCore.Ins._tareSettingCurrent03);

          ShowInforProduct( AppCore.Ins._productCurrent03,
                            AppCore.Ins._tareSettingCurrent03?.Tube ?? 0.0,
                            AppCore.Ins._tareSettingCurrent03?.TailTube ?? 0.0,
                            AppCore.Ins._tareSettingCurrent03?.Carton ?? 0.0);
        }
        else if (line == 4)
        {
          AppCore.Ins._tareSettingCurrent04 = obj;
          ShowInforTare(AppCore.Ins._tareSettingCurrent04);

          ShowInforProduct(AppCore.Ins._productCurrent04,
                            AppCore.Ins._tareSettingCurrent04?.Tube ?? 0.0,
                            AppCore.Ins._tareSettingCurrent04?.TailTube ?? 0.0,
                            AppCore.Ins._tareSettingCurrent04?.Carton ?? 0.0);
        }
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

    private void ShowInforLotAndTare(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforLotAndTare(tareSetting); }));
        return;
      }

      lbLotTube.ValueStr = tareSetting?.LotTube ?? string.Empty;
      lbLotCarton.ValueStr = tareSetting?.LotCarton ?? string.Empty;
      lbTube.ValueStr = tareSetting?.Tube.ToString() ?? string.Empty;
      lbTailTube.ValueStr = tareSetting?.TailTube.ToString() ?? string.Empty;
      lbCarton.ValueStr = tareSetting?.Carton.ToString() ?? string.Empty;
    }

    private void ShowInforTare(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforTare(tareSetting); }));
        return;
      }

      lbTube.ValueStr = tareSetting?.Tube.ToString() ?? string.Empty;
      lbTailTube.ValueStr = tareSetting?.TailTube.ToString() ?? string.Empty;
      lbCarton.ValueStr = tareSetting?.Carton.ToString() ?? string.Empty;
    }

    private void ShowInforLot(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforLot(tareSetting); }));
        return;
      }

      lbLotTube.ValueStr = tareSetting?.LotTube ?? string.Empty;
      lbLotCarton.ValueStr = tareSetting?.LotCarton ?? string.Empty;
    }

    private void ClearLot( )
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ClearLot(); }));
        return;
      }

      lbLotTube.ValueStr =string.Empty;
    }

    private void SetTimeFilterChart(int shift)
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

    private void label8_Click(object sender, EventArgs e)
    {
      AppCore.Ins.ChangeShiftTest();
    }

    private void btnChangeLot_Click(object sender, EventArgs e)
    {
      int line = int.Parse(cbbLine.SelectedItem.ToString());
      if (line == 3)
      {
        PopupChangeLot popupChangeLot = new PopupChangeLot(AppCore.Ins._tareSettingCurrent03);
        popupChangeLot.OnChangeTareSetting += PopupChangeLot_OnChangeTareSetting;
        popupChangeLot.ShowDialog();
      }
      else if (line == 4)
      {
        PopupChangeLot popupChangeLot = new PopupChangeLot(AppCore.Ins._tareSettingCurrent04);
        popupChangeLot.OnChangeTareSetting += PopupChangeLot_OnChangeTareSetting;
        popupChangeLot.ShowDialog();
      }
     
    }

    private void PopupChangeLot_OnChangeTareSetting(TareSetting obj)
    {
      try
      {
        //Save cài đặt
        int line = int.Parse(cbbLine.SelectedItem.ToString());

        if (line == 3)
        {
          AppCore.Ins._tareSettingCurrent03 = obj;
          ShowInforLot(obj);
        }
        else if (line == 4)
        {
          AppCore.Ins._tareSettingCurrent04 = obj;
          ShowInforLot(obj);
        }
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }


    private DateTime _from { get;set; }
    private DateTime _to { get;set; }
    private void picFilterChart_Click(object sender, EventArgs e)
    {
      GetDt();
    }

    private void GetDt()
    {
      try
      {
        TimeSpan timeSpanFrom = ucFilterTime1.From;
        TimeSpan timeSpanTo = ucFilterTime1.To;

        _from = DateTime.Today.Add(timeSpanFrom);
        _to = DateTime.Today.Add(timeSpanTo);

        if (timeSpanTo < timeSpanFrom)
        {
          DateTime dt = DateTime.Now;
          if (dt.Hour>=0 && dt.Hour<=6)
          {
            _from = _from.AddDays(-1);
          }
          else
          {
            _to = _to.AddDays(1);
          }  
        }
      }
      catch (Exception)
      {

      }
    }


  }
}
