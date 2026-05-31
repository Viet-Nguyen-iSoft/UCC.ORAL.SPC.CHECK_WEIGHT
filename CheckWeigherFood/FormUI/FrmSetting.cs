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
      ShowData(AppCore.Ins._appConfig.PathReport, machines, AppCore.Ins._machineCurrent);
    }

    private void ShowData(string pathReport, List<Machine> machines, Machine machineCurrent)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowData(pathReport, machines, machineCurrent); }));
        return;
      }

      this.lbPathReport.ValueStr = pathReport;
      this.cbbLine.DataSource = machines;
      this.cbbLine.DisplayMember = nameof(Machine.Name);
      this.cbbLine.SelectedItem = machineCurrent;
    }

    private void CustomUI()
    {
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = tableLayoutPanel4;
      elipseControl0.CornerRadius = 20;

      lbPathReport.SetLabelAlign(ContentAlignment.MiddleLeft);
    }

    private void picPathReport_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Chọn thư mục cần lưu Report";
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        this.lbPathReport.ValueStr = folderBrowserDialog.SelectedPath;
      }
    }

    private async void btnSavePathReport_Click(object sender, EventArgs e)
    {
      try
      {
        AppCore.Ins._appConfig.PathReport = lbPathReport.ValueStr;
        AppCore.Ins._appConfig.UpdatedAt = DateTime.Now;
        await AppCore.Ins.UpdateAppConfig(AppCore.Ins._appConfig);
        new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }

    private async void btnSaveLine_Click(object sender, EventArgs e)
    {
      try
      {
        Machine machine = (Machine)this.cbbLine.SelectedItem;

        if (machine != null)
        {
          AppCore.Ins._appConfig.MachineId = machine.Id;
          AppCore.Ins._appConfig.UpdatedAt = DateTime.Now;
          await AppCore.Ins.UpdateAppConfig(AppCore.Ins._appConfig);
          new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
        }
        else
        {
          new FrmInformation().ShowMessage("Không tìm thấy thông tin máy !", eImage.Information);
        }  
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu thất bại !", eImage.Warning);
      }
    }


  }
}
