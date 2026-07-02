using Database.Models;
using System;
using System.Collections.Generic;

namespace Database.DTO
{
  public class DatalogGroup
  {
    public long? ProductId { get; set; }
    public long ChangeOverId { get; set; }
    public List<Datalog> Datalogs { get; set; }
  }

  public class DatalogGroupCalLossOW
  {
    public long? MachineId { get; set; }
    public long? ProductId { get; set; }
    public long ChangeOverId { get; set; }

    // Ngày của ca
    public DateTime Date { get; set; }

    public int Shift { get; set; }

    public List<Datalog> Items { get; set; } = new List<Datalog>();
  }
}
