using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    private Random random = new Random();
    public async void RandomDataWeight()
    {
      double min = 200.0;
      double max = 250.0;
      double value = random.NextDouble() * (max - min) + min;
      value = Math.Round(value, 2);

      await SaveDatalog(value);
    }

    private async Task SaveDatalog(double value)
    {
      Datalog datalog = new Datalog();
      datalog.Net = value + (_tareSettingCurrent?.Tube ?? 0.0);
      datalog.TareTube = (_tareSettingCurrent?.Tube ?? 0.0);
      datalog.TareCarton = (_tareSettingCurrent?.Carton ?? 0.0);
      datalog.NameEmployeeOP = _operationSettingCurrent.OP;
      datalog.NameEmployeeQC = _operationSettingCurrent.QC;
      datalog.NameEmployeeShiftLeader = _operationSettingCurrent.ShiftLeader;
      datalog.MachineId = _machineCurrent?.Id;
      datalog.ProductId = _productCurrent?.Id;
      datalog.CreatedAt = DateTime.UtcNow;
      await _datalogService.AddAsync(datalog);

      _datalogsInShiftCurrent.Add(datalog);
    }
  }
}
