using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using CheckWeigherFood.FrmChild;
using CustomControls.RJControls;
using Database.Models;
using Database.Service;
using System;
using System.Windows.Forms;

namespace CheckWeigherFood.Popup
{
  public partial class PopupChangeTareAndLot : Form
  {
    public event Action<TareSetting> OnChangeTareSetting;
    public PopupChangeTareAndLot()
    {
      InitializeComponent();
      RegisterService();
      this.Load += PopupChangeTareAndLot_Load;
    }
    public PopupChangeTareAndLot(TareSetting tareSetting) :this()
    {
      ShowInforLotAndTare(tareSetting);
    }

    private void ShowInforLotAndTare(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforLotAndTare(tareSetting); }));
        return;
      }

      txtLot.Texts = tareSetting?.Lot ?? string.Empty;
      txtTareTube.Texts = tareSetting?.Tube.ToString() ?? string.Empty;
      txtTareCarton.Texts = tareSetting?.Carton.ToString() ?? string.Empty;
      txtTareTailTube.Texts = tareSetting?.TailTube.ToString() ?? string.Empty;
    }


    private TareSettingService _tareSettingService { get; set; }
    private void RegisterService()
    {
      _tareSettingService = AppFactory.CreateTareSettingService();
    }

    private void PopupChangeTareAndLot_Load(object sender, EventArgs e)
    {
      txtTareCarton.KeyPress += TextBox_PositiveDecimalOnly;
      txtTareTube.KeyPress += TextBox_PositiveDecimalOnly;
      txtTareTailTube.KeyPress += TextBox_PositiveDecimalOnly;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private async void btnConfirm_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtLot.Texts))
      {
        new FrmInformation().ShowMessage("Vui lòng nhập lô sản xuất !", eNumUI.eImage.Warning);
        return;
      }

      if (string.IsNullOrEmpty(txtTareTube.Texts))
      {
        new FrmInformation().ShowMessage("Vui lòng nhập Tare tube !", eNumUI.eImage.Warning);
        return;
      }

      if (string.IsNullOrEmpty(txtTareCarton.Texts))
      {
        new FrmInformation().ShowMessage("Vui lòng nhập Tare carton !", eNumUI.eImage.Warning);
        return;
      }

      TareSetting tareSetting = new TareSetting();
      tareSetting.Lot = txtLot.Texts;
      tareSetting.Carton = double.Parse(txtTareCarton.Texts);
      tareSetting.Tube = double.Parse(txtTareTube.Texts);
      tareSetting.TailTube = double.Parse(txtTareTailTube.Texts);
      tareSetting.CreatedAt = DateTime.UtcNow;

      await _tareSettingService.AddAsync(tareSetting);

      OnChangeTareSetting?.Invoke(tareSetting);
      this.Close();
    }


    private void TextBox_PositiveDecimalOnly(object sender, KeyPressEventArgs e)
    {
      RJTextBox txt = sender as RJTextBox;
      if (char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        return;
      if (e.KeyChar == '.' && !txt.Texts.Contains("."))
        return;
      e.Handled = true;
    }
  }
}
