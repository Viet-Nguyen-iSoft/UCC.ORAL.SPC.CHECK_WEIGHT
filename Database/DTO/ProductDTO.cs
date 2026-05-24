using System.ComponentModel;

namespace Database.DTO
{
  public class ProductDTO
  {
    [DisplayName("Stt")]
    public int No { get; set; }

    [DisplayName("Pro-name")]
    public string ProName { get; set; }

    [DisplayName("SAP code")]
    public string Code { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }

    [DisplayName("Weight on pack")]
    public string WeightOnPack { get; set; }
    [DisplayName("Đơn vị")]
    public string Unit { get; set; }


    [DisplayName("Trọng lượng cận trên (không bì)")]
    public double? USL { get; set; }



    [DisplayName("Trọng lượng an toàn trên (không bì)")]
    public double? UCL { get; set; }



    [DisplayName("Trọng lượng net weigh (g) (không bì)")]
    public double? Target { get; set; }



    [DisplayName("Trọng lượng an toàn dưới (không bì)")]
    public double? LCL { get; set; }


    [DisplayName("Trọng lượng cận dưới (không bì)")]
    public double? LSL { get; set; }



    [DisplayName("Tỉ trọng")]
    public double? Density { get; set; }


    [DisplayName("T (Trọng lượng thiếu cho phép) g")]
    public double? T { get; set; }
    [DisplayName("Trọng lượng bì (g)")]
    public double? Tare { get; set; }
    [DisplayName("Note")]
    public string Note { get; set; }
    public string Datetime { get; set; }

  }
}
