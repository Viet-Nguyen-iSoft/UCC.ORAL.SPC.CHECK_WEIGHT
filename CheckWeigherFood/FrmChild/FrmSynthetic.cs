using Aspose.Cells.DigitalSignatures;
using CheckWeigherFood.Controls;
using CheckWeigherFood.Models;
using DocumentFormat.OpenXml.Office2013.Word;
using LiveCharts.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmSynthetic : Form
  {
    public FrmSynthetic()
    {
      InitializeComponent();
    }
    #region Singleton parttern
    private static FrmSynthetic _Instance = null;
    public static FrmSynthetic Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmSynthetic();
        }
        return _Instance;
      }
    }


    #endregion


    List<Datalog> datalogs = new List<Datalog>();
    private DateTime fromDate;
    private DateTime toDate;
    Dictionary<string, List<Datalog>> DATA = new Dictionary<string, List<Datalog>>();
    private int ShiftId = 0;
    private string FGsFind = "";
    private string fileDB1 = "";
    private string fileDB2 = "";

    private FrmLoading frmLoading = new FrmLoading();
    private void btnPreview_Click(object sender, EventArgs e)
    {
      frmLoading = new FrmLoading();
      frmLoading.ShowLoading("Loading Data ...");


      ShiftId = this.cbShift.SelectedIndex;
      FGsFind = this.txtFGs.Text.Trim();

      if (!backgroundWorker1.IsBusy)
      {
        this.btnPreview.Visible = false;
        backgroundWorker1.RunWorkerAsync();
      }

      #region V1
      /*
       * 
       * 
       * this.flowLayoutPanel.Controls.Clear();
      this.flowLayoutPanel.Visible = false;
      ResetDgv();

      DATA = new Dictionary<string, List<Datalog>>();
      
      TimeSpan khoangCachThoiGian = toDate - fromDate;
      int NumberDay = khoangCachThoiGian.Days + 1;

      for (int i = 0; i < NumberDay; i++)
      {
        string fileDB1 = Application.StartupPath + $"\\DataBase\\{fromDate.AddDays(i).ToString("yyMMdd")}.sqlite";
        string fileDB2 = Application.StartupPath + $"\\DataBase\\{fromDate.AddDays(i+1).ToString("yyMMdd")}.sqlite";
        //if (!File.Exists(fileDB1)) continue;

        datalogs = new List<Datalog>();
        List<Datalog> dataFile1 = new List<Datalog>();
        List<Datalog> dataFile2 = new List<Datalog>();
        //select All Shift
        if (this.cbShift.SelectedIndex == 3)
        {
          if (txtFGs.Text.Trim() == "")
          {
            dataFile1 = await AppCore.Ins.GetDataReportByFilter(-1, fileDB1);
            dataFile2 = await AppCore.Ins.GetDataReportByFilter(-1, fileDB2);

            dataFile1 = dataFile1.Where(x => x.CreatedAt.Value.Hour >= 6).ToList();
            dataFile2 = dataFile2.Where(x => x.CreatedAt.Value.Hour < 6).ToList();

            datalogs.AddRange(dataFile1.OrderBy(x => x.CreatedAt).ToList());
            datalogs.AddRange(dataFile2.OrderBy(x => x.CreatedAt).ToList());
          }
          else
          {
            int idProduct = AppCore.Ins._listMasterData.Where(s => s.FGs == this.txtFGs.Text.Trim()).Select(x => x.Id).LastOrDefault();

            dataFile1 = await AppCore.Ins.GetDataReportByFilter(idProduct, fileDB1);
            dataFile2 = await AppCore.Ins.GetDataReportByFilter(idProduct, fileDB2);

            dataFile1 = dataFile1.Where(x => x.CreatedAt.Value.Hour >= 6).ToList();
            dataFile2 = dataFile2.Where(x => x.CreatedAt.Value.Hour < 6).ToList();

            datalogs.AddRange(dataFile1.OrderBy(x => x.CreatedAt).ToList());
            datalogs.AddRange(dataFile2.OrderBy(x => x.CreatedAt).ToList());
          }
        }
        //Select Shift Any
        else
        {
          if (this.txtFGs.Text.Trim() == "")
          {
            dataFile1 = await AppCore.Ins.GetDataReportByFilter(-1, fileDB1);
            dataFile2 = await AppCore.Ins.GetDataReportByFilter(-1, fileDB2);

            dataFile1 = dataFile1.Where(x => x.CreatedAt.Value.Hour >= 6 & x.ShiftId == cbShift.SelectedIndex + 1).ToList();
            dataFile2 = dataFile2.Where(x => x.CreatedAt.Value.Hour < 6 & x.ShiftId == cbShift.SelectedIndex + 1).ToList();
            datalogs.AddRange(dataFile1.OrderBy(x => x.CreatedAt).ToList());
            datalogs.AddRange(dataFile2.OrderBy(x => x.CreatedAt).ToList());
          }
          else
          {
            int idProduct = AppCore.Ins._listMasterData.Where(s => s.FGs == this.txtFGs.Text.Trim()).Select(x => x.Id).LastOrDefault();

            dataFile1 = await AppCore.Ins.GetDataReportByFilter(idProduct, fileDB1);
            dataFile2 = await AppCore.Ins.GetDataReportByFilter(idProduct, fileDB2);

            dataFile1 = dataFile1.Where(x => x.CreatedAt.Value.Hour >= 6 & x.ShiftId == cbShift.SelectedIndex + 1).ToList();
            dataFile2 = dataFile2.Where(x => x.CreatedAt.Value.Hour < 6 & x.ShiftId == cbShift.SelectedIndex + 1).ToList();

            datalogs.AddRange(dataFile1.OrderBy(x => x.CreatedAt).ToList());
            datalogs.AddRange(dataFile2.OrderBy(x => x.CreatedAt).ToList());
          }
        }
        DATA.Add(fromDate.AddDays(i).ToString("dd/MM/yyyy"), datalogs);
      }

      //if (DATA.Values.Count == 1)
      //{
      //  flowLayoutPanel.Visible = false;
      //  ResetDgv();
      //  new FrmInformation().ShowMessage("Không có dữ liệu trong khoảng thời gian này !", eNum.eNumUI.eImage.Warning);
      //  return;
      //}


      if (DATA!=null && DATA.Values.Count>0)
      {
        this.flowLayoutPanel.Visible = true;
        List<Datalog> data = new List<Datalog>();
        data = DATA.Values.SelectMany(list => list).ToList();
        //CreateButtonWithFGs(data, "Tất cả", true);
        foreach (var item in DATA)
        {
          if (item.Value.Count>0)
            CreateButtonWithFGs(item.Value, item.Key);
        }
        if (flowLayoutPanel.Controls.Count > 0 && flowLayoutPanel.Controls[0] is Button)
        {
          Button firstButton = (Button)flowLayoutPanel.Controls[0];
          firstButton.PerformClick();
        }
      }  
      else
      {
        flowLayoutPanel.Visible = false;
        ResetDgv();
        new FrmInformation().ShowMessage("Không có dữ liệu trong khoảng thời gian này !", eNum.eNumUI.eImage.Warning);
      }  
       * */


      #endregion
    }

    private void UpdateUI(List<Datalog> datas)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          UpdateUI(datas);
        }));
        return;
      }
      this.btnPreview.Visible = true;
      dgvDataProducts.DataSource = ConverDataToDataFull(datas);
      SetDgvTitle();
    }


    private List<Synthetic> ConverDataToDataFull(List<Datalog> DataIn)
    {
      if (DataIn == null) return null;

      List<Synthetic> DataOut = new List<Synthetic>();
      for (int i = 0; i < DataIn.Count; i++)
      {
        Synthetic data = new Synthetic();
        data.STT = i + 1;
        data.DateTime = (DataIn[i].CreatedAt)?.ToString("dd/MM/yyyy HH:mm:ss");
        data.Shift = $"Shift {DataIn[i].ShiftId}";
        data.OP = DataIn[i].OP;
        data.QC= DataIn[i].QC;
        data.TC = DataIn[i].TC;
        data.CodeFGs =(AppCore.Ins._listMasterData!=null)? AppCore.Ins._listMasterData.Where(x => x.Id == DataIn[i].ProductId).Select(x=>x.FGs).FirstOrDefault():"NAN";
        data.Description = (AppCore.Ins._listMasterData != null) ? AppCore.Ins._listMasterData.Where(x => x.Id == DataIn[i].ProductId).Select(x => x.Description).FirstOrDefault() : "NAN";
        data.LoBB = DataIn[i].LoBB;
        data.Gross = DataIn[i].Gross;
        data.Target = (AppCore.Ins._listMasterData != null) ? AppCore.Ins._listMasterData.Where(x => x.Id == DataIn[i].ProductId).Select(x => x.Target).FirstOrDefault() : -1;
        data.Status = DataIn[i].Status;
        DataOut.Add(data);
      }
      return DataOut;
    }




    private void ResetDgv()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetDgv();
        }));
        return;
      }
      dgvDataProducts.DataSource = null;
      this.btnPreview.Visible = true;
    }

    private void CreateButtonWithFGs(List<Datalog> dataHistoricals,string title, bool isAll = false)
    {
      Button button = new System.Windows.Forms.Button();
      button.BackColor = (isAll == false) ? Color.FromArgb(72, 61, 139) : Color.FromArgb(204, 102, 255);
      button.FlatAppearance.BorderColor = Color.FromArgb(102, 102, 153);
      button.FlatAppearance.BorderSize = 5;
      button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular);
      button.ForeColor = System.Drawing.Color.White;
      button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      button.Location = new System.Drawing.Point(3, 3);
      button.Name = title;
      button.Size = new System.Drawing.Size(150, 50);
      button.TabIndex = 16;
      button.Text = title;
      button.Text = (isAll == false) ? title : "Tất cả";
      button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      button.UseVisualStyleBackColor = false;
      button.Tag = dataHistoricals;
      button.Click += EvenButton_Click;
      this.flowLayoutPanel.Controls.Add(button);
    }

    private void EvenButton_Click(object sender, EventArgs e)
    {
      ActiveColor(sender);
    }

    private void ActiveColor(object sender)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ActiveColor(sender);
        }));
        return;
      }

      if (sender is Button)
      {
        Button button = (Button)sender;
        for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
        {
          if (this.flowLayoutPanel1.Controls[i] is Button)
          {
            Button localButton = (Button)(this.flowLayoutPanel1.Controls[i]);
            if (localButton != null)
            {
              localButton.BackColor = Color.FromArgb(72, 61, 139);
            }
          }
        }

        if (button.Tag != null)
        {
          button.BackColor = Color.FromArgb(204, 102, 255);
          dgvDataProducts.DataSource = ConverDataToDataFull( (List<Datalog>)button.Tag);
          SetDgvTitle();
        }
      }
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
      fromDate = (DateTime)dtpFrom.Value;
    }

    private void FrmSynthetic_Load(object sender, EventArgs e)
    {
      this.dtpFrom.Value = DateTime.Now;
      this.cbShift.SelectedIndex = 3;
      flowLayoutPanel.Visible = false;
    }

    private void SetDgvTitle()
    {
      int i = 0;
      this.dgvDataProducts.Columns[i++].HeaderText = "STT";
      this.dgvDataProducts.Columns[i++].HeaderText = "DateTime";
      this.dgvDataProducts.Columns[i++].HeaderText = "Shift";
      this.dgvDataProducts.Columns[i++].HeaderText = "OP Name";
      this.dgvDataProducts.Columns[i++].HeaderText = "QC Name";
      this.dgvDataProducts.Columns[i++].HeaderText = "TC Name";
      this.dgvDataProducts.Columns[i++].HeaderText = "Code FGs";
      this.dgvDataProducts.Columns[i++].HeaderText = "Name FGs";
      this.dgvDataProducts.Columns[i++].HeaderText = "Lô bao bì";
      this.dgvDataProducts.Columns[i++].HeaderText = "Net";
      this.dgvDataProducts.Columns[i++].HeaderText = "Target";
      this.dgvDataProducts.Columns[i++].HeaderText = "Status";

      this.dgvDataProducts.Columns[0].Width = 60;
      this.dgvDataProducts.Columns[1].Width = 170;
      this.dgvDataProducts.Columns[2].Width = 80;
      this.dgvDataProducts.Columns[3].Width = 170;
      this.dgvDataProducts.Columns[4].Width = 170;
      this.dgvDataProducts.Columns[5].Width = 170;
      this.dgvDataProducts.Columns[6].Width = 100;
      this.dgvDataProducts.Columns[7].Width = 300;
      this.dgvDataProducts.Columns[8].Width = 90;
      this.dgvDataProducts.Columns[9].Width = 90;
      this.dgvDataProducts.Columns[10].Width = 90;
      this.dgvDataProducts.Columns[11].Width = 60;
    }

    private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        datalogs = new List<Datalog>();

        string pathDatabase = Application.StartupPath + $"\\DataBase\\";
        string fileDB = fromDate.ToString("yyMMdd");
        string pathFull = pathDatabase + fileDB + ".sqlite";

        if (!File.Exists(pathFull)) return;


        if (ShiftId == 3)
        {
          if (FGsFind == "")
          {
            datalogs = await AppCore.Ins.GetDataReportByFilter(-1, fileDB);
          }
          else
          {
            if (AppCore.Ins._listMasterData != null)
            {
              int idProduct = AppCore.Ins._listMasterData.Where(s => s.FGs == FGsFind).Select(x => x.Id).LastOrDefault();
              datalogs = await AppCore.Ins.GetDataReportByFilter(idProduct, fileDB);
            }
          }
        }
        //Select Shift Any
        else
        {
          if (FGsFind == "")
          {
            datalogs = await AppCore.Ins.GetDataReportByFilter(-1, fileDB);
            datalogs = datalogs?.Where(s => s.ShiftId == ShiftId + 1).ToList();
          }
          else
          {
            if (AppCore.Ins._listMasterData != null)
            {
              int idProduct = AppCore.Ins._listMasterData.Where(s => s.FGs == FGsFind).Select(x => x.Id).LastOrDefault();
              datalogs = await AppCore.Ins.GetDataReportByFilter(idProduct, fileDB1);
              datalogs = datalogs?.Where(s => s.ShiftId == ShiftId + 1).ToList();
            }
          }
        }
      }
      catch (Exception)
      {
      }

    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      try
      {
        flowLayoutPanel.Controls.Clear();
        if (datalogs.Count > 0)
        {
          var groupedByFGs = datalogs.GroupBy(x => x.ProductId);
          foreach (var item in groupedByFGs)
          {
            if (item.ToList().Count > 0)
            {
              flowLayoutPanel.Visible = true;
              var Datas = item.ToList();
              var FGs = (AppCore.Ins._listMasterData != null) ? AppCore.Ins._listMasterData.Where(x => x.Id == item.Key).Select(x => x.FGs).FirstOrDefault() : "NAN";
              CreateButtonWithFGs(Datas, FGs.ToString());
            }
          }
          if (flowLayoutPanel.Controls.Count > 0 && flowLayoutPanel.Controls[0] is Button)
          {
            Button firstButton = (Button)flowLayoutPanel.Controls[0];
            firstButton.PerformClick();
          }
        }
        else
        {
          frmLoading.CloseLoading();
          flowLayoutPanel.Visible = false;
          ResetDgv();
          new FrmInformation().ShowMessage("Không có dữ liệu trong khoảng thời gian này !", eNum.eNumUI.eImage.Warning);
        }
      }
      catch (Exception)
      {

      }
      finally
      {
        this.btnPreview.Visible = true;
        frmLoading.CloseLoading();
      }
      
    }


  }
}
