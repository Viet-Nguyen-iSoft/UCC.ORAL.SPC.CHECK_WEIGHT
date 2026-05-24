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
  public partial class UCTextbox3 : UserControl
  {
    public UCTextbox3()
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
  }
}
