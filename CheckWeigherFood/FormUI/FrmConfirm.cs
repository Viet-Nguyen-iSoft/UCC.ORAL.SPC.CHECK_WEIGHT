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
  public partial class FrmConfirm : Form
  {
    public delegate void SendSendOKClicked(object sender);
    public event SendSendOKClicked OnSendOKClicked;
    public FrmConfirm()
    {
      InitializeComponent();
    }

    public FrmConfirm(string title, eImage eImage)
    {
      InitializeComponent();

      this.lbInformation.Text = title;
      this.picIcon.Image = new Bitmap(Application.StartupPath + $"\\Image\\{eImage}.png");
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
        lbInformation.Text = information;
        this.picIcon.Image = new Bitmap(Application.StartupPath + $"\\Image\\{nameImage}.png");
        this.ShowDialog();
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex.Message);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (OnSendOKClicked != null)
      {
        OnSendOKClicked(this);
      }
      this.Close();
    }
  }
}
