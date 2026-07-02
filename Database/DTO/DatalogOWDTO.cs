using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DTO
{
  public class DatalogOWDTO
  {
    public string Line { get; set; }
    [DisplayName("Mã SP")]
    public string FGs { get; set; }
    [DisplayName("Tên SP")]
    public string NameProduction { get; set; }
    [DisplayName("Ngày")]
    public string Date { get; set; }
    [DisplayName("Ca")]
    public int Shift { get; set; }
    [DisplayName("Sản lượng")]
    public int NumberDatalog { get; set; }
    [DisplayName("Vận hành máy")]
    public string Operator { get; set; }
    [DisplayName("Số rejects")]
    public int NumberReject { get; set; }
    [DisplayName("OW (%)")]
    public double OW { get; set; }
    [DisplayName("Loss do rejects (g)")]
    public double LossByReject { get; set; }
    [DisplayName("Loss do OW (g)")]
    public double LossByOW { get; set; }
  }
}
