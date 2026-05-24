using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Models
{
  public class BaseModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //[Browsable(false)]
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}
