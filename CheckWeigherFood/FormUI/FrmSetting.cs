using CheckWeigherFood.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmSetting : Form
  {
    public FrmSetting()
    {
      InitializeComponent();
    }

    #region Singleton parttern
    private static FrmSetting _Instance = null;
    public static FrmSetting Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmSetting();
        }
        return _Instance;
      }
    }

    #endregion

    private void FrmSetting_Load(object sender, EventArgs e)
    {
      
    }

    private void picPathReport_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Chọn thư mục cần lưu Report";
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        this.txtReport.Text = folderBrowserDialog.SelectedPath;
        this.txtReport.ForeColor = Color.Red;
      }
    }

    private void btnSavePath_Click(object sender, EventArgs e)
    {

    }





 
  }
}
