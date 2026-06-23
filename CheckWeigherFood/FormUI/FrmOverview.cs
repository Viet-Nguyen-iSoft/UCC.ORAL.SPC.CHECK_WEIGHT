using CheckWeigherFood.Controls;
using CheckWeigherFood.FrmChild;
using Database.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CheckWeigherFood.FormUI
{
  public partial class FrmOverview : Form
  {
    public FrmOverview()
    {
      InitializeComponent();
      this.Load += FrmOverview_Load;
    }

   
    #region Singleton parttern
    private static FrmOverview _Instance = null;
    public static FrmOverview Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmOverview();
        }
        return _Instance;
      }
    }
    #endregion


    private System.Timers.Timer timer_UpdateUI = new System.Timers.Timer();
    private void FrmOverview_Load(object sender, EventArgs e)
    {
      //Tạo timer load data 2s
      timer_UpdateUI.Interval = 2000;
      timer_UpdateUI.Elapsed += Timer_UpdateUI_Elapsed;
      timer_UpdateUI.Start();
    }

    private SumaryDTO _sumaryDTOLine3 { get; set; }
    private SumaryDTO _sumaryDTOLine4 { get; set; }
    private void Timer_UpdateUI_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      var data03 = AppCore.Ins._datalogsInShiftCurrent?.Where(x => x.LineId == 3).ToList();
      var data04 = AppCore.Ins._datalogsInShiftCurrent?.Where(x => x.LineId == 3).ToList();

      _sumaryDTOLine4 = AppCore.Ins.SumaryDTOData(data03, AppCore.Ins._productCurrent04, AppCore.Ins._tareSettingCurrent04);
    }


    private void LoadDataDashBoard()
    {
      try
      {
        if (AppCore.Ins._datalogsInShiftCurrent == null || AppCore.Ins._datalogsInShiftCurrent?.Count() == 0)
        {
          ResetDashboard();
          return;
        }

        GetDt();

        var list = AppCore.Ins._datalogsInShiftCurrent;
        sumaryDTO = AppCore.Ins.SumaryDTOData(list, AppCore.Ins._productCurrent04, AppCore.Ins._tareSettingCurrent04);

        //Data time
        dataChartline = sumaryDTO.DatalogPass
                        .Where(x => x.CreatedAt >= _from && x.CreatedAt <= _to)
                        .OrderBy(x => x.CreatedAt)
                        .ToList();
        //dataTimeData = sumaryDTO.DatalogPass.Select(x => x.CreatedAt.ToString()).ToList();


        //dataTimeData = sumaryDTO.DatalogPass.Select(x => x.CreatedAt.ToString()).ToList();

        //Data reject
        reject = new List<DataRejectDTO>();
        if (sumaryDTO.DatalogReject?.Count() > 0)
        {
          foreach (var data in sumaryDTO.DatalogReject)
          {
            DataRejectDTO dataReject = new DataRejectDTO();
            dataReject.DateTime = (DateTime)data.CreatedAt;
            dataReject.FGs = AppCore.Ins._productCurrent04.Code;
            dataReject.Actual = data.Gross;
            dataReject.Target = sumaryDTO.Target;
            reject.Add(dataReject);
          }
        }

        UpdateDataUI(true);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }


    
    private void ResetDashboard()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetDashboard();
        }));
        return;
      }

      try
      {
        sumaryDTO = new SumaryDTO();
        reject = new List<DataRejectDTO>();
        UpdateDataUI(true);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
    }

  }
}
