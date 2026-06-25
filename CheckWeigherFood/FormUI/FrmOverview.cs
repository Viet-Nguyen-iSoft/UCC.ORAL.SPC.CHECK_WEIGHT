using CheckWeigherFood.Controls;
using CheckWeigherFood.FrmChild;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.Popup;
using CheckWeigherFood.RJControl;
using CheckWeigherFood.UC;
using Database.DTO;
using Database.Models;
using Database.Service;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.FormUI
{
  public partial class FrmOverview : Form
  {
    private OperationSettingService _operationSettingService { get; set; }
    public FrmOverview()
    {
      InitializeComponent();
      CustomUI();
      RegisterService();
      ucOverviewLine3.SetKeyMachine(3);
      ucOverviewLine4.SetKeyMachine(4);
      this.Load += FrmOverview_Load;

      ucOverviewLine3.OnSendChangeProduct += UcOverview_OnSendChangeProduct;
      ucOverviewLine4.OnSendChangeProduct += UcOverview_OnSendChangeProduct;
    }
    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = tableLayoutPanel3;
      elipseControl0.CornerRadius = 20;

      ElipseControl elipseControl1 = new ElipseControl();
      elipseControl1.TargetControl = tableLayoutPanel4;
      elipseControl1.CornerRadius = 20;
    }

    private void UcOverview_OnSendChangeProduct(long obj)
    {
      PopupChangeFGs popupChangeFGs = new PopupChangeFGs();
      popupChangeFGs.OnSelectedProduct += PopupChangeFGs_OnSelectedProduct;
      popupChangeFGs.ShowDialog();
    }

    private void PopupChangeFGs_OnSelectedProduct(long line, Product obj)
    {
      try
      {
        //if (line == 3)
        //{
        //  AppCore.Ins._productCurrent03 = obj;

        //  AppCore.Ins._machineCurrent03.ChangeOverId = AppCore.Ins._machineCurrent04.ChangeOverId + 1;
        //  AppCore.Ins._machineCurrent03.ProductId = obj.Id;
        //  AppCore.Ins._machineCurrent03.UpdatedAt = DateTime.UtcNow;
        //  await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent03);
        //  AppCore.Ins._datalogsInShiftCurrent_Line3 = new List<Datalog>();
        //  ShowInforProduct(obj, AppCore.Ins._tareSettingCurrent03?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent03?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent03?.Carton ?? 0.0);
        //}
        //else if (line == 4)
        //{
        //  AppCore.Ins._productCurrent04 = obj;
        //  AppCore.Ins._machineCurrent04.ChangeOverId = AppCore.Ins._machineCurrent04.ChangeOverId + 1;

        //  AppCore.Ins._machineCurrent04.ProductId = obj.Id;
        //  AppCore.Ins._machineCurrent04.UpdatedAt = DateTime.UtcNow;
        //  await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent04);
        //  AppCore.Ins._datalogsInShiftCurrent_Line4 = new List<Datalog>();
        //  ShowInforProduct(obj, AppCore.Ins._tareSettingCurrent04?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.TailTube ?? 0.0, AppCore.Ins._tareSettingCurrent04?.Carton ?? 0.0);
        //}
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

    private void RegisterService()
    {
      _operationSettingService = AppFactory.CreateOperationSettingService();
    }


    #region Singleton parttern
    private static FrmOverview _Instance = null;
    public static FrmOverview Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmOverview();
        }
        return _Instance;
      }
    }
    #endregion


    private System.Timers.Timer timer_UpdateUI = new System.Timers.Timer();
    private void FrmOverview_Load(object sender, EventArgs e)
    {
      ucOverviewLine3.InitChart();
      ucOverviewLine4.InitChart();

      ucOverviewLine3.SetTimeFilterChart(AppCore.Ins._shiftCurrent);
      ucOverviewLine4.SetTimeFilterChart(AppCore.Ins._shiftCurrent);

      ShowInforOperator(AppCore.Ins._operationSettingCurrent03?.OP ?? string.Empty,
                        AppCore.Ins._operationSettingCurrent04?.OP ?? string.Empty,
                        AppCore.Ins._operationSettingCurrent04?.QC ?? string.Empty,
                        AppCore.Ins._operationSettingCurrent04?.ShiftLeader ?? string.Empty
        );

      ShowInforProduct(AppCore.Ins._productCurrent03, AppCore.Ins._tareSettingCurrent03, 3);
      ShowInforProduct(AppCore.Ins._productCurrent04, AppCore.Ins._tareSettingCurrent04, 4);

      //Tạo timer load data 2s
      timer_UpdateUI.Interval = 2000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      timer_UpdateUI.Start();

      AppCore.Ins.OnSendValueWeight += Ins_OnSendValueWeight;
    }

    private void Ins_OnSendValueWeight(double value, bool statusMachine, long machineKey)
    {
      try
      {
        if (machineKey == 3)
        {
          ucOverviewLine3.SetValueWeightRealtime(value);
        }
        else if (machineKey == 4)
        {
          ucOverviewLine4.SetValueWeightRealtime(value);
        }
      }
      catch (Exception)
      {

      }
    }

    private List<Datalog> dataChartline03 = new List<Datalog>();
    private List<Datalog> dataChartline04 = new List<Datalog>();

    private void LoadDataDashBoard()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          LoadDataDashBoard();
        }));
        return;
      }

      var dt03 = ucOverviewLine3.GetDt();
      dataChartline03 = AppCore.Ins._sumaryDTOLine3.DatalogPass
                      .Where(x => x.CreatedAt >= dt03.From && x.CreatedAt <= dt03.To)
                      .OrderBy(x => x.CreatedAt)
                      .ToList();
      ucOverviewLine3.ChartLine(AppCore.Ins._sumaryDTOLine3, dataChartline03);
      ucOverviewLine3.SetDataOW_Mean(AppCore.Ins._sumaryDTOLine3);
      ucOverviewLine3.SetSumaryDTO(AppCore.Ins._sumaryDTOLine3);
      ucOverviewLine3.SetStatusMachine(AppCore.Ins._enumStatusMachine03);
      if (AppCore.Ins._enumStatusMachine03 == Database.Enum.EnumStatusMachine.Run)
      {
        tableLayoutPanel3.BackColor = Color.Lime;
      }
      else if (AppCore.Ins._enumStatusMachine03 == Database.Enum.EnumStatusMachine.Stop)
      {
        tableLayoutPanel3.BackColor = Color.Tomato;
      }  

      //lbDataNumberReject.ValueStr = AppCore.Ins._sumaryDTOLine3.DatalogReject.Count().ToString();
      //ucInformationDataSumary1.SetSumaryDTO(AppCore.Ins._sumaryDTOLine3);
      //SetDataOW_Mean(AppCore.Ins._sumaryDTOLine3);
      //UpdateInforLoss(AppCore.Ins._sumaryDTOLine3);

      //SetContent(3);



      var dt04 = ucOverviewLine4.GetDt();
      dataChartline04 = AppCore.Ins._sumaryDTOLine4.DatalogPass
                      .Where(x => x.CreatedAt >= dt04.From && x.CreatedAt <= dt04.To)
                      .OrderBy(x => x.CreatedAt)
                      .ToList();
      ucOverviewLine4.ChartLine(AppCore.Ins._sumaryDTOLine4, dataChartline04);
      ucOverviewLine4.SetDataOW_Mean(AppCore.Ins._sumaryDTOLine4);
      ucOverviewLine4.SetSumaryDTO(AppCore.Ins._sumaryDTOLine4);
      ucOverviewLine4.SetStatusMachine(AppCore.Ins._enumStatusMachine04);

      if (AppCore.Ins._enumStatusMachine04 == Database.Enum.EnumStatusMachine.Run)
      {
        tableLayoutPanel4.BackColor = Color.Lime;
      }
      else if (AppCore.Ins._enumStatusMachine04 == Database.Enum.EnumStatusMachine.Stop)
      {
        tableLayoutPanel4.BackColor = Color.Tomato;
      }
    }
   
    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        timer_UpdateUI.Stop();
        LoadDataDashBoard();
      }
      catch (Exception ex)
      {

      }
      finally
      {
        timer_UpdateUI.Start();
      }
    }

    private void ShowInforOperator(string op03, string op04, string qc, string shiftleader)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforOperator(op03, op04, qc, shiftleader); }));
        return;
      }

      lbOP03.ValueStr = op03;
      lbOP04.ValueStr = op04;
      lbQC.ValueStr = qc;
      lbShiftLeader.ValueStr = shiftleader;
    }

    private void ShowInforProduct(Product product, TareSetting tareSetting, long keyMachine)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforProduct(product, tareSetting, keyMachine); }));
        return;
      }

      if (keyMachine==3)
        ucOverviewLine3.ShowInforProduct(product, tareSetting);
      else if (keyMachine == 4)
        ucOverviewLine4.ShowInforProduct(product, tareSetting);
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

      //try
      //{
      //  sumaryDTO = new SumaryDTO();
      //  reject = new List<DataRejectDTO>();
      //  UpdateDataUI(true);
      //}
      //catch (Exception ex)
      //{
      //  Debug.WriteLine(ex);
      //}
    }
    private void btnChangeOperator_Click(object sender, EventArgs e)
    {
      PopupChangeOperationAll popupChangeOperationAll = new PopupChangeOperationAll();
      popupChangeOperationAll.OnSelectedEmployees += PopupChangeOperationAll_OnSelectedEmployees;
      popupChangeOperationAll.ShowDialog();
    }

    private async void PopupChangeOperationAll_OnSelectedEmployees(Database.Models.Employee arg1, Database.Models.Employee arg2, Database.Models.Employee arg3, Database.Models.Employee arg4)
    {
      try
      {
        OperationSetting operationSetting03 = new OperationSetting();
        operationSetting03.OP = arg1.FullName;
        operationSetting03.QC = arg3.FullName;
        operationSetting03.ShiftLeader = arg4.FullName;
        operationSetting03.KeyMachine = 3;
        operationSetting03.CreatedAt = DateTime.UtcNow;

        OperationSetting operationSetting04 = new OperationSetting();
        operationSetting04.OP = arg2.FullName;
        operationSetting04.QC = arg3.FullName;
        operationSetting04.ShiftLeader = arg4.FullName;
        operationSetting04.KeyMachine = 4;
        operationSetting04.CreatedAt = DateTime.UtcNow;

        AppCore.Ins._operationSettingCurrent03 = await _operationSettingService.AddAsync(operationSetting03);
        AppCore.Ins._operationSettingCurrent04 = await _operationSettingService.AddAsync(operationSetting04);

        FrmDashboard.Instance.ResfreshOperation();
      }
      catch (Exception ex)
      {
        new FrmInformation().ShowMessage($"Lưu thất bại: {ex.ToString()}", eImage.Warning);
      }
    }
  }
}
