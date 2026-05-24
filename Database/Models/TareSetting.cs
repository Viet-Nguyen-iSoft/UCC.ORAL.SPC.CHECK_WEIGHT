using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
  public class TareSetting:BaseModel
  {
    public double? Tube { get; set; }
    public double? Carton { get; set; }


    #region Mapping
   
    #endregion
  }
}
