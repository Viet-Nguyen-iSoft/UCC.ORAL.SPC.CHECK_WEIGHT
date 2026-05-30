using CheckWeigherFood.Modbus;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    public delegate void SendValueWeight(double value, bool statusMachine, string ok);
    public event SendValueWeight OnSendValueWeight;
    /// 
    //OPC -UA
    private OpcUaClient opcClient = new OpcUaClient();
    private string opcUrl = $"opc.tcp://10.157.120.23:49320";

    public System.Timers.Timer timer_read_opc_ua = new System.Timers.Timer();
    public System.Timers.Timer timer_check_connect = new System.Timers.Timer();
    private void Init_OPC_UA()
    {
      opcUrl = $"opc.tcp://10.157.120.23:49320";

      timer_read_opc_ua.Interval = 200;
      timer_read_opc_ua.Elapsed += Timer_read_opc_ua_Elapsed;
      timer_read_opc_ua.Start();

      timer_check_connect.Interval = 1000;
      timer_check_connect.Elapsed += Timer_check_connect_Elapsed;
      timer_check_connect.Start();
    }

    private void Timer_check_connect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        timer_read_opc_ua.Stop();

        if (opcClient.Connected == false)
        {
          opcClient = new OpcUaClient();
          //UserIdentity userIdentity = new UserIdentity("admin", "admin");
          UserIdentity userIdentity = new UserIdentity();
          opcClient.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
          opcClient = new OpcUaClient();
          opcClient.ConnectComplete += OpcClient_ConnectComplete;
          opcClient.UserIdentity = userIdentity;
          opcClient.ConnectServer(opcUrl);
        } 
      }
      catch (Exception)
      {

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
        if (opcClient.Connected)
        {
          //Value
          string nodeId_temp = "ns=2;s=OL04C.07.C4P00";
          var value_temp = opcClient.ReadNode(nodeId_temp);
          double value = Convert.ToDouble(value_temp.Value);
          value = Math.Round(value/100.0, 2);

          //Status
          string nodeId_status_machine = "ns=2;s=OL04C.07.C4P00";
          var value_status_machine = opcClient.ReadNode(nodeId_status_machine);
          int _status_machine = Convert.ToInt16(value_status_machine.Value);


          if (value!= previous)
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
          OnSendValueWeight?.Invoke(0.0, false, "Mất kết nối");
        }

        firstApp = false;
      }
      catch (Exception ex)
      {
        OnSendValueWeight?.Invoke(0.0, false, ex.ToString());
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
    private ModbusTcpService _modbus;
    private void InitModbus(string ip, int port)
    {
      _modbus = new ModbusTcpService(ip, port, 1);

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
      double min = 230.0;
      double max = 240.0;
      double value = random.NextDouble() * (max - min) + min;
      value = Math.Round(value, 2);

      await SaveDatalog(value);
    }

    private async Task SaveDatalog(double value)
    {
      Datalog datalog = new Datalog();
      datalog.Net = value;
      datalog.TareTube = (_tareSettingCurrent?.Tube ?? 0.0);
      datalog.TareCarton = (_tareSettingCurrent?.Carton ?? 0.0);
      datalog.EnumStatusRecord = CheckStatus(_productCurrent, (_tareSettingCurrent?.Tube ?? 0.0), (_tareSettingCurrent?.Carton ?? 0.0), value);
      datalog.NameEmployeeOP = _operationSettingCurrent.OP;
      datalog.NameEmployeeQC = _operationSettingCurrent.QC;
      datalog.NameEmployeeShiftLeader = _operationSettingCurrent.ShiftLeader;
      datalog.MachineId = _machineCurrent?.Id;
      datalog.ProductId = _productCurrent?.Id;
      datalog.CreatedAt = DateTime.UtcNow;
      var rs = await _datalogService.AddAsync(datalog);
      if (rs)
        _datalogsInShiftCurrent.Add(datalog);
    }

    private EnumStatusRecord CheckStatus(Product product, double tube, double carton, double net)
    {
      double actual = net - carton - tube;
      if (actual > product.USL)
      {
        return EnumStatusRecord.Over;
      }
      if (actual < product.LSL)
      {
        return EnumStatusRecord.Reject;
      }
      return EnumStatusRecord.Accept;
    }
  }
}
