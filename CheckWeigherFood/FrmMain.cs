using CheckWeigherFood.Controls;
using CheckWeigherFood.FormUI;
using CheckWeigherFood.FrmChild;
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

namespace CheckWeigherFood
{
  public partial class FrmMain : Form
  {
    public FrmMain()
    {
      InitializeComponent();
      this.WindowState = FormWindowState.Maximized;
      this.StartPosition = FormStartPosition.CenterScreen;
    }

    #region Singleton parttern
    private static FrmMain _Instance = null;
    public static FrmMain Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmMain();
        }
        return _Instance;
      }
    }
    #endregion


    #region Call form child
    private Form CurrentForm;
    public void OpenChildForm(AppModulSupport modulSupport, Form ChildForm)
    {
      bool Is_same_form = false;
      if (this.panelMain.Tag != null)
      {
        if (this.panelMain.Tag is Tuple<AppModulSupport, Form>)
        {
          Tuple<AppModulSupport, Form> TagAsForm = (Tuple<AppModulSupport, Form>)(this.panelMain.Tag);
          if (TagAsForm.Item1 == modulSupport)
          {
            Is_same_form = true;
          }
        }
      }
      if (Is_same_form == false)
      {
        if (CurrentForm != null)
        {
          CurrentForm.Visible = false;

        }
        this.panelMain.Tag = Tuple.Create(modulSupport, ChildForm);
        CurrentForm = ChildForm;
        ChildForm.TopLevel = false;
        ChildForm.FormBorderStyle = FormBorderStyle.None;
        ChildForm.Dock = DockStyle.Fill;
        ChildForm.BringToFront();
        this.panelMain.Controls.Add(ChildForm);
        ChildForm.Show();
      }
      else
      {
        //do not 
      }
    }
    #endregion

    private static Color Select = Color.FromArgb(255, 255, 255);
    private static Color NoSelect = Color.FromArgb(49, 67, 107);




    private void btnDashBoard_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.DashBoard);
    }

    private void btnMasterData_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.MasterData);
    }

    private void btnSynthetic_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Synthetic);
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Report);
    }

    private void btnSetting_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Setting);
    }

    private void btnEmployee_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Employee);
    }

    public void ChangeButton(AppModulSupport button)
    {
      this.btnDashBoard.ForeColor = NoSelect;
      this.btnMasterData.ForeColor = NoSelect;
      this.btnReport.ForeColor = NoSelect;
      this.btnSetting.ForeColor = NoSelect;

      switch (button)
      {
        case AppModulSupport.DashBoard:
          this.btnDashBoard.ForeColor = Select;
          OpenChildForm(AppModulSupport.DashBoard, FrmDashboard.Instance);
          break;
        case AppModulSupport.MasterData:
          this.btnMasterData.ForeColor = Select;
          OpenChildForm(AppModulSupport.MasterData, FrmMasterData.Instance);
          break;
        case AppModulSupport.Report:
          this.btnReport.ForeColor = Select;
          OpenChildForm(AppModulSupport.Report, FrmReport.Instance);
          break;
        case AppModulSupport.Setting:
          this.btnSetting.ForeColor = Select;
          OpenChildForm(AppModulSupport.Setting, FrmSetting.Instance);
          break;
        case AppModulSupport.Employee:
          this.btnSetting.ForeColor = Select;
          OpenChildForm(AppModulSupport.Employee, FrmEmployee.Instance);
          break;

      }
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
      this.panelMenu.Width = 75;
      this.picLogo.Visible = false;
      this.picLogoVule.Visible = false;
      this.btnDashBoard.PerformClick();
      AppCore.Ins.OnSendStatus += Ins_OnSendStatus;

      AppCore.Ins.OnSendAutoReport += Ins_OnSendAutoReport1; 
    }

    private void Ins_OnSendAutoReport1(object sender, int shiftId, int productId)
    {
      FrmAutoReport report = new FrmAutoReport(shiftId, productId);
      report.BringToFront();
      report.ShowDialog();
    }

  

    private void Ins_OnSendStatus(object sender, bool isConnect)
    {
      //if (this.InvokeRequired)
      //{
      //  this.Invoke(new Action(() =>
      //  {
      //    Ins_OnSendStatus(sender, isConnect);
      //  }));
      //  return;
      //}

      //this.statusPLC.Visible = !isConnect;
    }

    private void label1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnMenu_Click(object sender, EventArgs e)
    {
      if (this.panelMenu.Width == 190)
      {
        this.panelMenu.Width = 75;
        this.picLogo.Visible = false;
        this.picLogoVule.Visible = false;
      }
      else
      {
        this.panelMenu.Width = 190;
        this.picLogo.Visible = true;
        this.picLogoVule.Visible = true;
      }
    }

    private void picLogo_Click(object sender, EventArgs e)
    {
      
    }

    private void label2_Click(object sender, EventArgs e)
    {
      AppCore.Ins.RandomData();
    }


    private void label4_Click(object sender, EventArgs e)
    {
      AppCore.Ins.RandomDataWeight();
    }
  }
}
