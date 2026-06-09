using Database.Models;
using System.Collections.Generic;

namespace Database.DTO
{
  public class DatalogGroup
  {
    public long? ProductId { get; set; }
    public long ChangeOverId { get; set; }
    public List<Datalog> Datalogs { get; set; }
  }
}
