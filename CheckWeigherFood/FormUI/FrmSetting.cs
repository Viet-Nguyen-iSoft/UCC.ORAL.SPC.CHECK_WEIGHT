using CheckWeigherFood.Controls;
using CheckWeigherFood.RJControl;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmSetting : Form
  {
    public FrmSetting()
    {
      InitializeComponent();
      CustomUI();
      ResgisterService();
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

    private MachineService _machineService { get; set; }

    private void ResgisterService()
    {
      _machineService = AppFactory.CreateMachineService();
    }

    private async void FrmSetting_Load(object sender, EventArgs e)
    {
      //Line
      var machines = await _machineService.GetDataAsync();
      ShowData(AppCore.Ins._machineCurrent03.PathReport, machines, AppCore.Ins._machineCurrent03);
    }

    private void ShowData(string pathReport, List<Machine> machines, Machine machineCurrent)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowData(pathReport, machines, machineCurrent); }));
        return;
      }

      this.lbPathReport03.ValueStr = pathReport;
    }

    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = tableLayoutPanel4;
      elipseControl0.CornerRadius = 20;

      ElipseControl elipseControl1 = new ElipseControl();
      elipseControl1.TargetControl = tableLayoutPanel2;
      elipseControl1.CornerRadius = 20;

      lbPathReport03.SetLabelAlign(ContentAlignment.MiddleLeft);
      lbPathReport04.SetLabelAlign(ContentAlignment.MiddleLeft);
    }

    private void picPathReport03_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Chọn thư mục cần lưu Report";
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        this.lbPathReport03.ValueStr = folderBrowserDialog.SelectedPath;
      }
    }

    private void picPathReportLine04_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Chọn thư mục cần lưu Report";
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        this.lbPathReport04.ValueStr = folderBrowserDialog.SelectedPath;
      }
    }

    private async void btnSavePathReport03_Click(object sender, EventArgs e)
    {
      try
      {
        AppCore.Ins._machineCurrent03.PathReport = lbPathReport03.ValueStr;
        AppCore.Ins._machineCurrent03.UpdatedAt = DateTime.Now;
        await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent03);
        new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

    private async void btnSavePathReportLine04_Click(object sender, EventArgs e)
    {
      try
      {
        AppCore.Ins._machineCurrent04.PathReport = lbPathReport04.ValueStr;
        AppCore.Ins._machineCurrent04.UpdatedAt = DateTime.Now;
        await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent04);
        new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

   
    private async void btnComm03_Click(object sender, EventArgs e)
    {
      try
      {
        int port = 502;
        int.TryParse(txtPort03.Texts, out port);
        AppCore.Ins._machineCurrent03.IP = txtIP03.Texts.Trim();
        AppCore.Ins._machineCurrent03.Port = port;
        AppCore.Ins._machineCurrent03.UpdatedAt = DateTime.Now;
        await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent03);

        new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

    private async void btnComm04_Click(object sender, EventArgs e)
    {
      try
      {
        int port = 502;
        int.TryParse(txtPort04.Texts, out port);
        AppCore.Ins._machineCurrent04.IP = txtIP04.Texts.Trim();
        AppCore.Ins._machineCurrent04.Port = port;
        AppCore.Ins._machineCurrent04.UpdatedAt = DateTime.Now;
        await AppCore.Ins.UpdateMachine(AppCore.Ins._machineCurrent04);

        new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

   
  }
}
