using CheckWeigherFood.Modbus;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
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
