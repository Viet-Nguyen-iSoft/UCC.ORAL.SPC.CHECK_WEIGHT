using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmInformation : Form
  {
    public FrmInformation()
    {
      InitializeComponent();
    }
    public void ShowMessage(string information, eImage nameImage)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ShowMessage(information, nameImage);
        }));
        return;
      }
      try
      {
        lbTitle.Text = information;
        this.picLogo.Image = new Bitmap(Application.StartupPath + $"\\Image\\{nameImage}.png");
        timerInformation.Enabled = true;
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void timerInformation_Tick(object sender, EventArgs e)
    {
      timerInformation.Stop();
      this.Close();
    }
  }
}
