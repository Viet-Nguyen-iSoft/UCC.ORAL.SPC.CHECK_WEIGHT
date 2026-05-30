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
    [Browsable(false)]
    public string WeightOnPack { get; set; }
    [DisplayName("Đơn vị")]
    [Browsable(false)]
    public string Unit { get; set; }


    [DisplayName("USL (không bì)")]
    public double? USL { get; set; }



    [DisplayName("UCL (không bì)")]
    public double? UCL { get; set; }



    [DisplayName("Target (g) (không bì)")]
    public double? Target { get; set; }



    [DisplayName("LCL (không bì)")]
    public double? LCL { get; set; }


    [DisplayName("LSL (không bì)")]
    public double? LSL { get; set; }



    [DisplayName("Tỉ trọng")]
    [Browsable(false)]
    public double? Density { get; set; }


    [DisplayName("T g")]
    public double? T { get; set; }

    [DisplayName("Trọng lượng bì (g)")]
    [Browsable(false)]
    public double? Tare { get; set; }

    [DisplayName("Note")]
    public string Note { get; set; }

    public string Datetime { get; set; }

  }
}
