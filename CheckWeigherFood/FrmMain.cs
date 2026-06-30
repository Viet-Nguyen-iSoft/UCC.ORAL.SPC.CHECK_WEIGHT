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
      this.Shown += FrmMain_Shown;
      this.WindowState = FormWindowState.Maximized;
      this.StartPosition = FormStartPosition.CenterScreen;

      this.btnOverview.Click += BtnOverview_Click;
      this.btnDashBoard.Click += BtnDashBoard_Click;
      this.btnEmployee.Click += BtnEmployee_Click;
      this.btnSetting.Click += BtnSetting_Click;
      this.btnReport.Click += BtnReport_Click;
      this.btnMasterData.Click += BtnMasterData_Click;
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


    private void BtnReport_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Report);
    }

    private void BtnSetting_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Setting);
    }

    private void BtnEmployee_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Employee);
    }

    private void BtnDashBoard_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.DashBoard);
    }
    private void BtnMasterData_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.MasterData);
    }

    private void BtnOverview_Click(object sender, EventArgs e)
    {
      ChangeButton(AppModulSupport.Overview);
    }
   
    public void ChangeButton(AppModulSupport button)
    {
      this.btnOverview.ForeColor = NoSelect;
      this.btnDashBoard.ForeColor = NoSelect;
      this.btnMasterData.ForeColor = NoSelect;
      this.btnReport.ForeColor = NoSelect;
      this.btnSetting.ForeColor = NoSelect;
      this.btnEmployee.ForeColor = NoSelect;

      switch (button)
      {
        case AppModulSupport.Overview:
          this.btnOverview.ForeColor = Select;
          OpenChildForm(button, FrmOverview.Instance);
          break;
        case AppModulSupport.DashBoard:
          this.btnDashBoard.ForeColor = Select;
          OpenChildForm(button, FrmDashboard.Instance);
          break;
        case AppModulSupport.MasterData:
          this.btnMasterData.ForeColor = Select;
          OpenChildForm(button, FrmMasterData.Instance);
          break;
        case AppModulSupport.Report:
          this.btnReport.ForeColor = Select;
          OpenChildForm(button, FrmReport.Instance);
          break;
        case AppModulSupport.Setting:
          this.btnSetting.ForeColor = Select;
          OpenChildForm(button, FrmSetting.Instance);
          break;
        case AppModulSupport.Employee:
          this.btnEmployee.ForeColor = Select;
          OpenChildForm(button, FrmEmployee.Instance);
          break;

      }
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
      this.btnDashBoard.Text = "";
      this.btnMasterData.Text = "";
      this.btnSetting.Text = "";
      this.btnReport.Text = "";
      this.btnEmployee.Text = "";
      this.panelMenu.Width = 75;
      this.picLogo.Visible = false;
      this.picLogoVule.Visible = false;
      this.btnOverview.PerformClick();

      FrmOverview.Instance.OnSendClickDetail += Instance_OnSendClickDetail;
      AppCore.Ins.OnSendAutoReport += Ins_OnSendAutoReport;
    }

    private void Ins_OnSendAutoReport(object sender, int shiftId, int productId)
    {
      FrmAutoReport frmAutoReport = new FrmAutoReport();
      frmAutoReport.FormBorderStyle = FormBorderStyle.None;
      frmAutoReport.WindowState = FormWindowState.Maximized;
      frmAutoReport.StartPosition = FormStartPosition.Manual;
      frmAutoReport.ShowDialog();
    }

    private void Instance_OnSendClickDetail(long obj)
    {
      this.btnDashBoard.PerformClick();
      FrmDashboard.Instance.SetLine(obj);
    }

    private System.Timers.Timer timer_UpdateUI = new System.Timers.Timer();
    private void FrmMain_Shown(object sender, EventArgs e)
    {
      timer_UpdateUI.Interval = 1000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      //timer_UpdateUI.Start();
    }

    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      if (keyMachine==3)
      {
        AppCore.Ins.RandomDataWeight03();
      }  
      else
      {
        AppCore.Ins.RandomDataWeight04();
      }  
    }

    private void btnMenu_Click(object sender, EventArgs e)
    {
      if (this.panelMenu.Width == 190)
      {
        this.panelMenu.Width = 75;
        this.picLogo.Visible = false;
        this.picLogoVule.Visible = false;

        this.btnDashBoard.Text = "";
        this.btnMasterData.Text = "";
        this.btnSetting.Text = "";
        this.btnReport.Text = "";
        this.btnEmployee.Text = "";
      }
      else
      {
        this.panelMenu.Width = 190;
        this.picLogo.Visible = true;
        this.picLogoVule.Visible = true;

        this.btnOverview.Text = "          TỔNG QUAN";
        this.btnDashBoard.Text = "          CHI TIẾT";
        this.btnMasterData.Text = "          SẢN PHẨM";
        this.btnSetting.Text = "          CÀI ĐẶT";
        this.btnReport.Text = "          BÁO CÁO";
        this.btnEmployee.Text = "          NHÂN VIÊN";
      }
    }

    private bool enableTime = false;
    private long keyMachine = 3;
    private void picLogoVule_Click(object sender, EventArgs e)
    {
      return;
      keyMachine = 4;
      enableTime = !enableTime;
      if (enableTime)
        timer_UpdateUI.Start();
      else
        timer_UpdateUI.Stop();
      //AppCore.Ins.RandomDataWeight();
    }

    private void lbLine_Click(object sender, EventArgs e)
    {
      keyMachine = keyMachine == 3 ? 4 : 3;
    }

    private void label2_Click(object sender, EventArgs e)
    {
      //FrmAutoReport frmAutoReport = new FrmAutoReport();
      //frmAutoReport.FormBorderStyle = FormBorderStyle.None;
      //frmAutoReport.WindowState = FormWindowState.Maximized;
      //frmAutoReport.StartPosition = FormStartPosition.Manual;
      //frmAutoReport.ShowDialog();

      //AppCore.Ins._testChangeShift = true;
    }
  }
}
