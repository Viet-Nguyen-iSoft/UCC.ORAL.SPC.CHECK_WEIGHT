using CheckWeigherFood.FrmChild;
using CheckWeigherFood.Models;
using CheckWeigherFood.PLC;
using DocumentFormat.OpenXml.Spreadsheet;
using IoTClient;
using IoTClient.Clients.PLC;
using IoTClient.Enums;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;
using static CheckWeigherFood.eNum.eRegisterPLC;
using static CheckWeigherFood.Models.DbStore;
using static ClosedXML.Excel.XLPredefinedFormat;
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
      //Check DB và tạo nếu không tồn tại
      LoadConfigsDB();

      //Load data dưới DB
      LoadDataLine().Wait();

      //Init PLC
      Init_PLC();


      //Đăng kí sự kiện thao tác trong UI
      InitEvent();

      //Init Report tự động mỗi khi chuyển ca
      InitReportAuto();

      //Init PLC
      InitReadDataPLC();

      LoadDataDB();

      SetDataStartApp();

      StartShowUI();

      LogActiveAppToFileLog("Open App");
    }

    private void SetDataStartApp()
    {
      if (isConnected)
      {
        //_funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.LoBB, (uint)eRegisterPLCLength.LoBB, LoBB);
      }  
    }

    public volatile List<Datalog> datalogsDB = new List<Datalog>();
    private int ProductId = 0;

    private async void LoadDataDB()
    {
      try
      {
        ProductId = _masterDataCurrent?.Id??0;

        datalogsDB = new List<Datalog>();
        DateTime dt = DateTime.Now;
        int HourCurrent = dt.Hour;

        int shiftCurrent = GetShiftByHour(HourCurrent);

        string pathDatabase = Application.StartupPath + $"\\DataBase\\";
        string fileDB = (HourCurrent >= 0 && HourCurrent < 6) ? dt.AddDays(-1).ToString("yyMMdd") : dt.ToString("yyMMdd");

        string pathFull = pathDatabase + fileDB + ".sqlite";
        if (File.Exists(pathFull))
          datalogsDB = await AppCore.Ins.GetDataDashBoard(ProductId, fileDB, shiftCurrent);

      }
      catch (Exception ex)
      {
        LogErrorToFileLog(ex.ToString());
      }

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
    private async void Timer_Report_Auto_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      timer_Report_Auto.Stop();
      try
      {
        
        shift_current = GetShiftByHour(DateTime.Now.Hour);
        if (shift_current != shift_last)
        {
          await CreateIfNotExist();
          if (OnSendAutoReport != null)
          {
            OnSendAutoReport(this, shift_last, _masterDataCurrent.Id);
            OnSendReSetInforShift();
            shift_last = shift_current;
          }

          datalogsDB.Clear();

        }
      }
      catch (Exception)
      {
      }
      finally { timer_Report_Auto.Start(); }
    }



    private void InitEvent()
    {
      FrmSetting.Instance.OnSendSavePLC += Instance_OnSendSavePLC1;
      FrmSetting.Instance.OnSendSavePathReport += Instance_OnSendSavePathReport;


      FrmSetting.Instance.OnSendAddOP += Instance_OnSendAddOP;
      FrmSetting.Instance.OnSendAddQC += Instance_OnSendAddQC;
      FrmSetting.Instance.OnSendAddTC += Instance_OnSendAddTC;

      FrmSetting.Instance.OnSendSaveNumberSafe += Instance_OnSendSaveNumberSafe;
      FrmSetting.Instance.OnSendSaveNumverQuality += Instance_OnSendSaveNumverQuality;

      FrmSetting.Instance.OnSendSaveOLE += Instance_OnSendSaveOLE;
      FrmSetting.Instance.OnSendSaveOR += Instance_OnSendSaveOR;


      FrmSetting.Instance.OnSendSaveLoBB += Instance_OnSendSaveLoBB;

      FrmDashboard.Instance.OnSendKawaTV += Instance_OnSendKawaTV;

      FrmMasterData.Instance.OnSendChangeMasterData += Instance_OnSendChangeMasterData;



      FrmDashboard.Instance.OnSendChangeOver += Instance_OnSendChangeOver;
      FrmDashboard.Instance.OnSendSaveLoBB += Instance_OnSendSaveLoBB1;

      FrmDashboard.Instance.OnSendSaveOP += Instance_OnSendSaveOP1;
      FrmDashboard.Instance.OnSendSaveQC += Instance_OnSendSaveQC1;
      FrmDashboard.Instance.OnSendSaveTC += Instance_OnSendSaveTC1;

    }

    private async void Instance_OnSendAddTC()
    {
      _users = await LoadUser();
    }

    private async void Instance_OnSendAddQC()
    {
      _users = await LoadUser();
    }

    private async void Instance_OnSendAddOP()
    {
      _users = await LoadUser();
    }

    private void Instance_OnSendSaveLoBB1(object sender, string LoBB)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.LoBB, (uint)eRegisterPLCLength.LoBB, LoBB);
    }

    private async void Instance_OnSendChangeOver(object sender, string FGs, string NameProduct, double normalSpeed)
    {
      if (OnSendAutoReport != null && _masterDataCurrent != null)
        OnSendAutoReport(this, shift_current, _masterDataCurrent.Id);

      await ClearActiveProductCurrent();
      await ActiveProductCurrent(FGs);
      _listMasterData = await LoadRangeMasterData();
      _masterDataCurrent = _listMasterData?.Where(s => s.isEnable == true && s.isDelete == false).FirstOrDefault();

      if (isConnected == true)
      {
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.FGs, (uint)eRegisterPLCLength.FGs, FGs);
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NameProduct, (uint)eRegisterPLCLength.NameProduct, NameProduct);
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NormalSpeed, (ulong)normalSpeed);
      }

      isChangeOver = false;
    }

    public void SendSettingTemperatureSMC(int low, int mid, int high)
    {
      if (isConnected == true)
      {
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.valueTempLow, low);
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.valueTempMid, mid);
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.valueTempHigh, high);
      }
    }

    private async void Instance_OnSendChangeMasterData()
    {
      _listMasterData = await LoadRangeMasterData();
    }

    private void Instance_OnSendKawaTV(object sender, ulong OW, ulong NumberReject)
    {
      if (isConnected == true)
      {
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.OW, OW);
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NumberReject, NumberReject);
      }

    }


    private void Instance_OnSendSaveTC1(object sender, string TC)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.TC, (uint)eRegisterPLCLength.TC, TC);
    }

    private void Instance_OnSendSaveQC1(object sender, string QC)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.QC, (uint)eRegisterPLCLength.QC, QC);
    }

    private void Instance_OnSendSaveOP1(object sender, string OP)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.OP, (uint)eRegisterPLCLength.OP, OP);
    }

    private async void Instance_OnSendSavePathReport(object sender, string path)
    {
      _lineCurrent.PathReport = path;
      await UpdateInfoLine(_lineCurrent);
    }

    private async void Instance_OnSendSavePLC1(object sender, string PLCIP, int Port)
    {
      _lineCurrent.Port = Port;
      _lineCurrent.IpPLC = PLCIP;
      await UpdateInfoLine(_lineCurrent);
    }







    private void Instance_OnSendSaveOR(object sender, int OR)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.OR, OR);
    }

    private void Instance_OnSendSaveOLE(object sender, int OLE)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.OLE, OLE);
    }

    private void Instance_OnSendSaveLoBB(object sender, string LoBB)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.LoBB, (uint)eRegisterPLCLength.LoBB, LoBB);
    }


    private void Instance_OnSendSaveNumverQuality(object sender, ulong NumberQuality)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NumberQuality, NumberQuality);
    }

    private void Instance_OnSendSaveNumberSafe(object sender, ulong NumberSafe)
    {
      if (isConnected == true)
        _funstionPLC.SendDataPLC(_client, (uint)eRegisterPLCStart.NumberSafe, NumberSafe);
    }


    public void LoadConfigsDB()
    {
      try
      {
        DataBase.Init().Wait();
      }
      catch (Exception ex)
      {
        LogErrorToFileLog("Lỗi khi khởi tạo chương trình, vui lòng khởi động lại!" + ex.ToString());
        System.Windows.Forms.MessageBox.Show($"Lỗi khi khởi tạo chương trình, vui lòng khởi động lại!", "Lỗi");
        Environment.Exit(2);
      }
    }


    public List<MasterData> _listMasterData = new List<MasterData>();
    public MasterData _masterDataCurrent = new MasterData();
    public Line _lineCurrent = new Line();
    public List<User> _users = new List<User>();
    public async Task LoadDataLine()
    {
      _listMasterData = await LoadRangeMasterData();
      _masterDataCurrent = _listMasterData.Where(s => s.isEnable == true && s.isDelete == false).FirstOrDefault();
      _lineCurrent = await LoadLineCurrent();

      _users = await LoadUser();
    }

    public async void ReloadUser()
    {
      _users = await LoadUser();
    }

    public MitsubishiClient _client;
    public System.Timers.Timer timer_checkReadtPLC = new System.Timers.Timer();
    private System.Timers.Timer timer_checkConnectPLC = new System.Timers.Timer();
    private bool StatusConnectCurrent = false;
    private FunstionPLC _funstionPLC = new FunstionPLC();
    public void Init_PLC()
    {
      try
      {
        _client = new MitsubishiClient(MitsubishiVersion.Qna_3E, _lineCurrent.IpPLC, _lineCurrent.Port);
        _client.Open();

        timer_checkConnectPLC.Interval = 1000;
        timer_checkConnectPLC.Elapsed += Timer_checkConnectPLC_Elapsed; ;
        timer_checkConnectPLC.Start();
      }
      catch (Exception)
      {
      }
    }



    private bool isConnected = false;
    private void Timer_checkConnectPLC_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      timer_checkConnectPLC.Stop();
      try
      {
        Ping ping = new Ping();
        PingReply FindPLC = ping.Send(_lineCurrent.IpPLC, 100);
        isConnected = _client.Connected && FindPLC.Status.ToString().Equals("Success");
        if (isConnected)
        {

        }
        else
        {
          _client.Close();
          _client = new MitsubishiClient(MitsubishiVersion.Qna_3E, _lineCurrent.IpPLC, _lineCurrent.Port);
          _client.Open();
        }

        NotifyState(isConnected);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally { timer_checkConnectPLC.Start(); }

    }
    private void NotifyState(bool isConnected)
    {
      if (OnSendStatus != null)
      {
        OnSendStatus(this, isConnected);
      }
      StatusConnectCurrent = isConnected;
    }

    private System.Timers.Timer timer_Randomdata = new System.Timers.Timer();


    private System.Timers.Timer timer_ReadDataPLC = new System.Timers.Timer();
    private void InitReadDataPLC()
    {
      timer_ReadDataPLC.Interval = 1000;
      timer_ReadDataPLC.Elapsed += Timer_ReadDataPLC_Elapsed;
      timer_ReadDataPLC.Start();
    }

    private void Timer_ReadDataPLC_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      timer_ReadDataPLC.Stop();
      ReadData();
      timer_ReadDataPLC.Start();
    }


    private async void ReadData()
    {
      if (isChangeOver || !isConnected) return;
      if (_masterDataCurrent==null) return;

      try
      {
        uint startResgister = (uint)eRegisterPLCStart.BuferSTT;
        for (int i = 0; i < 10; i++)
        {
          int index = i * 4;
          var dataBlockPLC = _funstionPLC.ReadDataPLC(_client, (uint)(startResgister + index), 4);
          if (dataBlockPLC == null) return;

          var STT = (ushort)dataBlockPLC[2].Value << 16 | (ushort)dataBlockPLC[3].Value;
          var ValueWeigher = ((ushort)dataBlockPLC[0].Value << 16 | (ushort)dataBlockPLC[1].Value);

          DateTime dt = DateTime.Now;
          int Hour = dt.Hour;
          string fileNameDB = (Hour <= 23 && Hour >= 6) ? dt.ToString("yyMMdd") : dt.AddDays(-1).ToString("yyMMdd");

          bool isExit = await IsExit((ulong)STT, fileNameDB);

          if (!isExit && STT != 0)
          {
            Datalog dataRow = new Datalog();
            dataRow.STT = (ulong)STT;
            dataRow.ProductId = _masterDataCurrent.Id;
            dataRow.Gross = Math.Round(((double)ValueWeigher) / 100, 2);
            dataRow.Net = dataRow.Gross;
            dataRow.OP = _users?.Where(s => s.Role == "OP" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
            dataRow.QC = _users?.Where(s => s.Role == "QC" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
            dataRow.TC = _users?.Where(s => s.Role == "TC" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
            dataRow.LoBB = Properties.Settings.Default.LoBB;
            dataRow.ShiftId = GetShiftByHour(Hour);

            dataRow.CreatedAt = (Hour <= 23 && Hour >= 6) ? dt : dt.AddDays(-1);

            //Check Status
            if (dataRow.Gross > _masterDataCurrent.Max) dataRow.Status = "Over";
            else if (dataRow.Gross < _masterDataCurrent.Min) dataRow.Status = "Reject";
            else dataRow.Status = "Ok";

            //Check fileDB Shift 3
            var isResult = await AddDataLog(dataRow, fileNameDB);
            if (isResult)
            {
              lock (datalogsDB)
                datalogsDB.Add(dataRow);
            }
          }  
        }
      }
      catch (Exception ex)
      {
        LogErrorToFileLog("Lỗi log datalog-" + ex.ToString());
        await Console.Out.WriteLineAsync(ex.Message);

      }
    }

    private ulong no = 1;
    public async void RandomData()
    {
      double value = rd.Next(38000, 42000);

      DateTime dt = DateTime.Now;
      int Hour = dt.Hour;
      string fileNameDB = (Hour <= 23 && Hour >= 6) ? dt.ToString("yyMMdd") : dt.AddDays(-1).ToString("yyMMdd");


      Datalog dataRow = new Datalog();
      dataRow.STT = (ulong)no++;
      dataRow.ProductId = _masterDataCurrent.Id;
      dataRow.Gross = Math.Round(((double)value) / 100, 2);
      dataRow.Net = dataRow.Gross;
      dataRow.OP = _users?.Where(s => s.Role == "OP" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
      dataRow.QC = _users?.Where(s => s.Role == "QC" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
      dataRow.TC = _users?.Where(s => s.Role == "TC" & s.isEnable == true).Select(s => s.Name).FirstOrDefault();
      dataRow.LoBB = Properties.Settings.Default.LoBB;
      dataRow.ShiftId = GetShiftByHour(Hour);

      dataRow.CreatedAt = (Hour <= 23 && Hour >= 6) ? dt : dt.AddDays(-1);

      //Check Status
      if (dataRow.Gross > _masterDataCurrent.Max) dataRow.Status = "Over";
      else if (dataRow.Gross < _masterDataCurrent.Min) dataRow.Status = "Reject";
      else dataRow.Status = "Ok";

      //Check fileDB Shift 3
      var isResult = await AddDataLog(dataRow, fileNameDB);
      if (isResult)
      {
        lock (datalogsDB)
          datalogsDB.Add(dataRow);
      }
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
      using (var context = new ConfigDBContext())
      {
        GenericRepository<MasterData, ConfigDBContext> repo = new ResponsitoryMasterData(context);
        await repo.UpdateRangeMasterDataOld();
      }
    }

    public async Task AddMasterData(List<MasterData> data)
    {
      using (var context = new ConfigDBContext())
      {
        var repo = new GenericRepository<MasterData, ConfigDBContext>(context);
        await repo.AddRange(data);
      }
    }
    public async Task<List<MasterData>> LoadRangeMasterData()
    {
      using (var context = new ConfigDBContext())
      {
        var repo = new ResponsitoryMasterData(context);
        List<MasterData> MasterDatas = await repo.GetAllAsync();
        return MasterDatas.ToList();
      }
    }

    public async Task ClearActiveProductCurrent()
    {
      using (var context = new ConfigDBContext())
      {
        GenericRepository<MasterData, ConfigDBContext> repo = new ResponsitoryMasterData(context);
        await repo.ClearActiveProductCurrent();
      }
    }

    public async Task ActiveProductCurrent(string FGs)
    {
      using (var context = new ConfigDBContext())
      {
        GenericRepository<MasterData, ConfigDBContext> repo = new ResponsitoryMasterData(context);
        await repo.ActiveProductCurrent(FGs);
      }
    }


    public async Task<Line> LoadLineCurrent()
    {
      using (var context = new ConfigDBContext())
      {
        var repo = new ResponsitoryLine(context);
        List<Line> Line = await repo.GetAllAsync();
        return Line.Where(s => s.IsEnable == true).FirstOrDefault();
      }
    }

    public async Task UpdateInfoLine(Line line)
    {
      using (var context = new ConfigDBContext())
      {
        var repo = new ResponsitoryLine(context);
        await repo.UpdateInfoLine(line);
      }
    }

    public async Task CreateIfNotExist()
    {
      using (var context = new DailyDBContext($"{DateTime.Now.AddDays(+1).ToString("yyMMdd")}"))
      {
        await context.Database.EnsureCreatedAsync();
      }
    }
    //Datalog
    public async Task<bool> AddDataLog(Datalog data, string fileName)
    {
      using (var context = new DailyDBContext(fileName))
      {
        var repo = new GenericRepository<Datalog, DailyDBContext>(context);
        return await repo.Add(data);
      }
    }

    public async Task<List<Datalog>> LoadDatalog()
    {
      using (var context = new DailyDBContext())
      {
        var repo = new GenericRepository<Datalog, DailyDBContext>(context);
        List<Datalog> MasterDatas = await repo.GetAllAsync();
        return MasterDatas.OrderBy(x => x.Id).ToList();
      }
    }

    public async Task<bool> IsExit(ulong stt, string fileDB)
    {
      using (var context = new DailyDBContext(fileDB))
      {
        var repo = new GenericRepository<Datalog, DailyDBContext>(context);
        List<Datalog> MasterDatas = await repo.GetAllAsync();
        ulong cnt = (ulong)MasterDatas.Where(s => s.STT == stt).ToList().Count();
        if (cnt > 0)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    public async Task<List<Datalog>> LoadDatalogDashboard(int shiftId, int productId)
    {
      using (var context = new DailyDBContext())
      {
        var repo = new GenericRepository<Datalog, DailyDBContext>(context);
        List<Datalog> MasterDatas = await repo.GetAllAsync();
        //if (ShiftId==3)
        return MasterDatas.Where(x => x.ShiftId == shiftId && x.ProductId == productId).OrderBy(x => x.Id).ToList();
      }
    }


    public async Task<List<Datalog>> GetDataReportByFilter(int productId, string dateTimeData)
    {
      using (var context = new DailyDBContext(dateTimeData))
      {
        GenericRepository<Datalog, DailyDBContext> repo = new ResponsitoryDatalog(context);
        return await repo.GetDataReportByFilter(productId);
      }
    }

    public async Task<List<Datalog>> GetDataDashBoard(int productId, string dateTimeData, int shiftId)
    {
      using (var context = new DailyDBContext(dateTimeData))
      {
        GenericRepository<Datalog, DailyDBContext> repo = new ResponsitoryDatalog(context);
        var data = await repo.GetDataReportByFilter(productId);
        return data?.Where(x => x.ShiftId == shiftId).ToList();
      }
    }



    //User
    public async Task AddUser(User data)
    {
      using (var context = new ConfigDBContext())
      {
        var repo = new GenericRepository<User, ConfigDBContext>(context);
        await repo.Add(data);
      }
    }

    public async Task<bool> CheckUserExit(string data)
    {
      using (var context = new ConfigDBContext())
      {
        GenericRepository<User, ConfigDBContext> repo = new ResponsitoryUser(context);
        return await repo.CheckExitUser(data);
      }
    }

    public async Task ClearActiveUser(string role)
    {
      using (var context = new ConfigDBContext())
      {
        GenericRepository<User, ConfigDBContext> repo = new ResponsitoryUser(context);
        await repo.ClearActiveUser(role);
      }
    }



    public async Task ActiveUser(string name, string role)
    {
      using (var context = new ConfigDBContext())
      {
        GenericRepository<User, ConfigDBContext> repo = new ResponsitoryUser(context);
        await repo.ActiveUser(name, role);
      }
    }



    public async Task<List<User>> LoadUser()
    {
      using (var context = new ConfigDBContext())
      {
        var repo = new GenericRepository<User, ConfigDBContext>(context);
        List<User> MasterDatas = await repo.GetAllAsync();
        return MasterDatas;
      }
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

  }
}
