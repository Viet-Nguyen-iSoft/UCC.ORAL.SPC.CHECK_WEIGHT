using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using CheckWeigherFood.FrmChild;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.Popup
{
  public partial class PopupChangeLot : Form
  {
    public event Action<TareSetting, long> OnChangeTareSetting;
    public PopupChangeLot()
    {
      InitializeComponent();
      RegisterService();
    }

    private TareSetting _tareSetting { get; set; }
    private long _keyMachine { get; set; }
    public PopupChangeLot(TareSetting tareSetting, long keyMachine) : this()
    {
      _tareSetting = tareSetting;
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

      txtLotTube.Texts = tareSetting.LotTube;
      txtLotCarton.Texts = tareSetting.LotCarton;
    }

    private TareSettingService _tareSettingService { get; set; }
    private void RegisterService()
    {
      _tareSettingService = AppFactory.CreateTareSettingService();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private async void btnConfig_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtLotTube.Texts))
        {
          new FrmInformation().ShowMessage("Vui lòng nhập Lot tube !", eNumUI.eImage.Warning);
          return;
        }

        if (string.IsNullOrEmpty(txtLotCarton.Texts))
        {
          new FrmInformation().ShowMessage("Vui lòng nhập Lot carton !", eNumUI.eImage.Warning);
          return;
        }


        TareSetting tareSetting = new TareSetting();
        tareSetting.Carton = _tareSetting.Carton;
        tareSetting.Tube = _tareSetting.Tube;
        tareSetting.TailTube = _tareSetting.TailTube;
        tareSetting.LotTube = txtLotTube.Texts;
        tareSetting.LotCarton = txtLotCarton.Texts;
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

  }
}
