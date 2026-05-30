using CheckWeigherFood.FrmChild;
using CheckWeigherFood.PLC;
using Database.DTO;
using Database.Models;
using IoTClient.Clients.PLC;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

    private System.Timers.Timer timer_Report_Auto = new System.Timers.Timer();
    public void Init()
    {
      ResgisterService();

      InitLoadDataStartApp().Wait();

      //Init_OPC_UA();

      //Đăng kí sự kiện thao tác trong UI
      InitEvent();

      //Init Report tự động mỗi khi chuyển ca
      InitReportAuto();

      StartShowUI();
    }

    public List<Datalog> _datalogsInShiftCurrent = new List<Datalog>();
    public OperationSetting _operationSettingCurrent { get; set; }
    public AppConfig _appConfig { get; set; }
    public Product _productCurrent { get; set; }
    public TareSetting _tareSettingCurrent { get; set; }
    public Machine _machineCurrent { get; set; }
    private async Task InitLoadDataStartApp()
    {
      _machineCurrent = await _machineService.GetFirstlDataAsync();

      _operationSettingCurrent = await _operationSettingService.GetFirstDataAsync();
      _appConfig = await _appConfigService.GetFirstlDataAsync();
      _tareSettingCurrent = await _tareSettingService.GetFirstDataAsync();
      

      if (_appConfig?.ProductId > 0)
      {
        _productCurrent = await _productService.GetDataByIdAsync(_appConfig.ProductId);

        var shift = GetCurrentShift(DateTime.Now);
        DateTime startUtc = shift.StartTime.ToUniversalTime();
        DateTime endUtc = shift.EndTime.ToUniversalTime();
        _datalogsInShiftCurrent = await _datalogService.GetAllDataByTimeAsync(startUtc, endUtc, _productCurrent.Id);
      }
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

    private async void LoadDataDB()
    {
      //try
      //{
      //  ProductId = _masterDataCurrent?.Id ?? 0;

      //  datalogsDB = new List<Datalog>();
      //  DateTime dt = DateTime.Now;
      //  int HourCurrent = dt.Hour;

      //  int shiftCurrent = GetShiftByHour(HourCurrent);

      //  string pathDatabase = Application.StartupPath + $"\\DataBase\\";
      //  string fileDB = (HourCurrent >= 0 && HourCurrent < 6) ? dt.AddDays(-1).ToString("yyMMdd") : dt.ToString("yyMMdd");

      //  string pathFull = pathDatabase + fileDB + ".sqlite";
      //  if (File.Exists(pathFull))
      //    datalogsDB = await AppCore.Ins.GetDataDashBoard(ProductId, fileDB, shiftCurrent);

      //}
      //catch (Exception ex)
      //{
      //  LogErrorToFileLog(ex.ToString());
      //}

    }

    private void InitReportAuto()
    {
      shift_last = GetShiftByHour(DateTime.Now.Hour);
      timer_Report_Auto.Interval = 1000;
      timer_Report_Auto.Elapsed += Timer_Report_Auto_Elapsed; ;
      timer_Report_Auto.Start();
    }

    private int shift_last = 0;
    private int shift_current = 0;
    private bool testChangeShift = false;
    public void ChangeShiftTest()
    {
      _datalogsInShiftCurrent = new List<Datalog>();
      shift_current = GetShiftByHour(DateTime.Now.Hour);
      OnSendAutoReport(this, shift_last, shift_current);
    }
    private void Timer_Report_Auto_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      //timer_Report_Auto.Stop();
      //try
      //{

      //  shift_current = GetShiftByHour(DateTime.Now.Hour);
      //  if (shift_current != shift_last)
      //  {
      //    await CreateIfNotExist();
      //    if (OnSendAutoReport != null)
      //    {
      //      OnSendAutoReport(this, shift_last, _masterDataCurrent.Id);
      //      OnSendReSetInforShift();
      //      shift_last = shift_current;
      //    }

      //    datalogsDB.Clear();

      //  }
      //}
      //catch (Exception)
      //{
      //}
      //finally { timer_Report_Auto.Start(); }
    }



    private void InitEvent()
    {
      FrmDashboard.Instance.OnSendChangeOver += Instance_OnSendChangeOver;
    }




    private async void Instance_OnSendChangeOver(object sender, string FGs, string NameProduct, double normalSpeed)
    {
      //if (OnSendAutoReport != null && _masterDataCurrent != null)
      //  OnSendAutoReport(this, shift_current, _masterDataCurrent.Id);

      //await ClearActiveProductCurrent();
      //await ActiveProductCurrent(FGs);
      //_listMasterData = await LoadRangeMasterData();
      //_masterDataCurrent = _listMasterData?.Where(s => s.isEnable == true && s.isDelete == false).FirstOrDefault();

      //if (isConnected == true)
      //{
      //  _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.FGs, (uint)eRegisterPLCLength.FGs, FGs);
      //  _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NameProduct, (uint)eRegisterPLCLength.NameProduct, NameProduct);
      //  _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NormalSpeed, (ulong)normalSpeed);
      //}

      //isChangeOver = false;
    }

    public void LoadConfigsDB()
    {
      try
      {
        //DataBase.Init().Wait();
      }
      catch (Exception ex)
      {
        //LogErrorToFileLog("Lỗi khi khởi tạo chương trình, vui lòng khởi động lại!" + ex.ToString());
        System.Windows.Forms.MessageBox.Show($"Lỗi khi khởi tạo chương trình, vui lòng khởi động lại!", "Lỗi");
        Environment.Exit(2);
      }
    }

    public async Task LoadDataLine()
    {
      //_listMasterData = await LoadRangeMasterData();
      //_masterDataCurrent = _listMasterData.Where(s => s.isEnable == true && s.isDelete == false).FirstOrDefault();
      //_lineCurrent = await LoadLineCurrent();

      //_users = await LoadUser();
    }

    public async void ReloadUser()
    {
      //_users = await LoadUser();
    }

    public MitsubishiClient _client;
    public System.Timers.Timer timer_checkReadtPLC = new System.Timers.Timer();
    private System.Timers.Timer timer_checkConnectPLC = new System.Timers.Timer();
    private bool StatusConnectCurrent = false;
    private FunstionPLC _funstionPLC = new FunstionPLC();
    public void Init_PLC()
    {
      //try
      //{
      //  _client = new MitsubishiClient(MitsubishiVersion.Qna_3E, _lineCurrent.IpPLC, _lineCurrent.Port);
      //  _client.Open();

      //  timer_checkConnectPLC.Interval = 1000;
      //  timer_checkConnectPLC.Elapsed += Timer_checkConnectPLC_Elapsed; ;
      //  timer_checkConnectPLC.Start();
      //}
      //catch (Exception)
      //{
      //}
    }


    private Random rd = new Random();
    public bool isChangeOver = false;

    public int GetShiftByHour(int hour)
    {
      if (hour >= 6 && hour < 14) return 1;
      else if (hour >= 14 && hour < 22) return 2;
      else return 3;
    }

    public async Task UpdateRangeMasterDataOld()
    {
      //using (var context = new ConfigDBContext())
      //{
      //  GenericRepository<MasterData, ConfigDBContext> repo = new ResponsitoryMasterData(context);
      //  await repo.UpdateRangeMasterDataOld();
      //}
    }

    //public async Task AddMasterData(List<MasterData> data)
    //{
    //  //using (var context = new ConfigDBContext())
    //  //{
    //  //  var repo = new GenericRepository<MasterData, ConfigDBContext>(context);
    //  //  await repo.AddRange(data);
    //  //}
    //}
    //public async Task<List<MasterData>> LoadRangeMasterData()
    //{
    //  //using (var context = new ConfigDBContext())
    //  //{
    //  //  var repo = new ResponsitoryMasterData(context);
    //  //  List<MasterData> MasterDatas = await repo.GetAllAsync();
    //  //  return MasterDatas.ToList();
    //  //}
    //}

    //public async Task ClearActiveProductCurrent()
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    GenericRepository<MasterData, ConfigDBContext> repo = new ResponsitoryMasterData(context);
    //    await repo.ClearActiveProductCurrent();
    //  }
    //}

    //public async Task ActiveProductCurrent(string FGs)
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    GenericRepository<MasterData, ConfigDBContext> repo = new ResponsitoryMasterData(context);
    //    await repo.ActiveProductCurrent(FGs);
    //  }
    //}


    //public async Task<Line> LoadLineCurrent()
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    var repo = new ResponsitoryLine(context);
    //    List<Line> Line = await repo.GetAllAsync();
    //    return Line.Where(s => s.IsEnable == true).FirstOrDefault();
    //  }
    //}

    //public async Task UpdateInfoLine(Line line)
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    var repo = new ResponsitoryLine(context);
    //    await repo.UpdateInfoLine(line);
    //  }
    //}

    //public async Task CreateIfNotExist()
    //{
    //  using (var context = new DailyDBContext($"{DateTime.Now.AddDays(+1).ToString("yyMMdd")}"))
    //  {
    //    await context.Database.EnsureCreatedAsync();
    //  }
    //}
    ////Datalog
    //public async Task<bool> AddDataLog(Datalog data, string fileName)
    //{
    //  using (var context = new DailyDBContext(fileName))
    //  {
    //    var repo = new GenericRepository<Datalog, DailyDBContext>(context);
    //    return await repo.Add(data);
    //  }
    //}

    //public async Task<List<Datalog>> LoadDatalog()
    //{
    //  using (var context = new DailyDBContext())
    //  {
    //    var repo = new GenericRepository<Datalog, DailyDBContext>(context);
    //    List<Datalog> MasterDatas = await repo.GetAllAsync();
    //    return MasterDatas.OrderBy(x => x.Id).ToList();
    //  }
    //}

    //public async Task<bool> IsExit(ulong stt, string fileDB)
    //{
    //  using (var context = new DailyDBContext(fileDB))
    //  {
    //    var repo = new GenericRepository<Datalog, DailyDBContext>(context);
    //    List<Datalog> MasterDatas = await repo.GetAllAsync();
    //    ulong cnt = (ulong)MasterDatas.Where(s => s.STT == stt).ToList().Count();
    //    if (cnt > 0)
    //    {
    //      return true;
    //    }
    //    else
    //    {
    //      return false;
    //    }
    //  }
    //}

    //public async Task<List<Datalog>> LoadDatalogDashboard(int shiftId, int productId)
    //{
    //  using (var context = new PostgresDbContext())
    //  {
    //    var repo = new GenericRepository<Datalog, PostgresDbContext>(context);
    //    List<Datalog> MasterDatas = await repo.GetAllAsync();
    //    //if (ShiftId==3)
    //    return MasterDatas.Where(x => x.ShiftId == shiftId && x.ProductId == productId).OrderBy(x => x.Id).ToList();
    //  }
    //}


    //public async Task<List<Datalog>> GetDataReportByFilter(int productId, string dateTimeData)
    //{
    //  using (var context = new DailyDBContext(dateTimeData))
    //  {
    //    GenericRepository<Datalog, DailyDBContext> repo = new ResponsitoryDatalog(context);
    //    return await repo.GetDataReportByFilter(productId);
    //  }
    //}

    //public async Task<List<Datalog>> GetDataDashBoard(int productId, string dateTimeData, int shiftId)
    //{
    //  using (var context = new DailyDBContext(dateTimeData))
    //  {
    //    GenericRepository<Datalog, DailyDBContext> repo = new ResponsitoryDatalog(context);
    //    var data = await repo.GetDataReportByFilter(productId);
    //    return data?.Where(x => x.ShiftId == shiftId).ToList();
    //  }
    //}



    ////User
    //public async Task AddUser(User data)
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    var repo = new GenericRepository<User, ConfigDBContext>(context);
    //    await repo.Add(data);
    //  }
    //}

    //public async Task<bool> CheckUserExit(string data)
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    GenericRepository<User, ConfigDBContext> repo = new ResponsitoryUser(context);
    //    return await repo.CheckExitUser(data);
    //  }
    //}

    //public async Task ClearActiveUser(string role)
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    GenericRepository<User, ConfigDBContext> repo = new ResponsitoryUser(context);
    //    await repo.ClearActiveUser(role);
    //  }
    //}



    //public async Task ActiveUser(string name, string role)
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    GenericRepository<User, ConfigDBContext> repo = new ResponsitoryUser(context);
    //    await repo.ActiveUser(name, role);
    //  }
    //}



    //public async Task<List<User>> LoadUser()
    //{
    //  using (var context = new ConfigDBContext())
    //  {
    //    var repo = new GenericRepository<User, ConfigDBContext>(context);
    //    List<User> MasterDatas = await repo.GetAllAsync();
    //    return MasterDatas;
    //  }
    //}


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

      double USL = (product?.USL ?? 0.0) + (tare?.Tube ?? 0.0) + (tare?.Carton ?? 0.0);
      double LSL = (product?.LSL ?? 0.0) + (tare?.Tube ?? 0.0) + (tare?.Carton ?? 0.0);
      double UCL = (product?.LCL ?? 0.0) + (tare?.Tube ?? 0.0) + (tare?.Carton ?? 0.0);
      double LCL = (product?.UCL ?? 0.0) + (tare?.Tube ?? 0.0) + (tare?.Carton ?? 0.0);
      double target = (product?.Target ?? 0.0) + (tare?.Tube ?? 0.0) + (tare?.Carton ?? 0.0);

      sumaryDTO.USL = USL;
      sumaryDTO.UCL = UCL;
      sumaryDTO.Target = target;
      sumaryDTO.LCL = LCL;
      sumaryDTO.LSL = LSL;

      sumaryDTO.DatalogPass = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept || s.EnumStatusRecord == EnumStatusRecord.Over).ToList();
      sumaryDTO.DatalogOver = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Over).ToList();
      sumaryDTO.DatalogAccept = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept).ToList();
      sumaryDTO.DatalogReject = datalogs.Where(s => s.EnumStatusRecord == EnumStatusRecord.Reject).ToList();

      var dataNetPass = sumaryDTO.DatalogPass.Select(x => x.Net).ToList();
      sumaryDTO.Sample = dataNetPass.Count;
      sumaryDTO.Mean = (sumaryDTO.Sample == 0) ? 0 : CalMean(dataNetPass);

      double Std = (sumaryDTO.Sample == 0) ? 0 : CalStdDev(dataNetPass);
      sumaryDTO.Stdev = Std;
      sumaryDTO.Min = (sumaryDTO.Sample == 0) ? 0 : dataNetPass.Min();
      sumaryDTO.Max = (sumaryDTO.Sample == 0) ? 0 : dataNetPass.Max();

      sumaryDTO.Cp = (Std != 0) ? Math.Round(((USL - LSL) / (6 * Std)), 3) : 0;

      double hcpk = (Std != 0) ? ((USL - sumaryDTO.Mean) / (3 * Std)) : 0;
      double lcpk = (Std != 0) ? ((sumaryDTO.Mean - LSL) / (3 * Std)) : 0;
      sumaryDTO.Cpk = Math.Round(Math.Min(hcpk, lcpk), 3);

      sumaryDTO.OW = (target != 0) ? Math.Round(((sumaryDTO.Mean - target) / target) * 100, 2) : 0;

      //Kết quả
      //if (sumaryDTO.Mean >= LSL && sumaryDTO.Sample > 0)
      //{
      //  sumaryDTO.EnumResult = EnumResult.Success;
      //}
      //if (sumaryDTO.Mean < LSL && sumaryDTO.Sample > 0)
      //{
      //  sumaryDTO.EnumResult = EnumResult.Fail;
      //}  
      //else
      //{
      //  sumaryDTO.EnumResult = EnumResult.None;
      //}
      sumaryDTO.EnumResult = EnumResult.Success;

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
