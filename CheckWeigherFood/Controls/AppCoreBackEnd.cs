using CheckWeigherFood.FrmChild;
using CheckWeigherFood.PLC;
using Database.DTO;
using Database.Models;
using IoTClient.Clients.PLC;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Database.Enum;
using Application = System.Windows.Forms.Application;
using DateTime = System.DateTime;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    private static AppCore _ins = new AppCore();
    public static AppCore Ins
    {
      get
      {
        return _ins == null ? _ins = new AppCore() : _ins;
      }
    }

    public delegate void SendStatusPLC(object sender, bool isConnect);
    public event SendStatusPLC OnSendStatus;

    public delegate void SendAutoReport(object sender, int shiftId, int productId);
    public event SendAutoReport OnSendAutoReport;

    public delegate void SendReSetInforShift();
    public event SendReSetInforShift OnSendReSetInforShift;

    private System.Timers.Timer timerCheckChangeShift = new System.Timers.Timer();
    private System.Timers.Timer timerCheckData = new System.Timers.Timer();
    public void Init()
    {
      ResgisterService();

      InitLoadDataStartApp().Wait();

      InitModbus();

      //Đăng kí sự kiện thao tác trong UI
      InitEvent();

      //Init Report tự động mỗi khi chuyển ca
      InitReportAuto();

      StartShowUI();
    }

    public List<Datalog> _datalogsInShiftCurrent_Line3 = new List<Datalog>();
    public List<Datalog> _datalogsInShiftCurrent_Line4 = new List<Datalog>();
    public OperationSetting _operationSettingCurrent03 { get; set; }
    public OperationSetting _operationSettingCurrent04 { get; set; }


    public List<Product> _products { get; set; }
    public Product _productCurrent03 { get; set; }
    public Product _productCurrent04 { get; set; }


    public TareSetting _tareSettingCurrent03 { get; set; }
    public TareSetting _tareSettingCurrent04 { get; set; }

    public Machine _machineCurrent03 { get; set; }
    public Machine _machineCurrent04 { get; set; }


    private async Task InitLoadDataStartApp()
    {
      var listMachine = await _machineService.GetDataAsync();
      _machineCurrent03 = listMachine?.FirstOrDefault(x => x.KeyMachine == 3);
      _machineCurrent04 = listMachine?.FirstOrDefault(x => x.KeyMachine == 4);

      _operationSettingCurrent03 = await _operationSettingService.GetFirstDataAsync(3);
      _operationSettingCurrent04 = await _operationSettingService.GetFirstDataAsync(4);

      _tareSettingCurrent03 = await _tareSettingService.GetFirstDataAsync(3);
      _tareSettingCurrent04 = await _tareSettingService.GetFirstDataAsync(4);

      _products = await _productService.GetAllAsync();
      _productCurrent03 = _products?.Where(x => x.Id == _machineCurrent03.ProductId).FirstOrDefault();
      _productCurrent04 = _products?.Where(x => x.Id == _machineCurrent04.ProductId).FirstOrDefault();

      var shift = GetCurrentShift(DateTime.Now);
      if (_productCurrent03!=null && _machineCurrent03!=null)
        _datalogsInShiftCurrent_Line3 = await _datalogService.GetAllDataByTimeAsync(shift.StartTime, shift.EndTime, _productCurrent03.Id, _machineCurrent03.ChangeOverId);

      if (_productCurrent04 != null && _machineCurrent04 != null)
        _datalogsInShiftCurrent_Line4 = await _datalogService.GetAllDataByTimeAsync(shift.StartTime, shift.EndTime, _productCurrent04.Id, _machineCurrent04.ChangeOverId);
    }

    public async Task ReloadMasterData()
    {
      _products = await _productService.GetAllAsync();
      //_productCurrent04 = _products?.Where(x => x.Id == _appConfig.ProductId).FirstOrDefault();
    }

    
    public static ShiftInfo GetCurrentShift(DateTime now)
    {
      DateTime today = now.Date;

      DateTime shift1Start = today.AddHours(6);   // 06:00
      DateTime shift1End = today.AddHours(14);    // 14:00

      DateTime shift2Start = today.AddHours(14);  // 14:00
      DateTime shift2End = today.AddHours(22);    // 22:00

      // Ca 1
      if (now >= shift1Start && now < shift1End)
      {
        return new ShiftInfo
        {
          Shift = 1,
          StartTime = shift1Start,
          EndTime = shift1End.AddSeconds(-1)
        };
      }

      // Ca 2
      if (now >= shift2Start && now < shift2End)
      {
        return new ShiftInfo
        {
          Shift = 2,
          StartTime = shift2Start,
          EndTime = shift2End.AddSeconds(-1)
        };
      }

      // Ca 3
      // 22:00 -> 05:59 hôm sau
      if (now.Hour >= 22)
      {
        return new ShiftInfo
        {
          Shift = 3,
          StartTime = today.AddHours(22),
          EndTime = today.AddDays(1).AddHours(6).AddSeconds(-1)
        };
      }
      else
      {
        return new ShiftInfo
        {
          Shift = 3,
          StartTime = today.AddDays(-1).AddHours(22),
          EndTime = today.AddHours(6).AddSeconds(-1)
        };
      }
    }

    private void InitReportAuto()
    {
      _shiftCurrent = GetShiftByHour(DateTime.Now.Hour);
      _shiftLast = _shiftCurrent;
      timerCheckChangeShift.Interval = 1000;
      timerCheckChangeShift.Elapsed += Timer_Report_Auto_Elapsed; ;
      timerCheckChangeShift.Start();
    }

    private int _shiftLast = 0;
    public int _shiftCurrent = 0;
    private bool testChangeShift = false;
    public void ChangeShiftTest()
    {
      //_datalogsInShiftCurrent = new List<Datalog>();
      //_shiftCurrent = GetShiftByHour(DateTime.Now.Hour);
      //OnSendAutoReport(this, _shiftLast, _shiftCurrent);
    }

    public bool _testChangeShift = false;
    private void Timer_Report_Auto_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      timerCheckChangeShift.Stop();
      try
      {
        _shiftCurrent = GetShiftByHour(DateTime.Now.Hour);
        if (_shiftCurrent != _shiftLast || _testChangeShift == true)
        {
          _shiftLast = _shiftCurrent;
          if (OnSendAutoReport != null)
          {
            //OnSendAutoReport(this, shift_last, _masterDataCurrent.Id);
            ResetOperator();
            OnSendReSetInforShift?.Invoke();
          }

          //_datalogsInShiftCurrent.Clear();

          _testChangeShift = false;
        }
      }
      catch (Exception)
      {
      }
      finally { timerCheckChangeShift.Start(); }
    }

    private async void ResetOperator()
    {
      OperationSetting operationSetting03 = new OperationSetting();
      operationSetting03.OP = "";
      operationSetting03.QC = "";
      operationSetting03.ShiftLeader = "";
      operationSetting03.KeyMachine = 3;
      operationSetting03.CreatedAt = DateTime.UtcNow;
      AppCore.Ins._operationSettingCurrent03 = await _operationSettingService.AddAsync(operationSetting03);

      OperationSetting operationSetting04 = new OperationSetting();
      operationSetting04.OP = "";
      operationSetting04.QC = "";
      operationSetting04.ShiftLeader = "";
      operationSetting04.KeyMachine = 4;
      operationSetting04.CreatedAt = DateTime.UtcNow;
      AppCore.Ins._operationSettingCurrent04 = await _operationSettingService.AddAsync(operationSetting04);
    }


    private void InitEvent()
    {
      //FrmDashboard.Instance.OnSendChangeOver += Instance_OnSendChangeOver;
      //Tạo timer load data 2s
      timerCheckData.Interval = 2000;
      timerCheckData.Elapsed += Timer_UpdateUI_Elapsed;
      timerCheckData.Start();

      FrmMasterData.Instance.OnSendMasterDataChanged += Instance_OnSendMasterDataChanged;
    }

    public SumaryDTO _sumaryDTOLine3 { get; set; }
    public SumaryDTO _sumaryDTOLine4 { get; set; }
    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        timerCheckData.Stop();
        if (AppCore.Ins._datalogsInShiftCurrent_Line3?.Count() > 0)
        {
          _sumaryDTOLine3 = AppCore.Ins.SumaryDTOData(_datalogsInShiftCurrent_Line3, AppCore.Ins._productCurrent03, AppCore.Ins._tareSettingCurrent03);
        }
        else
        {
          _sumaryDTOLine3 = new SumaryDTO();
        }

        if (AppCore.Ins._datalogsInShiftCurrent_Line4?.Count() > 0)
        {
          _sumaryDTOLine4 = AppCore.Ins.SumaryDTOData(_datalogsInShiftCurrent_Line4, AppCore.Ins._productCurrent04, AppCore.Ins._tareSettingCurrent04);
        }
        else
        {
          _sumaryDTOLine4 = new SumaryDTO();
        }  
      }
      catch (Exception)
      {

      }
      finally
      {
        timerCheckData.Start();
      }
    }

    private async void Instance_OnSendMasterDataChanged()
    {
      await ReloadMasterData();
    }

    public MitsubishiClient _client;
    public System.Timers.Timer timer_checkReadtPLC = new System.Timers.Timer();
    private System.Timers.Timer timer_checkConnectPLC = new System.Timers.Timer();
    private bool StatusConnectCurrent = false;
    private FunstionPLC _funstionPLC = new FunstionPLC();

    private Random rd = new Random();
    public bool isChangeOver = false;

    public int GetShiftByHour(int hour)
    {
      if (hour >= 6 && hour < 14) return 1;
      else if (hour >= 14 && hour < 22) return 2;
      else return 3;
    }

    public void LogErrorToFileLog(string content)
    {
      string NameFileLog = Application.StartupPath + $"\\logError.txt";
      if (!File.Exists(NameFileLog))
      {
        File.Create(NameFileLog);
      }
      string contentLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + $": Content: {content} \r\n";
      File.AppendAllText(NameFileLog, contentLog);
    }

    public void LogActiveAppToFileLog(string content)
    {
      string NameFileLog = Application.StartupPath + $"\\logActive.txt";
      if (!File.Exists(NameFileLog))
      {
        File.Create(NameFileLog);
      }
      string contentLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + $": Content: {content} \r\n";
      File.AppendAllText(NameFileLog, contentLog);
    }


    #region Cal Tính toán Mean, Std
    public SumaryDTO SumaryDTOData(List<Datalog> datalogs, Product product, TareSetting tare)
    {
      SumaryDTO sumaryDTO = new SumaryDTO();

      if (datalogs?.Count()>0)
      {
        double USL = (product?.USL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double LSL = (product?.LSL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double UCL = (product?.UCL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double LCL = (product?.LCL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double target = (product?.Target ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);

        sumaryDTO.Product = product;
        sumaryDTO.USL = USL;
        sumaryDTO.UCL = UCL;
        sumaryDTO.Target = target;
        sumaryDTO.targetSrc = (product?.Target ?? 0.0);
        sumaryDTO.LCL = LCL;
        sumaryDTO.LSL = LSL;

        double dataFilter = target * 1.5;
        datalogs = datalogs?.Where(x => x.Gross <= dataFilter).ToList();

        sumaryDTO.DatalogPass = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept || s.EnumStatusRecord == EnumStatusRecord.Over).ToList();
        sumaryDTO.DatalogOver = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Over).ToList();
        sumaryDTO.DatalogAccept = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept).ToList();
        sumaryDTO.DatalogReject = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Reject).ToList();

        var dataGrossPass = sumaryDTO.DatalogPass.Select(x => x.Gross).ToList();
        sumaryDTO.Sample = dataGrossPass.Count;
        sumaryDTO.Mean = (sumaryDTO.Sample == 0) ? 0 : CalMean(dataGrossPass);

        double Std = (sumaryDTO.Sample == 0) ? 0 : CalStdDev(dataGrossPass);
        sumaryDTO.Stdev = Std;
        sumaryDTO.Min = (sumaryDTO.Sample == 0) ? 0 : dataGrossPass.Min();
        sumaryDTO.Max = (sumaryDTO.Sample == 0) ? 0 : dataGrossPass.Max();

        sumaryDTO.Cp = (Std != 0) ? Math.Round(((USL - LSL) / (6 * Std)), 3) : 0;

        double hcpk = (Std != 0) ? ((USL - sumaryDTO.Mean) / (3 * Std)) : 0;
        double lcpk = (Std != 0) ? ((sumaryDTO.Mean - LSL) / (3 * Std)) : 0;
        sumaryDTO.Cpk = Math.Round(Math.Min(hcpk, lcpk), 3);

        //sumaryDTO.OW = (target != 0 && sumaryDTO.Mean > target) ? Math.Round(((sumaryDTO.Mean - target) / target) * 100, 2) : 0;
        sumaryDTO.OW = Math.Round(((sumaryDTO.Mean - target) / target) * 100, 2);

        if (sumaryDTO.Mean >= target)
        {
          sumaryDTO.EnumResult = EnumResult.Pass;
        }
        else
        {
          sumaryDTO.EnumResult = EnumResult.Fail;
        }

        ////Kết quả
        //if (product.IsAbsolute)
        //{
        //  bool rule01 = datalogs?.Any(x => x.Net < target) ?? false;
        //  if (!rule01)
        //  {
        //    sumaryDTO.EnumResult = EnumResult.Pass;
        //  }
        //  else
        //  {
        //    sumaryDTO.EnumResult = EnumResult.Fail;
        //    sumaryDTO.ReasonFail.Add("Có mẫu < target theo luật TL Tuyệt đối");
        //  }
        //}
        //else
        //{
        //  //Số mẫu có khối lượng thuộc [Target - 2T, Target - T) không được vượt quá 2,5 %  cỡ lô
        //  var cnt = datalogs?.Count(x => x.Net < LCL && x.Net >= LSL);
        //  double value = (((double)cnt) * 100.0) / (double)datalogs.Count();
        //  bool rule01 = value <= 2.5;
        //  if (!rule01)
        //  {
        //    sumaryDTO.ReasonFail.Add("Số mẫu có khối lượng thuộc [LSL, LCL) vượt quá 2,5 % cỡ lô");
        //  }

        //  //Ko có mẫu nào nhỏ hơn giá trị (Target -2T)
        //  bool rule02 = !(datalogs?.Any(x => x.Net < LSL) ?? true);
        //  if (!rule02)
        //  {
        //    sumaryDTO.ReasonFail.Add("Có mẫu < LSL ");
        //  }

        //  //Ttb >= Target
        //  bool rule03 = sumaryDTO.Mean >= target;
        //  if (!rule03)
        //  {
        //    sumaryDTO.ReasonFail.Add("TL trung bình < target");
        //  }

        //  if (rule01 && rule02 && rule03)
        //  {
        //    sumaryDTO.EnumResult = EnumResult.Pass;
        //  }
        //  else
        //  {
        //    sumaryDTO.EnumResult = EnumResult.Fail;
        //  }
        //}
      }
      else
      {
        double USL = (product?.USL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double LSL = (product?.LSL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double UCL = (product?.LCL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double LCL = (product?.UCL ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);
        double target = (product?.Target ?? 0.0) + (tare?.Tube ?? 0.0) - (tare?.TailTube ?? 0.0) + (tare?.Carton ?? 0.0);

        sumaryDTO.USL = USL;
        sumaryDTO.UCL = UCL;
        sumaryDTO.Target = target;
        sumaryDTO.LCL = LCL;
        sumaryDTO.LSL = LSL;

        sumaryDTO.DatalogPass = new List<Datalog>() ;
        sumaryDTO.DatalogOver = new List<Datalog>();
        sumaryDTO.DatalogAccept = new List<Datalog>();
        sumaryDTO.DatalogReject = new List<Datalog>();

        sumaryDTO.Sample = 0;
        sumaryDTO.Mean = 0;

        sumaryDTO.Stdev = 0;
        sumaryDTO.Min = 0;
        sumaryDTO.Max = 0;

        sumaryDTO.Cp = 0;
        sumaryDTO.Cpk = 0;

        sumaryDTO.OW = 0;

        //Kết quả
        sumaryDTO.EnumResult = EnumResult.None;
      }  

      return sumaryDTO;
    }

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


  }

  public class ShiftInfo
  {
    public int Shift { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
  }
}
