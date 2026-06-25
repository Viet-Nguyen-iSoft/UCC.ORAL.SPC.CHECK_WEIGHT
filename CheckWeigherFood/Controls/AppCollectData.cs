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
using System.Threading;
using System.Threading.Tasks;
using static CheckWeigherFood.Controls.AppCore;
using static Database.Enum;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    public delegate void SendValueWeight(double value, bool statusMachine, long machineKey);
    public event SendValueWeight OnSendValueWeight;


    public delegate void SendMsgDebug(string msg);
    public event SendMsgDebug OnSendDebug;

    public delegate void SendMsgRead(string msg);
    public event SendMsgRead OnSendMsgRead;

    private double previous03 = 0;
    private bool firstApp03 = true;

    private double previous04 = 0;
    private bool firstApp04 = true;


    /// <summary>
    /// //
    /// </summary>
    private ModbusTcpService _modbusLine03 { get; set; }
    private ModbusTcpService _modbusLine04 { get; set; }
    private void InitModbus()
    {
      string ipModbus = Environment.GetEnvironmentVariable("MODBUS_HOST");
      int portModbus = int.Parse(Environment.GetEnvironmentVariable("MODBUS_PORT"));
      ushort addressWeight = ushort.Parse(Environment.GetEnvironmentVariable("MODBUS_ADDRESS_WEIGHT"));

      _modbusLine03 = new ModbusTcpService("10.2.3.56", 502, addressWeight, 1);
      _modbusLine03.ConnectionChanged += Modbus_ConnectionChanged_Line03;
      _modbusLine03.DataReceived += _modbus_DataReceived_Line03;
      _modbusLine03.Error += _modbus_Error_Line03;
      _modbusLine03.OnSendDebug += _modbus_OnSendDebug_Line03;
      _modbusLine03.Start(200);

      _modbusLine04 = new ModbusTcpService("10.2.4.56", 502, addressWeight, 1);
      _modbusLine04.ConnectionChanged += Modbus_ConnectionChanged_Line04;
      _modbusLine04.DataReceived += _modbus_DataReceived_Line04;
      _modbusLine04.Error += _modbus_Error_Line04;
      _modbusLine04.OnSendDebug += _modbus_OnSendDebug_Line04;
      _modbusLine04.Start(200);

      InitWatchdog();
    }

    private System.Threading.Timer _watchdogTimer03;
    private readonly object _lockObj03 = new object();
    public EnumStatusMachine _enumStatusMachine03  = EnumStatusMachine.Stop;

    private System.Threading.Timer _watchdogTimer04;
    private readonly object _lockObj04 = new object();
    public EnumStatusMachine _enumStatusMachine04 = EnumStatusMachine.Stop;

    public void InitWatchdog()
    {
      _watchdogTimer03 = new System.Threading.Timer(
          WatchdogTimeout03,
          null,
          Timeout.Infinite,
          Timeout.Infinite);

      _watchdogTimer04 = new System.Threading.Timer(
          WatchdogTimeout04,
          null,
          Timeout.Infinite,
          Timeout.Infinite);
    }

    private void ResetWatchdog03()
    {
      lock (_lockObj03)
      {
        _watchdogTimer03?.Change(
            TimeSpan.FromSeconds(60),
            Timeout.InfiniteTimeSpan);

        _enumStatusMachine03 = EnumStatusMachine.Run;
      }
    }
    private void ResetWatchdog04()
    {
      lock (_lockObj04)
      {
        _watchdogTimer04?.Change(
            TimeSpan.FromSeconds(60),
            Timeout.InfiniteTimeSpan);

        _enumStatusMachine04 = EnumStatusMachine.Run;
      }
    }

    private void WatchdogTimeout03(object state)
    {
      _enumStatusMachine03 = EnumStatusMachine.Stop;
    }
    private void WatchdogTimeout04(object state)
    {
      _enumStatusMachine04 = EnumStatusMachine.Stop;
    }





    private void _modbus_OnSendDebug_Line03(object sender, string e)
    {
      OnSendDebug?.Invoke(e);
    }
    private void _modbus_OnSendDebug_Line04(object sender, string e)
    {
      OnSendDebug?.Invoke(e);
    }

    private void _modbus_Error_Line03(object sender, Exception e)
    {
       
    }
    private void _modbus_Error_Line04(object sender, Exception e)
    {

    }

    private async void _modbus_DataReceived_Line03(object sender, ModbusDataEventArgs e)
    {
      ushort value = e.Registers[1];
      double valueWeight = ((double)value) / 100.0;
      OnSendValueWeight?.Invoke(valueWeight, true, 3);

      //k++;
      //string result = string.Join("-", e.Registers);
      //OnSendMsgRead?.Invoke(k.ToString() + "---"+ result);

      if (firstApp03)
      {
        previous03 = valueWeight;
        firstApp03 = false;
      }

      if (previous03 != valueWeight)
      {
        previous03 = valueWeight;
        double valueFilter = (_productCurrent04?.LSL ?? 0.0) * 0.5;
        if (valueWeight > valueFilter)
        {
          if (_productCurrent03?.Id > 0 && _machineCurrent03?.ChangeOverId > 0)
          {
            var rs = await SaveDatalog03(valueWeight, _productCurrent03.Id, _machineCurrent03.ChangeOverId);
            if (rs != null)
            {
              _datalogsInShiftCurrent_Line3.Add(rs);
              ResetWatchdog03();
            }
          }
        }
      }  
    }

    private async void _modbus_DataReceived_Line04(object sender, ModbusDataEventArgs e)
    {
      ushort value = e.Registers[1];
      double valueWeight = ((double)value) / 100.0;
      OnSendValueWeight?.Invoke(valueWeight, true, 4);

      //string result = string.Join("-", e.Registers);
      //OnSendMsgRead?.Invoke(k.ToString() + "---" + result);

      if (firstApp04)
      {
        previous04 = valueWeight;
        firstApp04 = false;
      }

      if (previous04 != valueWeight)
      {
        previous04 = valueWeight;
        double valueFilter = (_productCurrent04?.LSL ?? 0.0) * 0.5;
        if (valueWeight > valueFilter)
        {
          if (_productCurrent04?.Id > 0 && _machineCurrent04?.ChangeOverId > 0)
          {
            var rs = await SaveDatalog04(valueWeight, _productCurrent04.Id, _machineCurrent04.ChangeOverId);
            if (rs != null)
            {
              _datalogsInShiftCurrent_Line4.Add(rs);
              ResetWatchdog04();
            }
          }
        }
      }
    }

    private void Modbus_ConnectionChanged_Line03(
    object sender,
    bool connected)
    {

    }
    private void Modbus_ConnectionChanged_Line04(
   object sender,
   bool connected)
    {

    }




    private Random random = new Random();
    public async void RandomDataWeight03()
    {
      double max = 186.0;
      double min = 170.0;

      double value = random.NextDouble() * (max - min) + min;
      value = Math.Round(value, 2);
      OnSendValueWeight?.Invoke(value, true, 3);
      if (_productCurrent03?.Id>0 && _machineCurrent03?.ChangeOverId>0)
      {
        var rs = await SaveDatalog03(value, _productCurrent03.Id, _machineCurrent03.ChangeOverId);
        if (rs != null)
        {
          _datalogsInShiftCurrent_Line3.Add(rs);
        }
      }  
        
    }

    public async void RandomDataWeight04()
    {
      double max = 185.0;
      double min = 166.0;

      double value = random.NextDouble() * (max - min) + min;
      value = Math.Round(value, 2);
      OnSendValueWeight?.Invoke(value, true, 4);
      if (_productCurrent04?.Id > 0 && _machineCurrent04?.ChangeOverId > 0)
      {
        var rs = await SaveDatalog03(value, _productCurrent04.Id, _machineCurrent04.ChangeOverId);
        if (rs != null)
        {
          _datalogsInShiftCurrent_Line4.Add(rs);
        }  
      }  
    }


    private async Task<Datalog> SaveDatalog03(double value,long productId, long changeOverId)
    {
      try
      {
        Datalog datalog = new Datalog();
        datalog.Gross = value;
        datalog.TareTube = (_tareSettingCurrent03?.Tube ?? 0.0);
        datalog.TareCarton = (_tareSettingCurrent03?.Carton ?? 0.0);
        datalog.TareTailTube = (_tareSettingCurrent03?.TailTube ?? 0.0);
        datalog.LotTube = _tareSettingCurrent03?.LotTube;
        datalog.LotCarton = _tareSettingCurrent03?.LotCarton;
        datalog.EnumStatusRecord = CheckStatus(_productCurrent03, _tareSettingCurrent03, value);


        if (_operationSettingCurrent03?.OP != null)
          datalog.NameEmployeeOP = _operationSettingCurrent03?.OP;
        if (_operationSettingCurrent03?.QC != null)
          datalog.NameEmployeeQC = _operationSettingCurrent03?.QC;
        if (_operationSettingCurrent03?.ShiftLeader != null)
          datalog.NameEmployeeShiftLeader = _operationSettingCurrent03?.ShiftLeader;

        datalog.MachineId = _machineCurrent03?.Id;
        datalog.ProductId = productId;
        datalog.ChangeOverId = changeOverId;
        datalog.CreatedAt = DateTime.Now;
        var rs = await _datalogService.AddAsync(datalog);
        return rs;
      }
      catch (Exception)
      {
        throw;
      }
    }
    private async Task<Datalog> SaveDatalog04(double value, long productId, long changeOverId)
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


        if (_operationSettingCurrent04?.OP != null)
          datalog.NameEmployeeOP = _operationSettingCurrent04?.OP;
        if (_operationSettingCurrent04?.QC != null)
          datalog.NameEmployeeQC = _operationSettingCurrent04?.QC;
        if (_operationSettingCurrent04?.ShiftLeader != null)
          datalog.NameEmployeeShiftLeader = _operationSettingCurrent04?.ShiftLeader;

        datalog.MachineId = _machineCurrent04?.Id;
        datalog.ProductId = productId;
        datalog.ChangeOverId = changeOverId;
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
