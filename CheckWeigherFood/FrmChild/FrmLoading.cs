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
  public partial class FrmLoading : Form
  {
    public FrmLoading()
    {
      InitializeComponent();
      
    }

    public void ShowLoading(string title = "Loading ...")
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ShowLoading();
        }));
        return;
      }

      this.label1.Text = title;

      if (!this.Visible)
      {
        this.Visible = true;
        this.TopMost = true;
        cnt = 0;
        this.timerTimeOut.Start();
      }  
        
    }
    public void CloseLoading()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          CloseLoading();
        }));
        return;
      }
      this.Visible = false;
      this.timerTimeOut.Stop();
    }



    private int cnt = 0;
    private void timerTimeOut_Tick(object sender, EventArgs e)
    {
      this.timerTimeOut.Stop();
      if (cnt < 30)
      {
        cnt++;
        progressBar1.Value = cnt * (100 /30);
        
      }
      else
      {
        this.Close();
      }
      

      this.timerTimeOut.Start();
    }




  }
}
