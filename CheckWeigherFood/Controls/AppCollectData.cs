using CheckWeigherFood.Modbus;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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


    public delegate void SendMsgDebug(string msg);
    public event SendMsgDebug OnSendDebug;

    public delegate void SendMsgRead(string msg);
    public event SendMsgRead OnSendMsgRead;

    private double previous = 0;
    private bool firstApp = true;

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
   
    private async void Timer_check_connect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        timer_check_connect.Stop();

        if (opcClient.Connected == false)
        {
          opcClient = new OpcUaClient();
          //UserIdentity userIdentity = new UserIdentity("admin", "admin");
          UserIdentity userIdentity = new UserIdentity();
          opcClient.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
          opcClient.ConnectComplete += OpcClient_ConnectComplete;
          opcClient.UserIdentity = userIdentity;
          await opcClient.ConnectServer(opcUrl);
        }

        //string status = opcClient.Connected == true ? " - Kết nối" : " - Mất kết nối";
        //string msg = DateTime.Now.ToString("HH:mm:ss") + key + status;
        //OnSendMsg?.Invoke(msg);
      }
      catch (Exception ex)
      {
        OnSendDebug?.Invoke(ex.ToString());
      }
      finally
      {
        timer_read_opc_ua.Start();
      }
    }

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

            double valueFilter = (_productCurrent04?.LSL ?? 0.0) * 0.5;
            if (value > 0 && firstApp == false)
            {
              //await SaveDatalog(value);
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
    private void InitModbus()
    {
      string ipModbus = Environment.GetEnvironmentVariable("MODBUS_HOST");
      int portModbus = int.Parse(Environment.GetEnvironmentVariable("MODBUS_PORT"));
      ushort addressWeight = ushort.Parse(Environment.GetEnvironmentVariable("MODBUS_ADDRESS_WEIGHT"));

      _modbus = new ModbusTcpService(ipModbus, portModbus, addressWeight, 1);

      _modbus.ConnectionChanged += Modbus_ConnectionChanged;
      _modbus.DataReceived += _modbus_DataReceived;
      _modbus.Error += _modbus_Error;
      _modbus.OnSendDebug += _modbus_OnSendDebug;
      _modbus.Start(200);
    }

    private void _modbus_OnSendDebug(object sender, string e)
    {
      OnSendDebug?.Invoke(e);
    }

    private void _modbus_Error(object sender, Exception e)
    {
       
    }

    private int k = 0;
    private async void _modbus_DataReceived(object sender, ModbusDataEventArgs e)
    {
      ushort value = e.Registers[1];
      double valueWeight = ((double)value) / 100.0;
      OnSendValueWeight?.Invoke(valueWeight, true, "data ok");

      k++;
      string result = string.Join("-", e.Registers);
      OnSendMsgRead?.Invoke(k.ToString() + "---"+ result);

      if (firstApp)
      {
        previous = value;
        firstApp = false;
      }

      if (previous!= value)
      {
        previous = value;
        double valueFilter = (_productCurrent04?.LSL ?? 0.0) * 0.5;
        if (valueWeight > valueFilter)
        {
          //await SaveDatalog(valueWeight);
        }
      }  
    }

    private void Modbus_ConnectionChanged(
    object sender,
    bool connected)
    {

    }






    private Random random = new Random();
    public async void RandomDataWeight()
    {
      double max = 129.0;
      double min = 132.0;

      max = 41;
      min = 39;

      max = 129;
      min = 140;

      double value = random.NextDouble() * (max - min) + min;
      value = Math.Round(value, 2);
      OnSendValueWeight?.Invoke(value, true, "data ok");
      //await SaveDatalog(value);
    }


    private async Task<Datalog> SaveDatalog(double value,long machineId,long productId, long changeOverId)
    {
      try
      {
        Datalog datalog = new Datalog();
        datalog.Gross = value;
        datalog.TareTube = (_tareSettingCurrent04?.Tube ?? 0.0);
        datalog.TareCarton = (_tareSettingCurrent04?.Carton ?? 0.0);
        datalog.TareTailTube = (_tareSettingCurrent04?.TailTube ?? 0.0);
        datalog.LotTube = _tareSettingCurrent04?.LotTube;
        datalog.LotCarton = _tareSettingCurrent04?.LotCarton;
        datalog.EnumStatusRecord = CheckStatus(_productCurrent04, _tareSettingCurrent04, value);
        

        //if (_operationSettingCurrent?.OP != null)
        //  datalog.NameEmployeeOP = _operationSettingCurrent?.OP;
        //if (_operationSettingCurrent?.QC != null)
        //  datalog.NameEmployeeQC = _operationSettingCurrent?.QC;
        //if (_operationSettingCurrent?.ShiftLeader != null)
        //  datalog.NameEmployeeShiftLeader = _operationSettingCurrent?.ShiftLeader;

        datalog.MachineId = _machineCurrent03?.Id;
        datalog.ProductId = productId;
        datalog.ChangeOverId = changeOverId;
        datalog.MachineId = machineId;
        datalog.CreatedAt = DateTime.Now;
        var rs = await _datalogService.AddAsync(datalog);
        return rs;
      }
      catch (Exception)
      {
        throw;
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
