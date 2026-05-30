namespace Database.Models
{
  public class TareSetting : BaseModel
  {
    public double? Tube { get; set; }
    public double? TailTube { get; set; }
    public double? Carton { get; set; }
    public string Lot { get; set; }


    #region Mapping

    #endregion
  }
}
