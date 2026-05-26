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

      lbOverWeight.ValueTilte = "OW (%)";
      lbTLTB.ValueTilte = "TL trung bình (g)";
      ucInformationLoss1.ValueTitle = "Thông tin loss";
    }


    private OperationSettingService _operationSettingService { get; set; }
    private void RegisterService()
    {
      _operationSettingService = AppFactory.CreateOperationSettingService();
    }





    private int ShiftId = 0;
    private int ProductId = 0;

    private System.Timers.Timer timer_UpdateUI = new System.Timers.Timer();


    private double ProductMin = 0;
    private double ProductMax = 0;
    private double ProductLowerControl = 0;
    private double ProductUpperControl = 0;
    private double ProductTarget;
    private double ProductValueT;
    private string ProductSKU;

    private string ProductFGs;
    private string ProductName;
    private string ProductLoBB;


    private DataChart _dataChart = new DataChart();
    private void FrmDashboard_Load(object sender, EventArgs e)
    {
      //FrmSetting.Instance.OnSendChangeOver += Instance_OnSendChangeOver;
      FrmSetting.Instance.OnSendAddOP += Instance_OnSendAddOP;
      FrmSetting.Instance.OnSendAddQC += Instance_OnSendAddQC;
      FrmSetting.Instance.OnSendAddTC += Instance_OnSendAddTC;

      FrmMasterData.Instance.OnSendChangeMasterData += Instance_OnSendChangeMasterData;
      AppCore.Ins.OnSendReSetInforShift += Ins_OnSendAutoReport;

      _dataChart.ChartControlInit(chartControl);
      _dataChart.ChartHistogramInit(chartHistogram);

      timer_UpdateUI.Interval = 2000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      timer_UpdateUI.Start();

      UpdateInforProduct();
      LoadInfoLine();

      //var SelectIndexChangeFGs = new Action(() => { ComboboxSelectIndexChangeFGs(); });
      //this.reComboboxFGs.SetTag(SelectIndexChangeFGs);


      //var SelectIndexChangeOP = new Action(() => { ComboboxSelectIndexChangeOP(); });
      //this.reComboboxOP.SetTag(SelectIndexChangeOP);

      //var SelectIndexChangeQC = new Action(() => { ComboboxSelectIndexChangeQC(); });
      //this.reComboboxQC.SetTag(SelectIndexChangeQC);

      //var SelectIndexChangeTC = new Action(() => { ComboboxSelectIndexChangeTC(); });
      //this.reComboboxTC.SetTag(SelectIndexChangeTC);


      //this.txtCntIn.TextAlign();
      //this.txtCntOut.TextAlign();
      //this.txtCntReject.TextAlign();

      ShowInforOperator(AppCore.Ins._operationSettingCurrent?.OP,
        AppCore.Ins._operationSettingCurrent?.QC,
        AppCore.Ins._operationSettingCurrent?.ShiftLeader);

      ShowInforProduct(AppCore.Ins._productCurrent, AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0, AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0);
      ShowInforLotAndTare(AppCore.Ins._tareSettingCurrent);
    }

    private void Ins_OnSendAutoReport()
    {
      ResetUser();
    }

    private void Instance_OnSendAddTC()
    {
      ReloadUser();
    }

    private void Instance_OnSendAddQC()
    {
      ReloadUser();
    }

    private void Instance_OnSendAddOP()
    {
      ReloadUser();
    }

    private void ReloadUser()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ReloadUser();
        }));
        return;
      }

      //this.reComboboxOP.SetDataSource = AppCore.Ins._users?.Where(s => s.Role == "OP").Select(x => x.Name).ToList();
      //this.reComboboxQC.SetDataSource = AppCore.Ins._users?.Where(s => s.Role == "QC").Select(x => x.Name).ToList();
      //this.reComboboxTC.SetDataSource = AppCore.Ins._users?.Where(s => s.Role == "TC").Select(x => x.Name).ToList();

      //this.reComboboxOP.SelectedItem = AppCore.Ins._users?.Where(s => s.Role == "OP" & s.isEnable == true).Select(x => x.Name).FirstOrDefault();
      //this.reComboboxQC.SelectedItem = AppCore.Ins._users?.Where(s => s.Role == "QC" & s.isEnable == true).Select(x => x.Name).FirstOrDefault();
      //this.reComboboxTC.SelectedItem = AppCore.Ins._users?.Where(s => s.Role == "TC" & s.isEnable == true).Select(x => x.Name).FirstOrDefault();
    }

    private bool isChangeOver = true;
    private void Instance_OnSendChangeMasterData()
    {
      //isChangeOver = false;
      //List<string> list = AppCore.Ins._listMasterData.Where(x => x.isDelete == false).Select(x => x.FGs).ToList();
      //if (list != null)
      //{
      //  this.reComboboxFGs.SetDataSource = list;
      //  this.reComboboxFGs.SetValue = "";
      //  this.ucTextBoxProductName.Texts = "";
      //}
    }

    private async void ComboboxSelectIndexChangeOP()
    {
      //if (OnSendSaveOP != null)
      //  OnSendSaveOP(this, reComboboxOP.SetValue.Trim());

      //await AppCore.Ins.ClearActiveUser("OP");
      //await AppCore.Ins.ActiveUser(reComboboxOP.SetValue.Trim(), "OP");

      //AppCore.Ins.ReloadUser();

      //AppCore.Ins.LogActiveAppToFileLog("Thay đổi tên OP:" + reComboboxOP.SetValue.Trim());
    }
    private async void ComboboxSelectIndexChangeQC()
    {
      //if (OnSendSaveQC != null)
      //  OnSendSaveQC(this, reComboboxQC.SetValue.Trim());
      //await AppCore.Ins.ClearActiveUser("QC");
      //await AppCore.Ins.ActiveUser(reComboboxQC.SetValue.Trim(), "QC");

      AppCore.Ins.ReloadUser();

      //AppCore.Ins.LogActiveAppToFileLog("Thay đổi tên QC:" + reComboboxQC.SetValue.Trim());
    }
    private async void ComboboxSelectIndexChangeTC()
    {
      //if (OnSendSaveTC != null)
      //  OnSendSaveTC(this, reComboboxTC.SetValue.Trim());
      ////await AppCore.Ins.ClearActiveUser("TC");
      ////await AppCore.Ins.ActiveUser(reComboboxTC.SetValue.Trim(), "TC");

      //AppCore.Ins.ReloadUser();

      //AppCore.Ins.LogActiveAppToFileLog("Thay đổi tên TC:" + reComboboxTC.SetValue.Trim());
    }



    private void ComboboxSelectIndexChangeFGs()
    {
      //FrmConfirm frmConfirm = new FrmConfirm($"Xác nhận Change Over: {reComboboxFGs.SetValue} !\n {AppCore.Ins._listMasterData?.Where(s => s.FGs == this.reComboboxFGs.SelectedItem).Select(s => s.Description).FirstOrDefault()}", eImage.Question);
      //frmConfirm.OnSendOKClicked += FrmConfirm_OnSendOKClicked;
      //frmConfirm.Show();
    }

    private void FrmConfirm_OnSendOKClicked(object sender)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          FrmConfirm_OnSendOKClicked(sender);
        }));
        return;
      }
      try
      {
        //AppCore.Ins.isChangeOver = true;
        //string fgs = this.reComboboxFGs.SelectedItem.Trim();

        //if (fgs != "")
        //{
        //  string NameProduct = AppCore.Ins._listMasterData?.Where(s => s.FGs == fgs).Select(s => s.Description).FirstOrDefault();
        //  double NormalSpeed = 0;

        //  if (NameProduct == null || NameProduct == "")
        //  {
        //    this.ucTextBoxProductName.Texts = "NAN";
        //  }
        //  else
        //  {
        //    this.ucTextBoxProductName.Texts = NameProduct;
        //    NormalSpeed = AppCore.Ins._listMasterData.Where(s => s.FGs == fgs).Select(s => s.NormalSpeed).FirstOrDefault();
        //  }

        //  if (OnSendChangeOver != null)
        //  {
        //    OnSendChangeOver(this, fgs, this.ucTextBoxProductName.Texts.Trim(), NormalSpeed);
        //  }
        //}


        //ucTextBoxProductName.Texts = AppCore.Ins._listMasterData?.Where(s => s.FGs == this.reComboboxFGs.SelectedItem).Select(s => s.Description).FirstOrDefault();

        //UpdateInforProduct();

        //AppCore.Ins.isChangeOver = false;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        ResetDashboard();
        //AppCore.Ins.LogActiveAppToFileLog("Thay đổi sản phẩm:" + this.reComboboxFGs.SelectedItem);
      }
    }

    private void ResetDashboard()
    {
      //if (this.InvokeRequired)
      //{
      //  this.Invoke(new Action(() =>
      //  {
      //    ResetDashboard();
      //  }));
      //  return;
      //}

      //try
      //{
      //  list.Clear();
      //  valueNetPass = list.Where(s => s.Status == "Ok" || s.Status == "Over").Select(x => x.Net).ToList();
      //  valueNetOk = list.Where(s => s.Status == "Ok").Select(x => x.Net).ToList();
      //  valueNetOver = list.Where(s => s.Status == "Over").Select(x => x.Net).ToList();
      //  valueNetReject = list.Where(s => s.Status == "Reject").ToList();
      //  dataTimeData = list.Where(s => s.Status == "Ok" || s.Status == "Over").Select(x => x.CreatedAt.ToString()).ToList();

      //  dataRejects = new List<DataReject>();
      //  foreach (var data in valueNetReject)
      //  {
      //    DataReject dataReject = new DataReject();
      //    dataReject.DateTime = (DateTime)data.CreatedAt;
      //    dataReject.FGs = ProductFGs;
      //    dataReject.Actual = data.Net;
      //    dataReject.Target = ProductTarget;
      //    dataRejects.Add(dataReject);
      //  }

      //  NumbersOk = (double)valueNetOk.Count;
      //  NumbersOver = (double)valueNetOver.Count;
      //  NumbersReject = (double)valueNetReject.Count;

      //  CntIn = list.Count();
      //  CntOut = valueNetPass.Count();
      //  CntReject = (int)NumbersReject;

      //  Sample = valueNetPass.Count;
      //  Mean = (valueNetPass.Count == 0) ? 0 : CalMean(valueNetPass);
      //  Std = (valueNetPass.Count == 0) ? 0 : CalStdDev(valueNetPass);
      //  MinValue = (valueNetPass.Count == 0) ? 0 : valueNetPass.Min();
      //  MaxValue = (valueNetPass.Count == 0) ? 0 : valueNetPass.Max();

      //  Cp = (Std != 0) ? Math.Round(((MaxValue - MinValue) / (6 * Std)), 3) : 0;
      //  double hcpk = (Std != 0) ? ((MaxValue - Mean) / (3 * Std)) : 0;
      //  double lcpk = (Std != 0) ? ((Mean - MinValue) / (3 * Std)) : 0;
      //  Cpk = Math.Round(Math.Min(hcpk, lcpk), 3);
      //  OW = (ProductTarget != 0) ? Math.Round(((Mean - ProductTarget) / ProductTarget) * 100, 2) : 0;
      //  OW = (Mean == 0) ? 0 : OW;

      //  if (OnSendKawaTV != null)
      //    OnSendKawaTV(this, (ulong)(OW * 100), (ulong)valueNetPass.Count());

      //  UpdateDataUI(true);
      //}
      //catch (Exception)
      //{
      //}

    }

    private void ResetUser()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetUser();
        }));
        return;
      }

      //this.reComboboxOP.ClearCb();
      //this.reComboboxQC.ClearCb();
      //this.reComboboxTC.ClearCb();

      //this.ucTextBoxLoBB.Texts = "";
      //Properties.Settings.Default.LoBB = "";
      //Properties.Settings.Default.Save();
      //if (OnSendSaveLoBB != null)
      //{
      //  OnSendSaveLoBB(this, this.ucTextBoxLoBB.Texts);
      //}
    }

    private bool isVisible = false;
    private bool isLoBBSendPLC = true;
    private void WarningUser()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          WarningUser();
        }));
        return;
      }

      isVisible = !isVisible;



      //if (reComboboxOP.SelectedItem == "")
      //{
      //  lbWarningOP.Visible = isVisible;
      //}
      //else
      //{
      //  lbWarningOP.Visible = false;
      //}

      //if (reComboboxQC.SelectedItem == "")
      //{
      //  lbWarningQC.Visible = isVisible;
      //}
      //else
      //{
      //  lbWarningQC.Visible = false;
      //}

      //if (reComboboxTC.SelectedItem == "")
      //{
      //  lbWarningTC.Visible = isVisible;
      //}
      //else
      //{
      //  lbWarningTC.Visible = false;
      //}


      //if (ucTextBoxLoBB.Texts.Trim() == "" || isLoBBSendPLC == false)
      //{
      //  this.lbLOBBE.Visible = isVisible;
      //  this.lbLOBBVN.Visible = isVisible;
      //  this.lbLOBBE.ForeColor = Color.Yellow;
      //  this.lbLOBBVN.ForeColor = Color.Yellow;
      //  this.isLoBBSendPLC = false;
      //}
      //else
      //{
      //  this.lbLOBBE.Visible = true;
      //  this.lbLOBBVN.Visible = true;
      //  this.lbLOBBE.ForeColor = Color.White;
      //  this.lbLOBBVN.ForeColor = Color.White;
      //}

    }

    private int GetShiftByHour(int hour)
    {
      if (hour >= 6 && hour < 14) return 1;
      else if (hour >= 14 && hour < 22) return 2;
      else return 3;
    }

    private void UpdateInforProduct()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          UpdateInforProduct();
        }));
        return;
      }

      //MasterData masterData = new MasterData();
      //masterData = AppCore.Ins._masterDataCurrent;
      //if (masterData != null)
      //{
      //  ProductMin = masterData.Min;
      //  ProductMax = masterData.Max;
      //  ProductLowerControl = masterData.LowerControl;
      //  ProductUpperControl = masterData.UpperControl;
      //  ProductTarget = masterData.Target;
      //  ProductValueT = masterData.ValueT;

      //  ProductFGs = masterData.FGs;
      //  ProductName = masterData.Description;
      //  ProductSKU = masterData.SKU;

      //  ProductId = masterData.Id;

      //  this.lbKhoiLuongRieng.Text = ProductSKU;
      //  this.lb1T.Text = ProductValueT.ToString();
      //  this.lbTLMin.Text = ProductMin.ToString();
      //  this.lbTLMax.Text = ProductMax.ToString();
      //  this.lbUpperControl.Text = ProductUpperControl.ToString();
      //  this.lbLowerControl.Text = ProductLowerControl.ToString();
      //  this.lbTarget.Text = ProductTarget.ToString();

      //  //this.ucTextBoxFGs.Texts = ProductFGs.ToString();
      //  this.ucTextBoxProductName.Texts = ProductName;
      //}
    }

    private void LoadInfoLine()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          LoadInfoLine();
        }));
        return;
      }

      //reComboboxOP.SetDataSource
      //this.reComboboxOP.SetDataSource = AppCore.Ins._users?.Where(s => s.Role == "OP").Select(x => x.Name).ToList();
      //this.reComboboxQC.SetDataSource = AppCore.Ins._users?.Where(s => s.Role == "QC").Select(x => x.Name).ToList();
      //this.reComboboxTC.SetDataSource = AppCore.Ins._users?.Where(s => s.Role == "TC").Select(x => x.Name).ToList();

      //this.reComboboxOP.SelectedItem = AppCore.Ins._users?.Where(s => s.Role == "OP" & s.isEnable == true).Select(x => x.Name).FirstOrDefault();
      //this.reComboboxQC.SelectedItem = AppCore.Ins._users?.Where(s => s.Role == "QC" & s.isEnable == true).Select(x => x.Name).FirstOrDefault();
      //this.reComboboxTC.SelectedItem = AppCore.Ins._users?.Where(s => s.Role == "TC" & s.isEnable == true).Select(x => x.Name).FirstOrDefault();

      //List<string> list = AppCore.Ins._listMasterData?.Where(x => x.isDelete == false).Select(x => x.FGs).ToList();
      //if (list != null)
      //{
      //  this.reComboboxFGs.SetDataSource = list;
      //  this.reComboboxFGs.SelectedItem = AppCore.Ins._listMasterData?.Where(s => s.isEnable == true && s.isDelete == false).Select(x => x.FGs).FirstOrDefault();
      //  this.ucTextBoxProductName.Text = AppCore.Ins._listMasterData?.Where(s => s.FGs == this.reComboboxFGs.SelectedItem).Select(s => s.Description).FirstOrDefault();
      //}

      ////this.ucTextBoxOP.Texts = AppCore.Ins._users.Where(s=>s.Role=="OP" & s.isEnable==true).Select(s=>s.Name).FirstOrDefault();
      ////this.ucTextBoxQC.Texts = AppCore.Ins._users.Where(s => s.Role == "QC" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
      ////this.ucTextBoxTC.Texts = AppCore.Ins._users.Where(s => s.Role == "TC" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
      //this.ucTextBoxLoBB.Texts = Properties.Settings.Default.LoBB;
    }

    private int Sample = 0;
    private double NumbersOk = 0;
    private double NumbersOver = 0;
    private double NumbersReject = 0;
    private double Mean = 0;
    private double Std = 0;
    private double MinValue = 0;
    private double MaxValue = 0;
    private double Cp = 0;
    private double Cpk = 0;
    private double OW = 0;

    private int CntIn;
    private int CntOut;
    private int CntReject;



    private List<double> valueNetPass = new List<double>();
    private List<double> valueNetOk = new List<double>();
    private List<double> valueNetOver = new List<double>();
    private List<Datalog> valueNetReject = new List<Datalog>();
    //private List<DataReject> dataRejects = new List<DataReject>();

    private List<string> dataTimeData = new List<string>();
    private bool isUpdateChart = false;

    private int k = 0;
    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      this.timer_UpdateUI.Stop();

      try
      {
        LoadDataDashBoard();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        timer_UpdateUI.Start();
      }
    }

    private void Test()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          Test();
        }));
        return;
      }

      chartControl.Series[0].Points.AddXY(k, 0);

      chartControl.Series[1].Points.AddXY(0, ProductMax);
      chartControl.Series[1].Points.AddXY(list.Count() - 1 + k, ProductMax);

      chartControl.Series[2].Points.AddXY(0, ProductUpperControl);
      chartControl.Series[2].Points.AddXY(list.Count() - 1 + k, ProductUpperControl);

      chartControl.Series[3].Points.AddXY(0, ProductTarget);
      chartControl.Series[3].Points.AddXY(list.Count() - 1 + k, ProductTarget);

      chartControl.Series[4].Points.AddXY(0, ProductLowerControl);
      chartControl.Series[4].Points.AddXY(list.Count() - 1 + k, ProductLowerControl);

      chartControl.Series[5].Points.AddXY(0, ProductMin);
      chartControl.Series[5].Points.AddXY(list.Count() - 1 + k, ProductMin);


    }






    private List<Datalog> list = new List<Datalog>();
    private List<DataRejectDTO> reject = new List<DataRejectDTO>();
    private SumaryDTO sumaryDTO = new SumaryDTO();

    private void LoadDataDashBoard()
    {
      try
      {
        if (AppCore.Ins._datalogsInShiftCurrent == null || AppCore.Ins._datalogsInShiftCurrent.Count == 0)
        {
          //ResetDashBoard();
          return;
        }

        var list = AppCore.Ins._datalogsInShiftCurrent;
        valueNetPass = list.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept || s.EnumStatusRecord == EnumStatusRecord.Over).Select(x => x.Net).ToList();
        valueNetOk = list.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept).Select(x => x.Net).ToList();
        valueNetOver = list.Where(s => s.EnumStatusRecord == EnumStatusRecord.Over).Select(x => x.Net).ToList();
        valueNetReject = list.Where(s => s.EnumStatusRecord == EnumStatusRecord.Reject).ToList();
        dataTimeData = list.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept || s.EnumStatusRecord == EnumStatusRecord.Over).Select(x => x.CreatedAt.ToString()).ToList();

        reject = new List<DataRejectDTO>();
        double target = AppCore.Ins._productCurrent?.Target ?? 0.0 +
          AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0 +
          AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0;
        foreach (var data in valueNetReject)
        {
          DataRejectDTO dataReject = new DataRejectDTO();
          dataReject.DateTime = (DateTime)data.CreatedAt;
          dataReject.FGs = AppCore.Ins._productCurrent.Code;
          dataReject.Actual = data.Net;
          dataReject.Target = target;
          reject.Add(dataReject);
        }

        //
        sumaryDTO = SumaryDTOData(list);

        NumbersOk = (double)valueNetOk.Count;
        NumbersOver = (double)valueNetOver.Count;
        NumbersReject = (double)valueNetReject.Count;

        //CntIn = list.Count();
        //CntOut = valueNetPass.Count();
        //CntReject = (int)NumbersReject;

        //Sample = valueNetPass.Count;
        //Mean = (Sample == 0) ? 0 : CalMean(valueNetPass);
        //Std = (Sample == 0) ? 0 : CalStdDev(valueNetPass);
        //MinValue = (Sample == 0) ? 0 : valueNetPass.Min();
        //MaxValue = (Sample == 0) ? 0 : valueNetPass.Max();

        //Cp = (Std != 0) ? Math.Round(((MaxValue - MinValue) / (6 * Std)), 3) : 0;
        //double hcpk = (Std != 0) ? ((MaxValue - Mean) / (3 * Std)) : 0;
        //double lcpk = (Std != 0) ? ((Mean - MinValue) / (3 * Std)) : 0;
        //Cpk = Math.Round(Math.Min(hcpk, lcpk), 3);
        //OW = (ProductTarget != 0) ? Math.Round(((Mean - ProductTarget) / ProductTarget) * 100, 2) : 0;
        //OW = (Mean == 0) ? 0 : OW;


        UpdateDataUI(true);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
      }
    }


    private SumaryDTO SumaryDTOData(List<Datalog> datalogs)
    {
      SumaryDTO sumaryDTO = new SumaryDTO();
      var listDataOk = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept || s.EnumStatusRecord == EnumStatusRecord.Over).Select(x => x.Net).ToList();

      NumbersOk = (double)valueNetOk.Count;
      NumbersOver = (double)valueNetOver.Count;
      NumbersReject = (double)valueNetReject.Count;

      CntIn = list.Count();
      CntOut = valueNetPass.Count();
      CntReject = (int)NumbersReject;

      sumaryDTO.Sample = listDataOk.Count;
      sumaryDTO.Mean = (sumaryDTO.Sample == 0) ? 0 : CalMean(listDataOk);

      double Std = (sumaryDTO.Sample == 0) ? 0 : CalStdDev(listDataOk);
      sumaryDTO.Min = (sumaryDTO.Sample == 0) ? 0 : listDataOk.Min();
      sumaryDTO.Max = (sumaryDTO.Sample == 0) ? 0 : listDataOk.Max();

      sumaryDTO.Cp = (Std != 0) ? Math.Round(((MaxValue - MinValue) / (6 * Std)), 3) : 0;
      double hcpk = (Std != 0) ? ((MaxValue - Mean) / (3 * Std)) : 0;
      double lcpk = (Std != 0) ? ((Mean - MinValue) / (3 * Std)) : 0;
      sumaryDTO.Cpk = Math.Round(Math.Min(hcpk, lcpk), 3);
      sumaryDTO.OW = (ProductTarget != 0) ? Math.Round(((Mean - ProductTarget) / ProductTarget) * 100, 2) : 0;
      sumaryDTO.OW = (Mean == 0) ? 0 : OW;
      sumaryDTO.Stdev = Std;
      return sumaryDTO;
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

      //this.lbSample.Text = Sample.ToString();
      //this.lbXtb.Text = Mean.ToString();
      //this.lbMin.Text = MinValue.ToString();
      //this.lbMax.Text = MaxValue.ToString();
      //this.lbCp.Text = Cp.ToString();
      //this.lbCpk.Text = Cpk.ToString();
      //this.lbOWDB.Texts = OW.ToString() + " %";
      //this.lbTLTBDB.Texts = Mean.ToString() + " g";

      //this.lbOWDB.BackColor = (OW >= 0.5) ? Color.Red : Color.FromArgb(40, 167, 68);
      //this.lbResult.BackColor = (Mean < ProductTarget) ? Color.Red : Color.FromArgb(40, 167, 68);
      //this.lbResult.Text = (Mean < ProductTarget) ? "FAIL" : "PASS";

      //this.txtCntIn.Texts = CntIn.ToString();
      //this.txtCntOut.Texts = CntOut.ToString();
      //this.txtCntReject.Texts = CntReject.ToString();

      if (isUpdateChart)
      {
        ProductMax = (AppCore.Ins._productCurrent?.USL ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0);
        ProductUpperControl = (AppCore.Ins._productCurrent?.UCL ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0);
        ProductTarget = (AppCore.Ins._productCurrent?.Target ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0);
        ProductLowerControl = (AppCore.Ins._productCurrent?.LCL ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0);
        ProductMin = (AppCore.Ins._productCurrent?.LSL ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Carton ?? 0.0) +
         (AppCore.Ins._tareSettingCurrent?.Tube ?? 0.0);


        _dataChart.AddChartControlDashboard(chartControl, valueNetPass, dataTimeData, ProductMax, ProductUpperControl, ProductTarget, ProductLowerControl, ProductMin, MaxValue);
        _dataChart.AddChartHistogram(chartHistogram, valueNetPass, ProductMax, ProductUpperControl, sumaryDTO.Mean, sumaryDTO.Stdev, ProductLowerControl, ProductMin, sumaryDTO.Min, sumaryDTO.Max, ProductTarget);
        ucChartPie1.SetDataChartPie(NumbersOk, NumbersOver, NumbersReject);
      }

      ucInformationDataSumary1.SetSumaryDTO(sumaryDTO);
      SetDataOW_Mean(sumaryDTO);
      UpdateDataReject(reject);
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

            dgvReject.Rows.Add(timeOnly, item.FGs, item.Actual, item.Target);
          }
          numberRejectLast = dataRejects.Count();

          ucInformationLoss1.ValueReject = numberRejectLast.ToString();
        }


      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
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
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }


    #region Cal Tính toán Mean, Std
    private double CalMean(List<double> list_data)
    {
      double x_tb = 0;
      foreach (var item in list_data)
      {
        x_tb += item;
      }
      return Math.Round(x_tb / list_data.Count, 2);
    }

    private double CalStdDev(List<double> list_data)
    {
      double mean_x_tb = CalMean(list_data);
      double sumOfSquares = 0;
      foreach (double data_x in list_data)
        sumOfSquares += Math.Pow(data_x - mean_x_tb, 2);
      double stdDev = Math.Sqrt(sumOfSquares / (list_data.Count - 1));
      return Math.Round(stdDev, 2);
    }

    private double CalNormalDensityProbabilityFunction(double x_value, double mean, double stdDev)
    {
      double ret = 0;
      ret = (stdDev != 0) ? (1.0 / (stdDev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-(Math.Pow(x_value - mean, 2) / (2 * Math.Pow(stdDev, 2)))) : 0;
      return ret;
    }

    #endregion

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
      lbCarton.ValueStr = tareSetting?.Carton.ToString() ?? string.Empty;
    }
  }
}
