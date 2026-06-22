using CheckWeigherFood.Modbus;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static CheckWeigherFood.Controls.AppCore;
using static Database.Enum;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    public delegate void SendValueWeight(double value, bool statusMachine, string ok);
    public event SendValueWeight OnSendValueWeight;


    public delegate void SendMsg(string msg);
    public event SendMsg OnSendMsg;

    public delegate void SendMsgRead(string msg);
    public event SendMsgRead OnSendMsgRead;

    public delegate void SendJson(string json);
    public event SendJson OnSendJson;
    /// 
    //OPC -UA
    private OpcUaClient opcClient = new OpcUaClient();
    private string opcUrl = $"opc.tcp://10.157.120.23:49320";
    private string opcWeight = "";
    private string opcStatusMachine = "";

    public System.Timers.Timer timer_read_opc_ua = new System.Timers.Timer();
    public System.Timers.Timer timer_check_connect = new System.Timers.Timer();
    private void Init_OPC_UA()
    {
      opcUrl = Environment.GetEnvironmentVariable("OPC_UA_HOST");
      opcWeight = Environment.GetEnvironmentVariable("OPC_UA_WEIGHT");
      opcStatusMachine = Environment.GetEnvironmentVariable("OPC_UA_STATUS_MACHINE");

      timer_read_opc_ua.Interval = 200;
      timer_read_opc_ua.Elapsed += Timer_read_opc_ua_Elapsed;
      timer_read_opc_ua.Start();

      timer_check_connect.Interval = 1000;
      timer_check_connect.Elapsed += Timer_check_connect_Elapsed;
      timer_check_connect.Start();
    }
    //private async Task Connect()
    //{
    //  opcClient = new OpcUaClient();

    //  opcClient.AppConfig.ApplicationName = "test";
    //  opcClient.AppConfig.ApplicationUri = $"urn:{Utils.GetHostName()}:test";
    //  opcClient.AppConfig.ApplicationType = ApplicationType.Client;
    //  opcClient.UseSecurity = false;
    //  opcClient.AppConfig.SecurityConfiguration = new SecurityConfiguration
    //  {
    //    ApplicationCertificate = new CertificateIdentifier
    //    {
    //      StoreType = "Directory",
    //      StorePath = @"CertificateStores\Own",
    //      SubjectName = "test"
    //    },

    //    TrustedPeerCertificates = new CertificateTrustList
    //    {
    //      StoreType = "Directory",
    //      StorePath = @"CertificateStores\UA Applications"
    //    },

    //    TrustedIssuerCertificates = new CertificateTrustList
    //    {
    //      StoreType = "Directory",
    //      StorePath = @"CertificateStores\UA Certificate Authorities"
    //    },

    //    RejectedCertificateStore = new CertificateTrustList
    //    {
    //      StoreType = "Directory",
    //      StorePath = @"CertificateStores\RejectedCertificates"
    //    },

    //    AutoAcceptUntrustedCertificates = true,
    //    AddAppCertToTrustedStore = true
    //  };

    //  opcClient.AppConfig.TransportQuotas = new TransportQuotas
    //  {
    //    OperationTimeout = 15000
    //  };

    //  opcClient.AppConfig.ClientConfiguration.DefaultSessionTimeout = 60000;

    //  await opcClient.AppConfig.Validate(ApplicationType.Client);

    //  //bool certOk = await opcClient.AppConfig.(false, 2048);
    //  //if (!certOk)
    //  //  throw new Exception("Cannot create OPC UA application certificate.");

    //  opcClient.AppConfig.CertificateValidator.CertificateValidation += (sender, e) =>
    //  {
    //    // Chỉ dùng để test. Production nên trust certificate rõ ràng.
    //    e.Accept = true;
    //  };

    //  opcClient.UserIdentity = new UserIdentity();
    //  opcClient.ConnectComplete += (s, e) =>
    //  {
    //    Console.WriteLine("OPC UA connected.");
    //  };
    //  opcClient.OpcStatusChange += OpcClient_OpcStatusChange;
    //  await opcClient.ConnectServer("opc.tcp://DESKTOP-5DG8V11:49320");
    //}

    //private void OpcClient_OpcStatusChange(object sender, OpcUaStatusEventArgs e)
    //{
    //  Console.WriteLine(e.Error);
    //}

    private async void Timer_check_connect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        timer_check_connect.Stop();

        string key = " ";
        if (opcClient.Connected == false)
        {
          opcClient = new OpcUaClient();
          //UserIdentity userIdentity = new UserIdentity("admin", "admin");
          UserIdentity userIdentity = new UserIdentity();
          opcClient.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
          opcClient.ConnectComplete += OpcClient_ConnectComplete;
          opcClient.UserIdentity = userIdentity;
          await opcClient.ConnectServer(opcUrl);


          //try
          //{
          //  opcClient = new OpcUaClient();
          //  //opcClient.AppConfig.ApplicationName = "test";
          //  //opcClient.AppConfig.ClientConfiguration.DefaultSessionTimeout = 10000;
          //  opcClient.UserIdentity =
          //      new UserIdentity(new AnonymousIdentityToken());

          //  await opcClient.ConnectServer("opc.tcp://DESKTOP-5DG8V11:49320");
          //  //await Connect();
          //  //MessageBox.Show("Connected");
          //}
          //catch (Exception ex)
          //{
          //  //MessageBox.Show(ex.ToString());
          //}
          //finally
          //{

          //  //timer_check_connect.Start();
          //}
          key = " Trong ";
        }



        string status = opcClient.Connected == true ? " - Kết nối" : " - Mất kết nối";
        string msg = DateTime.Now.ToString("HH:mm:ss") + key + status;
        OnSendMsg?.Invoke(msg);
      }
      catch (Exception ex)
      {
        OnSendMsg?.Invoke(ex.ToString());
      }
      finally
      {
        timer_read_opc_ua.Start();
      }
    }

    private double previous = 0;
    private bool firstApp = true;
    private async void Timer_read_opc_ua_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        timer_read_opc_ua.Stop();

        //string key = " ";
        double valueSend = 0;
        if (opcClient.Connected)
        {
          //key = " tren ";
          //Value
          //string nodeId_temp = "ns=2;s=OL04C.07.C4P00";
          //nodeId_temp = "ns=2;s=OL04C.07.C4M00";
          //nodeId_temp = "ns=2;s=OL04C.07.C4P00";
          var value_temp = opcClient.ReadNode(opcWeight);
          double value = Convert.ToDouble(value_temp.Value);
          value = Math.Round(value / 100.0, 2);

          //string json = JsonConvert.SerializeObject(value_temp);
          //string a = value_temp.GetType().FullName;
          //OnSendJson?.Invoke(a + " - " + value_temp.StatusCode + " - " + value_temp.StatusCode.Code + "****" + json);
          //valueSend = value;

          ////Status
          ////string nodeId_status_machine = "ns=2;s=OL04C.07.C4P00";
          //var value_status_machine = opcClient.ReadNode(opcStatusMachine);
          //int _status_machine = Convert.ToInt16(value_status_machine.Value);
          int _status_machine = 1;

          if (value != previous)
          {
            previous = value;
            OnSendValueWeight?.Invoke(value, _status_machine == 1, "data ok");

            double valueFilter = (_productCurrent?.LSL ?? 0.0) * 0.5;
            if (value > 0 && firstApp == false)
            {
              await SaveDatalog(value);
            }

          }
        }
        else
        {
          //key = " duoi ";
          OnSendValueWeight?.Invoke(-1, false, "Mất kết nối");
        }


        //string status = opcClient.Connected == true ? " - Kết nối" : " - Mất kết nối";
        //string msg = DateTime.Now.ToString("HH:mm:ss") + key + status + " Value: " + valueSend.ToString();
        //OnSendMsgRead?.Invoke(msg);
        firstApp = false;
      }
      catch (Exception ex)
      {
        OnSendValueWeight?.Invoke(404, false, ex.ToString());
      }
      finally
      {
        timer_read_opc_ua.Start();
      }
    }
    private void OpcClient_ConnectComplete(object sender, EventArgs e)
    {
      //try
      //{
      //  if (opcClient.Connected)
      //  {
      //    string nodeId_temp = "ns=2;s=OL04C.07.C4P00";
      //    var value_temp = opcClient.ReadNode(nodeId_temp);

      //    double value = Convert.ToDouble(value_temp.Value);

      //    OnSendValueWeight?.Invoke(value, true, "ok");

      //    opcClient.Disconnect();
      //  }
      //  else
      //  {
      //    //OnSendStatusSMC?.Invoke(false);
      //    OnSendValueWeight?.Invoke(0.0, false, "Mất kết nối");
      //  }
      //}
      //catch (Exception ex)
      //{
      //  OnSendValueWeight?.Invoke(0.0, false, ex.ToString());
      //}
    }


    /// <summary>
    /// //
    /// </summary>
    private ModbusTcpService _modbus { get; set; }
    private void InitModbus(string ip, int port)
    {
      string ipModbus = Environment.GetEnvironmentVariable("MODBUS_HOST");
      int portModbus = int.Parse(Environment.GetEnvironmentVariable("MODBUS_PORT"));
      ushort addressWeight = ushort.Parse(Environment.GetEnvironmentVariable("MODBUS_ADDRESS_WEIGHT"));

      _modbus = new ModbusTcpService(ip, port, addressWeight, 1);

      _modbus.ConnectionChanged += Modbus_ConnectionChanged;
      _modbus.DataReceived += _modbus_DataReceived;
      _modbus.Error += _modbus_Error;
      _modbus.Start(200);
    }

    private void _modbus_Error(object sender, Exception e)
    {

    }

    private void _modbus_DataReceived(object sender, ModbusDataEventArgs e)
    {

    }

    private void Modbus_ConnectionChanged(
    object sender,
    bool connected)
    {

    }






    private Random random = new Random();
    public async void RandomDataWeight()
    {
      double max = 157.0;
      double min = 146.0;

      max = 41;
      min = 39;

      max = 165;
      min = 148;

      double value = random.NextDouble() * (max - min) + min;
      value = Math.Round(value, 2);
      OnSendValueWeight?.Invoke(value, true, "data ok");
      await SaveDatalog(value);
    }

    private async Task SaveDatalog(double value)
    {
      try
      {
        if (_machineCurrent == null || _productCurrent == null || _appConfig?.ChangeOverId <= 0) return;

        Datalog datalog = new Datalog();
        datalog.Gross = value;
        datalog.TareTube = (_tareSettingCurrent?.Tube ?? 0.0);
        datalog.TareCarton = (_tareSettingCurrent?.Carton ?? 0.0);
        datalog.TareTailTube = (_tareSettingCurrent?.TailTube ?? 0.0);
        datalog.LotTube = _tareSettingCurrent?.LotTube;
        datalog.LotCarton = _tareSettingCurrent?.LotCarton;
        datalog.EnumStatusRecord = CheckStatus(_productCurrent, _tareSettingCurrent, value);
        datalog.ChangeOverId = _appConfig.ChangeOverId;

        if (_operationSettingCurrent?.OP != null)
          datalog.NameEmployeeOP = _operationSettingCurrent?.OP;
        if (_operationSettingCurrent?.QC != null)
          datalog.NameEmployeeQC = _operationSettingCurrent?.QC;
        if (_operationSettingCurrent?.ShiftLeader != null)
          datalog.NameEmployeeShiftLeader = _operationSettingCurrent?.ShiftLeader;

        datalog.MachineId = _machineCurrent?.Id;
        datalog.ProductId = _productCurrent?.Id;
        datalog.CreatedAt = DateTime.Now;
        var rs = await _datalogService.AddAsync(datalog);
        if (rs)
          _datalogsInShiftCurrent.Add(datalog);
      }
      catch (Exception)
      {

      }
    }

    private static EnumStatusRecord CheckStatus(Product product, TareSetting tareSetting, double net)
    {
      double usl = (product?.USL ?? 0.0) + (tareSetting?.Tube ?? 0.0) - (tareSetting?.TailTube ?? 0.0) + (tareSetting?.Carton ?? 0.0);
      double target = (product?.Target ?? 0.0) + (tareSetting?.Tube ?? 0.0) - (tareSetting?.TailTube ?? 0.0) + (tareSetting?.Carton ?? 0.0);
      double lsl = (product?.LSL ?? 0.0) + (tareSetting?.Tube ?? 0.0) - (tareSetting?.TailTube ?? 0.0) + (tareSetting?.Carton ?? 0.0);

      if (product.IsAbsolute)
      {
        if (net > usl)
        {
          return EnumStatusRecord.Over;
        }
        else if (net <= usl && net >= lsl)
        {
          return EnumStatusRecord.Accept;
        }

        return EnumStatusRecord.Reject;
      }
      else
      {
        if (net > usl)
        {
          return EnumStatusRecord.Over;
        }
        else if (net >= lsl && net <= usl)
        {
          return EnumStatusRecord.Accept;
        }
        return EnumStatusRecord.Reject;
      }

    }
  }
}
