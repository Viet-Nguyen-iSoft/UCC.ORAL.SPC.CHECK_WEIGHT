using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Database.Models
{
  public class Tare:BaseModel
  {
    public string? Tube { get; set; }
    public string? Carton { get; set; }


    #region Mapping
   
    #endregion
  }
}
