using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static CheckWeigherFood.eNum.eNumUI;
using Color = System.Drawing.Color;
using DataTable = System.Data.DataTable;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmReport : Form
  {
    public FrmReport()
    {
      InitializeComponent();
    }

    #region Singleton parttern
    private static FrmReport _Instance = null;
    public static FrmReport Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmReport();
        }
        return _Instance;
      }
    }
    #endregion

    private DataChart _dataChart = new DataChart();
    private void FrmReport_Load(object sender, EventArgs e)
    {
      _dataChart.ChartControlInit(chartControl);
      _dataChart.ChartHistogramInit(chartHistogram);
      _dataChart.ChartPieInit(chartPie);


      this.dtpFrom.Value = DateTime.Now;
      this.cbShift.SelectedIndex = 3;
      this.flowLayoutPanelProductReport.Visible = false;

      
    }



    private DateTime fromDate;
    private DateTime toDate;
    Dictionary<string, List<Datalog>> DATA = new Dictionary<string, List<Datalog>>();


    private FrmLoading frmLoading = new FrmLoading();
    private System.Timers.Timer timerLoading = new System.Timers.Timer();
    private void btnPreview_Click(object sender, EventArgs e)
    {
      frmLoading.ShowLoading("Loading Data ...");

      ShiftId = this.cbShift.SelectedIndex;
      FGsFind = this.txtFGs.Text.Trim();

      if (!backgroundWorker1.IsBusy)
      {
        this.btnPreview.Visible = false;
        backgroundWorker1.RunWorkerAsync();
      }
    }

    private void CreateButtonWithFGs(List<Datalog> dataHistoricals, string title, bool isAll = false)
    {
      Button button = new System.Windows.Forms.Button();
      button.BackColor = (isAll == false) ? Color.FromArgb(72, 61, 139) : Color.FromArgb(204, 102, 255);
      button.FlatAppearance.BorderColor = Color.FromArgb(102, 102, 153);
      button.FlatAppearance.BorderSize = 3;
      button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular);
      button.ForeColor = System.Drawing.Color.White;
      button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      button.Location = new System.Drawing.Point(3, 3);
      button.Name = title;
      button.Size = new System.Drawing.Size(300, 40);
      button.TabIndex = 16;
      button.Text = title;
      button.Text = (isAll == false) ? title : "Tất cả";
      button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      button.UseVisualStyleBackColor = false;
      button.Tag = dataHistoricals;
      button.Click += EvenButton_Click;
      this.flowLayoutPanelProductReport.Controls.Add(button);
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
        for (int i = 0; i < this.flowLayoutPanelProductReport.Controls.Count; i++)
        {
          if (this.flowLayoutPanelProductReport.Controls[i] is Button)
          {
            Button localButton = (Button)(this.flowLayoutPanelProductReport.Controls[i]);
            if (localButton != null)
            {
              localButton.BackColor = Color.FromArgb(72, 61, 139);
            }
          }
        }

        if (button.Tag != null)
        {
          button.BackColor = Color.FromArgb(204, 102, 255);
          CalDatalog((List<Datalog>)button.Tag);
        }
      }
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

      this.lbKhoiLuongTinh_report.Text = "NAN";
      this.lb1T_report.Text = "0";

      this.lbUpper2T_report.Text = "0";
      this.lbUpper1T_report.Text = "0";
      this.lbLower1T_report.Text = "0";
      this.lbLower2T_report.Text = "0";
      this.lbTarget_report.Text = "0";

      this.lbSample_report.Text = "0";
      this.lbXtb_report.Text = "0";
      this.lbMax_report.Text = "0";
      this.lbMin_report.Text = "0";
      this.lbCpk_report.Text = "0";
      this.lbCp_report.Text = "0";
      this.lbOw_report.Text = "0";
      this.lbTLTB_report.Text = "0";

      this.lbOw_report.BackColor = Color.FromArgb(40, 167, 68);
      this.lbResult_report.BackColor = Color.FromArgb(40, 167, 68);
      this.lbResult_report.Text = "PASS";

      _dataChart.AddChartControl(chartControl, null, null, 0, 0, 0, 0, 0, 0);
      _dataChart.AddChartHistogram(chartHistogram, null, 0, 0, 0, 0, 0, 0, 0, 0,0);
      _dataChart.SetDataChartPie(chartPie, 0, 0, 0);
      dgvData.DataSource = null;
    }

    private double ProductMin = 0;
    private double ProductMax = 0;
    private double ProductLowerControl = 0;
    private double ProductUpperControl = 0;
    private double ProductTarget;
    private double ProductValueT;
    private string SKU;

    private int Sample = 0;
    private int NumbersOk = 0;
    private int NumbersOver = 0;
    private int NumbersReject = 0;
    private double Mean = 0;
    private double Std = 0;
    private double MinValue = 0;
    private double MaxValue = 0;
    private double Cp = 0;
    private double Cpk = 0;
    private double OW = 0;

    private List<Datalog> datalogs = new List<Datalog>();
    private List<double> valueNetPass = new List<double>();
    private List<double> valueNetOk = new List<double>();
    private List<double> valueNetOver = new List<double>();
    private List<double> valueNetReject = new List<double>();
    private List<string> dataTimeData= new List<string>();
    private List<Datalog> listDataReport = new List<Datalog>();

    private void UpdateUI(List<ListDataLogs> datas)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          UpdateUI(datas);
        }));
        return;
      }
      this.lbKhoiLuongTinh_report.Text = SKU;
      this.lb1T_report.Text = ProductValueT.ToString();

      this.lbUpper2T_report.Text = ProductMax.ToString();
      this.lbUpper1T_report.Text= ProductUpperControl.ToString();
      this.lbLower1T_report.Text= ProductLowerControl.ToString();
      this.lbLower2T_report.Text= ProductMin.ToString();
      this.lbTarget_report.Text= ProductTarget.ToString();

      this.lbSample_report.Text = Sample.ToString();
      this.lbXtb_report.Text= Mean.ToString();
      this.lbMax_report.Text= MaxValue.ToString();
      this.lbMin_report.Text= MinValue.ToString();
      this.lbCpk_report.Text= Cpk.ToString();
      this.lbCp_report.Text= Cp.ToString();
      this.lbOw_report.Text= OW.ToString();
      this.lbTLTB_report.Text = Mean.ToString();

      this.lbOw_report.BackColor = (OW >= 0.5) ? Color.Red : Color.FromArgb(40, 167, 68);
      this.lbResult_report.BackColor = (Mean < ProductTarget) ? Color.Red : Color.FromArgb(40, 167, 68);
      this.lbResult_report.Text = (Mean < ProductTarget) ? "FAIL" : "PASS";


      _dataChart.AddChartHistogram(chartHistogram, valueNetPass, ProductMax, ProductUpperControl, Mean, Std, ProductLowerControl, ProductMin, MinValue, MaxValue, ProductTarget);
      _dataChart.SetDataChartPie(chartPie, NumbersOk, NumbersOver, NumbersReject);
      AddChartControl(chartControl, valueNetPass, dataTimeData, ProductMax, ProductUpperControl, ProductTarget, ProductLowerControl, ProductMin, MaxValue);


      this.dgvData.DataSource = null ;
      this.dgvData.DataSource = datas;
      SetWidthTitle();
    }

    public void AddChartControl(Chart chartName, List<double> dataY, List<string> dataX, double up2T, double up1T, double target, double lo1T, double lo2T, double max)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          AddChartControl(chartName, dataY, dataX, up2T, up1T, target, lo1T, lo2T, max);
        }));
        return;
      }

      try
      {
        if (dataX == null || dataY == null || dataX.Count == 0 || dataY.Count == 0)
        {
          chartName.Series[0].Points.Clear();
          chartName.Series[1].Points.Clear();
          chartName.Series[2].Points.Clear();
          chartName.Series[3].Points.Clear();
          chartName.Series[4].Points.Clear();
          chartName.Series[5].Points.Clear();
          return;
        }

   
        chartName.ChartAreas[0].AxisY.Maximum = (max < up2T) ? up2T + 5 : up2T * 1.01;
        chartName.ChartAreas[0].AxisY.Minimum = lo2T - 5;

        chartName.Series[0].Points.Clear();
        chartName.Series[1].Points.Clear();
        chartName.Series[2].Points.Clear();
        chartName.Series[3].Points.Clear();
        chartName.Series[4].Points.Clear();
        chartName.Series[5].Points.Clear();
        chartName.ChartAreas[0].AxisX.CustomLabels.Clear();

        for (int i = 0; i < dataX.Count(); i++)
        {
          chartName.Series[0].Points.AddXY(i, dataY[i]);
        }


        chartName.Series[1].Points.AddXY(0, up2T);
        chartName.Series[1].Points.AddXY(dataX.Count() - 1, up2T);

        chartName.Series[2].Points.AddXY(0, up1T);
        chartName.Series[2].Points.AddXY(dataX.Count() - 1, up1T);

        chartName.Series[3].Points.AddXY(0, target);
        chartName.Series[3].Points.AddXY(dataX.Count() - 1, target);

        chartName.Series[4].Points.AddXY(0, lo1T);
        chartName.Series[4].Points.AddXY(dataX.Count() - 1, lo1T);

        chartName.Series[5].Points.AddXY(0, lo2T);
        chartName.Series[5].Points.AddXY(dataX.Count() - 1, lo2T);

        List<int> selectedPoints = GetEquallySpacedPoints(chartName.Series[0].Points.Count(), 10);
        chartName.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

        for (int i = 0; i < selectedPoints.Count; i++)
        {
          int indexOfFirstSpace = dataX[selectedPoints[i]].IndexOf(' ');
          string timeOnly = dataX[selectedPoints[i]].Substring(indexOfFirstSpace + 1);
          chartName.ChartAreas[0].AxisX.CustomLabels.Add(selectedPoints[i], selectedPoints[i] + 2000, timeOnly);
        }

        chartName.Invalidate();

        this.btnPreview.Visible = true;
        this.frmLoading.CloseLoading();
      }
      catch (Exception)
      {
      }

    }


    private List<int> GetEquallySpacedPoints(int Total , int numberOfPointsToSelect)
    {
      List<int> selectedPoints = new List<int>();

      double step = (double)Total / numberOfPointsToSelect;

      for (int i = 0; i < numberOfPointsToSelect; i++)
      {
        int index = (int)Math.Round(i * step);
        selectedPoints.Add(index);
      }
      return selectedPoints;
    }

    private void SetWidthTitle()
    {
      this.dgvData.Columns[0].Width = 50;
      this.dgvData.Columns[1].Width = 180;
      this.dgvData.Columns[2].Width = 70;
      this.dgvData.Columns[3].Width = 200;
      this.dgvData.Columns[4].Width = 200;
      this.dgvData.Columns[5].Width = 200;
      this.dgvData.Columns[6].Width = 100;
      this.dgvData.Columns[7].Width = 350;
      this.dgvData.Columns[8].Width = 110;
      this.dgvData.Columns[10].Width = 90;
    }

    private void CalDatalog(List<Datalog> DataIn)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          CalDatalog(DataIn);
        }));
        return;
      }

      if (DataIn == null) return;

      MasterData dataProduct = (AppCore.Ins._listMasterData != null) ? AppCore.Ins._listMasterData.Where(x => x.Id == DataIn[0].ProductId).FirstOrDefault() : null;
      if (dataProduct == null) return;

      ProductTarget = dataProduct.Target;
      SKU = dataProduct.SKU;
      ProductValueT = dataProduct.ValueT;
      ProductMax = dataProduct.Max;
      ProductUpperControl = dataProduct.UpperControl;
      ProductLowerControl = dataProduct.LowerControl;
      ProductMin= dataProduct.Min;

      listDataReport = DataIn.Where(s => s.Status == "Ok" || s.Status == "Over").ToList();
      valueNetPass = DataIn.Where(s => s.Status == "Ok" || s.Status == "Over").Select(x => x.Net).ToList();
      valueNetOk = DataIn.Where(s => s.Status == "Ok").Select(x => x.Net).ToList();
      valueNetOver = DataIn.Where(s => s.Status == "Over").Select(x => x.Net).ToList();
      valueNetReject = DataIn.Where(s => s.Status == "Reject").Select(x => x.Net).ToList();
      dataTimeData = DataIn.Where(s => s.Status == "Ok" || s.Status == "Over").Select(x => x.CreatedAt.ToString()).ToList();

      NumbersOk = valueNetOk.Count;
      NumbersOver = valueNetOver.Count;
      NumbersReject = valueNetReject.Count;

      Sample = valueNetPass.Count;
      Mean = CalMean(valueNetPass);
      Std = CalStdDev(valueNetPass);
      MinValue = valueNetPass.Min();
      MaxValue = valueNetPass.Max();

      Cp = (Std != 0) ? Math.Round(((MaxValue - MinValue) / (6 * Std)), 3) : 0;
      double hcpk = (Std != 0) ? ((MaxValue - Mean) / (3 * Std)) : 0;
      double lcpk = (Std != 0) ? ((Mean - MinValue) / (3 * Std)) : 0;
      Cpk = Math.Round(Math.Min(hcpk, lcpk), 3);
      OW = (ProductTarget != 0) ? Math.Round(((Mean - ProductTarget) / ProductTarget) * 100, 2) : 0;


      listDataReport = DataIn;
      List<ListDataLogs> DataOut = new List<ListDataLogs>();
      for (int i = 0; i < listDataReport.Count; i++)
      {
        ListDataLogs data = new ListDataLogs();
        data.STT = i + 1;
        data.DateTime = (DateTime)listDataReport[i].CreatedAt;
        data.Shift = $"Shift {listDataReport[i].ShiftId}";
        data.OP = listDataReport[i].OP;
        data.QC = listDataReport[i].QC;
        data.TC = listDataReport[i].TC;
        data.CodeFGs = dataProduct.FGs;
        data.Description = dataProduct.Description;
        data.LoBB = listDataReport[i].LoBB;
        data.Net = listDataReport[i].Gross;
        data.Target = dataProduct.Target;
        //data.OW = OW;
        //data.Cp = Cp;
        //data.Cpk= Cpk;
        //data.In = DataIn.Count();
        //data.Out = listDataReport.Count();
        data.Reject = (listDataReport[i].Gross < ProductMin) ? 1 : 0;
        data.Over = (listDataReport[i].Gross > ProductMax) ? 1 : 0;
        DataOut.Add(data);
      }

      UpdateUI(DataOut);
    }



    #region Cal Tính toán Mean, Std
    private double CalMean(List<double> list_data)
    {
      double x_tb = 0;
      foreach (var item in list_data)
      {
        x_tb += item;
      }
      return Math.Round(x_tb / list_data.Count, 2);
    }

    private double CalStdDev(List<double> list_data)
    {
      double mean_x_tb = CalMean(list_data);
      double sumOfSquares = 0;
      foreach (double data_x in list_data)
        sumOfSquares += Math.Pow(data_x - mean_x_tb, 2);
      double stdDev = Math.Sqrt(sumOfSquares / (list_data.Count - 1));
      return Math.Round(stdDev, 2);
    }

    #endregion

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
      fromDate = (DateTime)dtpFrom.Value;
    }

    private string fileName = "";
    private void btnReport_Click(object sender, EventArgs e)
    {
      try
      {
        this.btnReport.Visible = false;


        // Load file template
        string templatePath = $@"{Application.StartupPath}\Template\FormatExcel.xlsx";
        XLWorkbook workbook = new XLWorkbook(templatePath);
        IXLWorksheet worksheet = workbook.Worksheet("Report");

        // Lấy dữ liệu từ DataGridView
        DataTable dataTable = new DataTable();
        foreach (DataGridViewColumn column in dgvData.Columns)
        {
          dataTable.Columns.Add(column.HeaderText);
        }
        foreach (DataGridViewRow row in dgvData.Rows)
        {
          DataRow dataRow = dataTable.NewRow();
          foreach (DataGridViewCell cell in row.Cells)
          {
            dataRow[cell.ColumnIndex] = cell.Value;
          }
          dataTable.Rows.Add(dataRow);
        }
        worksheet.Cell("A50").InsertTable(dataTable);

        string imagePath = "";
        // Chart Control
        Bitmap bitmap = new Bitmap(tableLayoutPanel37.Width, tableLayoutPanel37.Height);
        tableLayoutPanel37.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel37.Width, tableLayoutPanel37.Height));

        imagePath = "chart1.png";
        bitmap.Save(imagePath);
        var pictureChartControl = worksheet.Pictures.Add(imagePath);
        pictureChartControl.MoveTo(worksheet.Cell(23, 1));
        pictureChartControl.WithSize(1930, 500);

        //tableLayoutPanel24
        bitmap = new Bitmap(tableLayoutPanel24.Width, tableLayoutPanel24.Height);
        tableLayoutPanel24.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel24.Width, tableLayoutPanel24.Height));
        imagePath = "chart2.png";
        bitmap.Save(imagePath);
        var pictureChartPie = worksheet.Pictures.Add(imagePath);
        pictureChartPie.MoveTo(worksheet.Cell(10, 1));
        pictureChartPie.WithSize(1930, 300);

       

        using (var saveFD = new SaveFileDialog())
        {
          saveFD.Filter = "Excel|*.xlsx|All files|*.*";
          saveFD.Title = "Save report to excel file";
          saveFD.FileName = $"DataReport{fromDate.ToString("_dd_MM_yyyy")}_{cbShift.SelectedItem.ToString().Trim()}";
          DialogResult dialogResult = saveFD.ShowDialog();
          if (dialogResult == DialogResult.OK) fileName = saveFD.FileName; //lay duong dan luu file
          else return; //huy report neu chon cancel
        }
        workbook.SaveAs(fileName);

        FrmConfirm frmConfirm = new FrmConfirm("Xuất report thành công !\n Bạn có muốn mở file bây giờ ?", eImage.Question);
        frmConfirm.OnSendOKClicked += FrmConfirm_OnSendOKClicked;
        frmConfirm.ShowDialog();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        this.btnReport.Visible = true;
      }
    }

    private void FrmConfirm_OnSendOKClicked(object sender)
    {
      try
      {
        Process.Start(fileName);
      }
      catch (Exception)
      {
      }
    }




    private int ShiftId = 0;
    private string FGsFind = "";
    private string fileDB1 = "";

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
            if (AppCore.Ins._listMasterData!=null)
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
              datalogs = datalogs?.Where(s=>s.ShiftId == ShiftId + 1).ToList();
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
      LoadDataUI();
    }


    private void LoadDataUI()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          LoadDataUI();
        }));
        return;
      }

      try
      {
        flowLayoutPanelProductReport.Controls.Clear();
        if (datalogs.Count > 0)
        {
          this.btnReport.Visible = true;
          var groupedByFGs = datalogs.GroupBy(x => x.ProductId);
          foreach (var item in groupedByFGs)
          {
            if (item.ToList().Count > 0)
            {
              flowLayoutPanelProductReport.Visible = true;
              var Datas = item.ToList();
              var FGs = (AppCore.Ins._listMasterData != null) ? AppCore.Ins._listMasterData.Where(x => x.Id == item.Key).Select(x => x.FGs).FirstOrDefault() : "NAN";
              CreateButtonWithFGs(Datas, FGs.ToString());
            }
          }
          if (flowLayoutPanelProductReport.Controls.Count > 0 && flowLayoutPanelProductReport.Controls[0] is Button)
          {
            Button firstButton = (Button)flowLayoutPanelProductReport.Controls[0];
            firstButton.PerformClick();
          }
        }
        else
        {
          frmLoading.CloseLoading();
          flowLayoutPanelProductReport.Visible = false;
          this.btnReport.Visible = false;
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

    private void cbShift_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
  }
}
