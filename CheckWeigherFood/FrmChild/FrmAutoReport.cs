using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
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
using static CheckWeigherFood.eNum.eNumUI;
using Color = System.Drawing.Color;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmAutoReport : Form
  {
    public FrmAutoReport()
    {
      InitializeComponent();
    }

    private int IdProduct = 0;
    private int ShiftId = 0;
    private DataChart _dataChart = new DataChart();
    public FrmAutoReport(int shiftId, int idProduct)
    {
      InitializeComponent();
      IdProduct = idProduct;
      ShiftId = shiftId;
    }

    private void FrmAutoReport_Load(object sender, EventArgs e)
    {
      _dataChart.ChartControlInit(chartControl);
      _dataChart.ChartHistogramInit(chartHistogram);
      LoadDatalog();
    }

   
    private DateTime dt = new DateTime();
    private async void LoadDatalog()
    {
      try
      {
        dt = (ShiftId == 3) ? DateTime.Now.AddDays(-1) : DateTime.Now;
        datalogs = new List<Datalog>();

        string path = Application.StartupPath + $"\\DataBase\\";
        string fileDB = dt.ToString("yyMMdd");
        if (dt.Hour>=0 && dt.Hour<6)
        {
          fileDB = dt.AddDays(-1).ToString();
        }

        string pathFull = path + fileDB + ".sqlite";
        if (File.Exists(pathFull))
        {
          datalogs = await AppCore.Ins.GetDataReportByFilter(-1, fileDB);
          datalogs = datalogs?.Where(s=>s.ShiftId == ShiftId).ToList();
        }  

        if (datalogs.Count > 0)
        {
          string OP_Sure = datalogs.LastOrDefault().OP;
          string QC_Sure = datalogs.LastOrDefault().QC;
          string TC_Sure = datalogs.LastOrDefault().TC;
          string LoBB_Sure = datalogs.LastOrDefault().LoBB;
          datalogs.ForEach(x => x.OP = OP_Sure);
          datalogs.ForEach(x => x.QC = QC_Sure);
          datalogs.ForEach(x => x.OP = TC_Sure);
          datalogs.ForEach(x => x.LoBB = LoBB_Sure);

          CalDatalog(datalogs);
        }
        else
        {
          this.Close();
        }  
      }
      catch (Exception)
      {
      }
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
    private List<string> dataTimeData = new List<string>();
    private List<Datalog> listDataReport = new List<Datalog>();

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
      ProductMin = dataProduct.Min;

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
      this.lbUpper1T_report.Text = ProductUpperControl.ToString();
      this.lbLower1T_report.Text = ProductLowerControl.ToString();
      this.lbLower2T_report.Text = ProductMin.ToString();
      this.lbTarget_report.Text = ProductTarget.ToString();

      this.lbSample_report.Text = Sample.ToString();
      this.lbXtb_report.Text = Mean.ToString();
      this.lbMax_report.Text = MaxValue.ToString();
      this.lbMin_report.Text = MinValue.ToString();
      this.lbCpk_report.Text = Cpk.ToString();
      this.lbCp_report.Text = Cp.ToString();
      this.lbOw_report.Text = OW.ToString();
      this.lbTLTB_report.Text = Mean.ToString();

      this.lbOw_report.BackColor = (OW >= 0.5) ? Color.Red : Color.FromArgb(40, 167, 68);
      this.lbResult_report.BackColor = (Mean < ProductTarget) ? Color.Red : Color.FromArgb(40, 167, 68);
      this.lbResult_report.Text = (Mean < ProductTarget) ? "FAIL" : "PASS";

      _dataChart.AddChartControlDashboard(chartControl, valueNetPass, dataTimeData, ProductMax, ProductUpperControl, ProductTarget, ProductLowerControl, ProductMin, MaxValue);
      _dataChart.AddChartHistogram(chartHistogram, valueNetPass, ProductMax, ProductUpperControl, Mean, Std, ProductLowerControl, ProductMin, MinValue, MaxValue, ProductTarget);
      _dataChart.SetDataChartPie(chartPie, NumbersOk, NumbersOver, NumbersReject);

      this.dgvData.DataSource = null;
      this.dgvData.DataSource = datas;
      //SetDgvTitle();
      Report();
    }

    private void Report()
    {
      // Load file template
      string templatePath = $@"{Application.StartupPath}\Template\FormatExcel.xlsx";
      XLWorkbook workbook = new XLWorkbook(templatePath);
      IXLWorksheet worksheet = workbook.Worksheet("Report");

      string imagePath = "";
      Bitmap bitmap = new Bitmap(tableLayoutPanel1.Width, tableLayoutPanel1.Height);
      bitmap = new Bitmap(tableLayoutPanel1.Width, tableLayoutPanel1.Height);
      tableLayoutPanel1.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel1.Width, tableLayoutPanel1.Height));
      imagePath = "chart2.png";
      bitmap.Save(imagePath);
      var pictureChartPie = worksheet.Pictures.Add(imagePath);
      pictureChartPie.MoveTo(worksheet.Cell(10, 1));
      pictureChartPie.WithSize(1930, 830); 


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

      try
      {
        string path  = AppCore.Ins._lineCurrent.PathReport;
        string fileName = $@"{path}\DataReport{dt.ToString("_dd_MM_yyyy")}_Shift{ShiftId}.xlsx";
        if (dt.Hour >= 0 && dt.Hour < 6)
        {
          fileName = $@"{path}\DataReport{dt.AddDays(-1).ToString("_dd_MM_yyyy")}_Shift{ShiftId}.xlsx";
        }
        workbook.SaveAs(fileName);
      }
      catch (Exception ex)
      {
        AppCore.Ins.LogErrorToFileLog(ex.ToString());
      }
      finally { this.Close(); }
    }


    private void timerTimeOutReport_Tick(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
