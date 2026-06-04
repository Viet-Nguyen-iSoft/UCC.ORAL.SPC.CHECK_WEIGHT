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
    public event Action<TareSetting> OnChangeTareSetting;
    public PopupChangeLot()
    {
      InitializeComponent();
      RegisterService();
    }

    private TareSetting _tareSetting { get; set; }
    public PopupChangeLot(TareSetting tareSetting) : this()
    {
      _tareSetting = tareSetting;
      ShowInforLotAndTare(tareSetting);
    }

    private void ShowInforLotAndTare(TareSetting tareSetting)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforLotAndTare(tareSetting); }));
        return;
      }

      var rs = AppCore.Ins.SplitString(tareSetting?.Lot);
      txtLotTube.Texts = rs.str1;
      txtLotCarton.Texts = rs.str2;
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
        tareSetting.Lot = txtLotTube.Texts + "||" + txtLotCarton.Texts;
        tareSetting.CreatedAt = DateTime.UtcNow;

        await _tareSettingService.AddAsync(tareSetting);

        OnChangeTareSetting?.Invoke(tareSetting);
        this.Close();
      }
      catch (Exception ex)
      {

      }
    }

  }
}
