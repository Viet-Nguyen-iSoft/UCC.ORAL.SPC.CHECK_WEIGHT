using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.FrmChild
{
  public partial class ReCombobox : UserControl
  {
    public ReCombobox()
    {
      InitializeComponent();
    }

    public List<string> SetDataSource
    {
      set
      {
        this.comboBox1.DataSource = value;
      }
    }

    public string SetValue
    {
      set
      {
        this.comboBox1.Text = value;
      }
      get { return this.comboBox1.Text; }
    }

    public string SelectedItem
    {
      set
      {
        this.comboBox1.SelectedItem = value;
      }
      get { return this.comboBox1.Text; }
    }

    public void ClearCb()
    {
      this.comboBox1.SelectedIndex = -1;
    }


    private Action _SelectIndex = null;
    public void SetTag(Action SelectIndexChange)
    {
      this._SelectIndex = SelectIndexChange;
    }

    private void comboBox1_SelectedIndexChanged_2(object sender, EventArgs e)
    {
      if (_SelectIndex != null)
      {
        _SelectIndex();
      }
    }
  }
}
