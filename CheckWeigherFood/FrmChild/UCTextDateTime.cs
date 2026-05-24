using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood
{
  public partial class UCTextDateTime : UserControl
  {
    public UCTextDateTime()
    {
      InitializeComponent();
    }
    public string Text
    {
      set
      {
        this.textBox1.Text = value;
      }
      get { return this.textBox1.Text; }
    }

    public void TextAlign()
    {
      this.textBox1.TextAlign = HorizontalAlignment.Center;
    }


  }
}
