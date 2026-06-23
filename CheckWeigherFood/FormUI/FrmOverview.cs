using CheckWeigherFood.Controls;
using CheckWeigherFood.FrmChild;
using CheckWeigherFood.Popup;
using Database.DTO;
using Database.Models;
using Database.Service;
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
      RegisterService();
      ucOverviewLine3.SetKeyMachine(3);
      ucOverviewLine4.SetKeyMachine(4);
      this.Load += FrmOverview_Load;

      ucOverviewLine3.OnSendChangeProduct += UcOverview_OnSendChangeProduct;
      ucOverviewLine4.OnSendChangeProduct += UcOverview_OnSendChangeProduct;
    }

    private void UcOverview_OnSendChangeProduct(long obj)
    {
      PopupChangeFGs popupChangeFGs = new PopupChangeFGs();
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

          AppCore.Ins._machineCurrent03.ChangeOverId = AppCore.Ins._machineCurrent04.ChangeOverId + 1;
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
      //Tạo timer load data 2s
      timer_UpdateUI.Interval = 2000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      timer_UpdateUI.Start();
    }

   
    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      
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
      //                  .OrderBy(x => x.CreatedAt)
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
