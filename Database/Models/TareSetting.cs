namespace Database.Models
{
  public class TareSetting : BaseModel
  {
    public double? Tube { get; set; }
    public double? TailTube { get; set; }
    public double? Carton { get; set; }
    //public string Lot { get; set; }
    public string LotTube { get; set; }
    public string LotCarton { get; set; }
    public long KeyMachine { get; set; }


    #region Mapping

    #endregion
  }
}
