using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Enum;

namespace Database.DTO
{
  public class DatalogDTO
  {
    [DisplayName("Stt")]
    public int No { get; set; }
    [DisplayName("Trọng lượng (g)")]
    public double Gross { get; set; }
    [DisplayName("Tare tube (g)")]
    public double TareTube { get; set; }
    [DisplayName("Tare đuôi tube (g)")]
    public double TareTailTube { get; set; }
    [DisplayName("Tare carton (g)")]
    public double TareCarton { get; set; }
    [DisplayName("Lot tube")]
    public string LotTube { get; set; }
    [DisplayName("Lot carton")]
    public string LotCarton { get; set; }

    [DisplayName("Trạng thái")]
    public string Status { get; set; }
    [Browsable(false)]
    public EnumStatusRecord EnumStatusRecord { get; set; }

    [DisplayName("Thời gian")]
    public string DateTime { get; set; }
    
  }
}
