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
  public partial class PopupChangeTare : Form
  {
    public event Action<TareSetting, long> OnChangeTareSetting;
    public PopupChangeTare()
    {
      InitializeComponent();
      RegisterService();
      this.Load += PopupChangeTareAndLot_Load;
    }
    private TareSettingService _tareSettingService { get; set; }
    private long _keyMachine { get; set; }
    private void RegisterService()
    {
      _tareSettingService = AppFactory.CreateTareSettingService();
    }

    public PopupChangeTare(TareSetting tareSetting, long keyMachine) :this()
    {
      _keyMachine = keyMachine;
      ShowInforLotAndTare(tareSetting);
    }

    private void ShowInforLotAndTare(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforLotAndTare(tareSetting); }));
        return;
      }
      
      txtTareTube.Texts = tareSetting?.Tube.ToString() ?? string.Empty;
      txtTareCarton.Texts = tareSetting?.Carton.ToString() ?? string.Empty;
      txtTareTailTube.Texts = tareSetting?.TailTube.ToString() ?? string.Empty;
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
      try
      {
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

        if (string.IsNullOrEmpty(txtTareTailTube.Texts))
        {
          new FrmInformation().ShowMessage("Vui lòng nhập Tare đuôi tube !", eNumUI.eImage.Warning);
          return;
        }

        TareSetting tareSetting = new TareSetting();
        tareSetting.Carton = double.Parse(txtTareCarton.Texts);
        tareSetting.Tube = double.Parse(txtTareTube.Texts);
        tareSetting.TailTube = double.Parse(txtTareTailTube.Texts);
        tareSetting.KeyMachine = _keyMachine;
        tareSetting.CreatedAt = DateTime.UtcNow;

        await _tareSettingService.AddAsync(tareSetting);

        OnChangeTareSetting?.Invoke(tareSetting, _keyMachine);
        this.Close();
      }
      catch (Exception ex)
      {

      }
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
