using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using CheckWeigherFood.RJControl;
using ClosedXML.Excel;
using Database.DTO;
using Database.DtoHelper;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;
using static Database.Enum;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmReport : Form
  {
    public FrmReport()
    {
      InitializeComponent();
      ResgisterService();
      CustomUi();

      this.btnPreview.Click += btnPreview_Click;
      this.btnExport.Click += btnExport_Click;
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

    private void CustomUi()
    {
      this.AutoScroll = true;
      this.panel1.AutoScroll = true;
      this.panel1.AutoScrollMinSize = new Size(0, 1000);

      //Init chart
      _dataChart.ChartControlInit(chartControl);
      _dataChart.ChartHistogramInit(chartHistogram);

      lbOverWeight.ValueTilte = "OW (%)";
      lbTLTB.ValueTilte = "TL trung bình (g)";
      ucInformationLoss1.ValueTitle = "Thông tin loss";

      lbOP.SetBackColor = Color.White;
      lbQC.SetBackColor = Color.White;
      lbShiftLeader.SetBackColor = Color.White;
      lbTailTube.SetBackColor = Color.White;
      lbTube.SetBackColor = Color.White;
      lbCarton.SetBackColor = Color.White;
      lbLotTube.SetBackColor = Color.White;
      lbFGs.SetBackColor = Color.White;
      lbNameProduct.SetBackColor = Color.White;
      lbLotCarton.SetBackColor = Color.White;

      lbOP.SetForeColor = Color.Black;
      lbQC.SetForeColor = Color.Black;
      lbShiftLeader.SetForeColor = Color.Black;
      lbTailTube.SetForeColor = Color.Black;
      lbTube.SetForeColor = Color.Black;
      lbCarton.SetForeColor = Color.Black;
      lbLotTube.SetForeColor = Color.Black;
      lbFGs.SetForeColor = Color.Black;
      lbNameProduct.SetForeColor = Color.Black;
      lbLotCarton.SetForeColor = Color.Black;

      //
      ElipseControl elipseControl0 = new ElipseControl();
      elipseControl0.TargetControl = tableLayoutPanel20;
      elipseControl0.CornerRadius = 20;

      ElipseControl elipseControl1 = new ElipseControl();
      elipseControl1.TargetControl = tableLayoutPanel23;
      elipseControl1.CornerRadius = 20;

      ElipseControl elipseControl2 = new ElipseControl();
      elipseControl2.TargetControl = tableLayoutPanel1;
      elipseControl2.CornerRadius = 20;

      ElipseControl elipseControl3 = new ElipseControl();
      elipseControl3.TargetControl = tableLayoutPanel3;
      elipseControl3.CornerRadius = 20;

      ElipseControl elipseControl4 = new ElipseControl();
      elipseControl4.TargetControl = tableLayoutPanel7;
      elipseControl4.CornerRadius = 20;

      ElipseControl elipseControl5 = new ElipseControl();
      elipseControl5.TargetControl = tableLayoutPanel16;
      elipseControl5.CornerRadius = 20;

      ElipseControl elipseControl6 = new ElipseControl();
      elipseControl6.TargetControl = tableLayoutPanel24;
      elipseControl6.CornerRadius = 20;

      ElipseControl elipseControl7 = new ElipseControl();
      elipseControl7.TargetControl = dgvData;
      elipseControl7.CornerRadius = 20;
    }

    private DatalogService _datalogService { get; set; }
    private DataChart _dataChart = new DataChart();
    private int _lineIndexCurrent { get; set; } = 0;
    private void ResgisterService()
    {
      _datalogService = AppFactory.CreateDatalogService();
    }
    private void FrmReport_Load(object sender, EventArgs e)
    {
      this.dtp.Value = DateTime.Now;
      this.cbbShift.SelectedIndex = 0;
      this.flowLayoutPanelProductReport.Visible = false;

      this.cbbLine.SelectedIndexChanged += CbbLine_SelectedIndexChanged;
      this.cbbLine.SelectedIndex = 0;
    }

    private void CbbLine_SelectedIndexChanged(object sender, EventArgs e)
    {
      _lineIndexCurrent = cbbLine.SelectedIndex;
    }

    private DateTime _selectedDate;
    private int _selectedShift;
    private FrmLoading frmLoading = new FrmLoading();
    private System.Timers.Timer timerLoading = new System.Timers.Timer();
    private async void btnPreview_Click(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          btnPreview_Click(sender, e);
        }));
        return;
      }

      try
      {
        frmLoading.ShowLoading("Loading Data ...");

        _selectedDate = dtp.Value;
        _selectedShift = cbbShift.SelectedIndex + 1;

        this.btnPreview.Visible = false;

        if (_lineIndexCurrent == 0)
        {
          Machine machine = AppCore.Ins._machineCurrent03;
          var (from, to) = GetShiftRange(_selectedDate, _selectedShift);
          var dataLogs = await _datalogService.GetAllDataByTimeAsync(from, to, machine.Id);
          _resultGroups = dataLogs
                      .GroupBy(x => new
                      {
                        x.ProductId,
                        x.ChangeOverId
                      })
                      .Select(g => new DatalogGroup
                      {
                        ProductId = g.Key.ProductId,
                        ChangeOverId = g.Key.ChangeOverId,
                        Datalogs = g.ToList()
                      })
                      .ToList();
        }
        else if (_lineIndexCurrent == 1)
        {
          Machine machine = AppCore.Ins._machineCurrent04;
          var (from, to) = GetShiftRange(_selectedDate, _selectedShift);
          var dataLogs = await _datalogService.GetAllDataByTimeAsync(from, to, machine.Id);
          _resultGroups = dataLogs
                      .GroupBy(x => new
                      {
                        x.ProductId,
                        x.ChangeOverId
                      })
                      .Select(g => new DatalogGroup
                      {
                        ProductId = g.Key.ProductId,
                        ChangeOverId = g.Key.ChangeOverId,
                        Datalogs = g.ToList()
                      })
                      .ToList();
        }

        //Time chart
        SetTimeFilterChart(_selectedShift);
        LoadDataUI();
      }
      catch (Exception ex)
      {

      }
    }

    private void picFilterChart_Click(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          picFilterChart_Click(sender, e);
        }));
        return;
      }

      //Data time
      GetDt();
      var dataChartline = _sumaryDTO.DatalogPass
                      .Where(x => x.CreatedAt >= _from && x.CreatedAt <= _to)
                      .OrderBy(x => x.CreatedAt)
                      .ToList();

      _dataChart.AddChartControlDashboard(chartControl, _sumaryDTO, dataChartline, 0);
    }

    private List<DatalogGroup> _resultGroups { get; set; }
    private DateTime _from { get; set; }
    private DateTime _to { get; set; }
    private void GetDt()
    {
      try
      {
        TimeSpan timeSpanFrom = ucFilterTime1.From;
        TimeSpan timeSpanTo = ucFilterTime1.To;

        _from = dtp.Value.Date + timeSpanFrom;
        _to = dtp.Value.Date + (timeSpanTo);

        if (timeSpanTo < timeSpanFrom)
        {
          DateTime dt = DateTime.Now;
          if (dt.Hour >= 0 && dt.Hour <= 6)
          {
            _from = _from.AddDays(-1);
          }
          else
          {
            _to = _to.AddDays(1);
          }
        }
      }
      catch (Exception)
      {

      }
    }


    public static (DateTime From, DateTime To) GetShiftRange(DateTime date, int shift)
    {
      DateTime from;
      DateTime to;

      switch (shift)
      {
        case 1:
          from = date.Date.AddHours(6); // 06:00:00
          to = date.Date.AddHours(13)
                        .AddMinutes(59)
                        .AddSeconds(59); // 13:59:59
          break;

        case 2:
          from = date.Date.AddHours(14); // 14:00:00
          to = date.Date.AddHours(21)
                        .AddMinutes(59)
                        .AddSeconds(59); // 21:59:59
          break;

        case 3:
          from = date.Date.AddHours(22); // 22:00:00
          to = date.Date.AddDays(1)
                        .AddHours(5)
                        .AddMinutes(59)
                        .AddSeconds(59); // 05:59:59 ngày hôm sau
          break;

        default:
          throw new ArgumentException("Shift phải là 1, 2 hoặc 3.");
      }

      return (from, to);
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
        if (_resultGroups?.Count() > 0)
        {
          int no = 1;
          foreach (var group in _resultGroups)
          {
            RJButton btn = new RJButton();
            btn.Width = 200;
            btn.Height = 40;
            btn.BorderRadius = 5;
            btn.BackColor = Color.FromArgb(49, 68, 108);
            btn.Font = new Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular);

            Product product = AppCore.Ins._products?.Where(x => x.Id == group.ProductId).FirstOrDefault();
            btn.Text = $"{no++} - FGs: {product?.Code}";
            btn.Tag = group;

            btn.Click += ProductButton_Click;

            flowLayoutPanelProductReport.Controls.Add(btn);
          }

          // Mặc định chọn nút đầu tiên
          if (flowLayoutPanelProductReport.Controls.Count > 0)
          {
            ProductButton_Click(
                flowLayoutPanelProductReport.Controls[0],
                EventArgs.Empty);
          }

          flowLayoutPanelProductReport.Visible = true;
        }
        else
        {
          flowLayoutPanelProductReport.Visible = false;
          //Clear
          ucInformationDataSumary1.SetSumaryDTO(null);
          dgvData.DataSource = null;
          ucInformationDataSumary1.SetInforProduct(null, 0, 0, 0);
          _dataChart.AddChartControlDashboard(chartControl, null, null, 0);
          _dataChart.AddChartHistogram(chartHistogram, null);
          ucChartPie1.SetDataChartPie(null);
          SetDataOW_Mean(null);
          UpdateInforLoss(null);
          UpdateDataReject(null);
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

    private SumaryDTO _sumaryDTO { get; set; }
    private void ProductButton_Click(object sender, EventArgs e)
    {
      if (!(sender is RJButton))
        return;
      RJButton btn = (RJButton)sender;

      var group = (DatalogGroup)btn.Tag;

      // Highlight nút đang chọn
      foreach (Control control in flowLayoutPanelProductReport.Controls)
      {
        if (control is RJButton b)
        {
          b.BackColor = Color.FromArgb(49, 68, 108);
        }
      }

      btn.BackColor = Color.SeaGreen;

      // Data của nhóm được chọn
      List<Datalog> data = group.Datalogs;

      if (data?.Count() > 0)
      {
        GetDt();

        //Thông tin vận hành
        string op = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeOP;
        string qc = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeQC;
        string tc = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeShiftLeader;

        lbOP.ValueStr = op;
        lbQC.ValueStr = qc;
        lbShiftLeader.ValueStr = tc;

        //Thông tin sản phẩm
        var product = AppCore.Ins._products?.FirstOrDefault(x => x.Id == group.ProductId);

        double tareTube = Math.Round(data.Average(x => x.TareTube), 2);
        double tareTailTube = Math.Round(data.Average(x => x.TareTailTube), 2);
        double tareCarton = Math.Round(data.Average(x => x.TareCarton), 2);

        lbTube.ValueStr = tareTube.ToString();
        lbTailTube.ValueStr = tareTailTube.ToString();
        lbCarton.ValueStr = tareCarton.ToString();

        string lotTube = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().LotTube;
        string loCarton = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().LotCarton;
        lbLotTube.ValueStr = lotTube;
        lbLotCarton.ValueStr = loCarton;

        //
        TareSetting tareSetting = new TareSetting();
        tareSetting.Tube = tareTube;
        tareSetting.TailTube = tareTailTube;
        tareSetting.Carton = tareCarton;
        _sumaryDTO = AppCore.Ins.SumaryDTOData(data, product, tareSetting);


        if (product != null)
        {
          lbFGs.ValueStr = product.Code;
          lbNameProduct.ValueStr = product.Description;
          ucInformationDataSumary1.SetInforProduct(product, tareTube, tareTailTube, tareCarton);

        }

        //Data time
        var dataChartline = _sumaryDTO.DatalogPass
                        .Where(x => x.CreatedAt >= _from && x.CreatedAt <= _to)
                        .OrderBy(x => x.CreatedAt)
                        .ToList();

        //Reject
        var reject = new List<DataRejectDTO>();
        if (_sumaryDTO.DatalogReject?.Count() > 0)
        {
          foreach (var rj in _sumaryDTO.DatalogReject)
          {
            DataRejectDTO dataReject = new DataRejectDTO();
            dataReject.DateTime = (DateTime)rj.CreatedAt;
            dataReject.FGs = product?.Code;
            dataReject.Actual = rj.Gross;
            dataReject.Target = _sumaryDTO.Target;
            reject.Add(dataReject);
          }
        }

        _dataChart.AddChartControlDashboard(chartControl, _sumaryDTO, dataChartline, 0);
        _dataChart.AddChartHistogram(chartHistogram, _sumaryDTO);
        ucChartPie1.SetDataChartPie(_sumaryDTO);


        lbDataNumberReject.ValueStr = _sumaryDTO.DatalogReject.Count().ToString();
        ucInformationDataSumary1.SetSumaryDTO(_sumaryDTO);
        SetDataOW_Mean(_sumaryDTO);
        UpdateInforLoss(_sumaryDTO);
        UpdateDataReject(reject);

        var dtoDto = HelperDTO.ConvertDatalogDTO(data);
        dgvData.DataSource = dtoDto;
        dgvData.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //dgvData.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvData.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvData.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //dgvData.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvData.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        CheckShowColor();
      }
      else
      {
        //Clear
        ucInformationDataSumary1.SetSumaryDTO(null);
        dgvData.DataSource = null;
        ucInformationDataSumary1.SetInforProduct(null, 0, 0, 0);
        _dataChart.AddChartControlDashboard(chartControl, null, null, 0);
        _dataChart.AddChartHistogram(chartHistogram, null);
        ucChartPie1.SetDataChartPie(null);
        SetDataOW_Mean(null);
        UpdateInforLoss(null);
        UpdateDataReject(null);
      }
    }

    private void CheckShowColor()
    {
      foreach (DataGridViewRow row in dgvData.Rows)
      {
        if (!(row.DataBoundItem is DatalogDTO data))
          continue;

        DataGridViewCell cell = row.Cells[7];

        switch (data.EnumStatusRecord)
        {
          case EnumStatusRecord.Accept:
            cell.Style.BackColor = Color.FromArgb(0, 192, 0);
            break;

          case EnumStatusRecord.Over:
            cell.Style.BackColor = Color.FromArgb(255, 128, 0);
            break;

          case EnumStatusRecord.Reject:
            cell.Style.BackColor = Color.Red;
            break;
        }
      }
    }

    private void SetDataOW_Mean(SumaryDTO sumaryDTO)
    {
      try
      {
        if (this.InvokeRequired)
        {
          this.Invoke(new Action(() =>
          {
            SetDataOW_Mean(sumaryDTO);
          }));
          return;
        }

        if (sumaryDTO==null)
        {
          lbOverWeight.ValueData = "0.0";
          lbTLTB.ValueData = "0.0";
          lbOverWeight.SetColor = Color.DarkGreen;
          return;
        }  

        lbOverWeight.ValueData = sumaryDTO.OW.ToString();
        lbTLTB.ValueData = sumaryDTO.Mean.ToString();

        lbOverWeight.SetColor = (sumaryDTO.OW > 0.5) ? Color.Tomato : Color.DarkGreen;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void UpdateInforLoss(SumaryDTO sumaryDTO)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          UpdateInforLoss(sumaryDTO);
        }));
        return;
      }
      if (sumaryDTO == null)
      {
        ucInformationLoss1.ValueLossReject = "0.0";
        ucInformationLoss1.ValueLossOW = "0.0";
        return;
      }

      double cnt = (double)(sumaryDTO.DatalogAccept.Count());
      double lossByReject = sumaryDTO.DatalogReject.Sum(x => x.Gross);
      double lossByOW = (sumaryDTO.OW / 100.0) * sumaryDTO.targetSrc * cnt;

      ucInformationLoss1.ValueLossReject = Math.Round((lossByReject / 1000.0), 2).ToString();
      ucInformationLoss1.ValueLossOW = Math.Round((lossByOW / 1000.0), 2).ToString();
    }

    private void UpdateDataReject(List<DataRejectDTO> dataRejects)
    {
      try
      {
        if (this.InvokeRequired)
        {
          this.Invoke(new Action(() =>
          {
            UpdateDataReject(dataRejects);
          }));
          return;
        }

        if (dataRejects == null)
        {
          dgvReject.Rows.Clear();
          return;
        }

        dataRejects = dataRejects.OrderByDescending(x => x.DateTime).ToList();
        dgvReject.Rows.Clear();
        int noReject = dataRejects.Count();
        foreach (var item in dataRejects)
        {
          int indexOfFirstSpace = item.DateTime.ToString().IndexOf(' ');
          string timeOnly = item.DateTime.ToString().Substring(indexOfFirstSpace + 1);

          dgvReject.Rows.Add(noReject--, timeOnly, item.FGs, item.Target, item.Actual);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }


    private void SetTimeFilterChart(int shift)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { SetTimeFilterChart(shift); }));
        return;
      }

      if (shift == 1)
      {
        ucFilterTime1.From = new TimeSpan(6, 0, 0);
        ucFilterTime1.To = new TimeSpan(14, 0, 0);
      }
      else if (shift == 2)
      {
        ucFilterTime1.From = new TimeSpan(14, 0, 0);
        ucFilterTime1.To = new TimeSpan(22, 0, 0);
      }
      else if (shift == 3)
      {
        ucFilterTime1.From = new TimeSpan(22, 0, 0);
        ucFilterTime1.To = new TimeSpan(6, 0, 0);
      }
    }





    private string fileName = "";
    private void btnExport_Click(object sender, EventArgs e)
    {
      try
      {
        this.btnExport.Visible = false;
        string line = $"Line {cbbLine.SelectedItem.ToString()}";
        // Load file template
        string templatePath = $@"{Application.StartupPath}\Template\FormatExcel.xlsx";
        XLWorkbook workbook = new XLWorkbook(templatePath);
        IXLWorksheet worksheet = workbook.Worksheet("Report");

        worksheet.Cell("C3").Value = _selectedDate.ToString("yyyy-MM-dd");
        worksheet.Cell("C4").Value = _selectedShift.ToString();
        worksheet.Cell("C5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        worksheet.Cell("F3").Value = $"{cbbLine.SelectedItem.ToString()}";
        worksheet.Cell("F4").Value = _sumaryDTO.Product.Code;
        worksheet.Cell("F5").Value = _sumaryDTO.Product.ProName;

        //Sumary
        worksheet.Cell("C7").Value = _sumaryDTO.EnumResult == EnumResult.Pass ? "ĐẠT" : "KHÔNG ĐẠT";
        worksheet.Cell("C8").Value = _sumaryDTO.Sample;
        worksheet.Cell("C9").Value = _sumaryDTO.Cpk;
        worksheet.Cell("C10").Value = _sumaryDTO.Cp;
        worksheet.Cell("C11").Value = _sumaryDTO.Min;
        worksheet.Cell("C12").Value = _sumaryDTO.Max;


        double accept = (double)_sumaryDTO.DatalogAccept.Count();
        double over = (double)_sumaryDTO.DatalogOver.Count();
        double reject = (double)_sumaryDTO.DatalogReject.Count();
        double total = accept + over + reject;

        double accept_P = Math.Round((accept * 100) / total, 2);
        double over_P = Math.Round((over * 100) / total, 2);
        double reject_P = Math.Round((reject * 100) / total, 2);

        worksheet.Cell("C13").Value = $"{over}  ({over_P} %)";
        worksheet.Cell("C14").Value = $"{accept}  ({accept_P} %)";
        worksheet.Cell("C15").Value = $"{reject}  ({reject_P} %)";

        //INfor Product
        worksheet.Cell("F7").Value = _sumaryDTO.Target;
        worksheet.Cell("F8").Value = _sumaryDTO.USL;
        worksheet.Cell("F9").Value = _sumaryDTO.UCL;
        worksheet.Cell("F10").Value = _sumaryDTO.LCL;
        worksheet.Cell("F11").Value = _sumaryDTO.LSL;

        //// Lấy dữ liệu từ DataGridView
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
        worksheet.Cell("A33").InsertTable(dataTable);

        string imagePath = "";
        // Chart Control
        Bitmap bitmap = new Bitmap(tableLayoutPanel23.Width, tableLayoutPanel23.Height);
        tableLayoutPanel23.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel23.Width, tableLayoutPanel23.Height));

        imagePath = "chart1.png";
        bitmap.Save(imagePath);
        var pictureChartControl = worksheet.Pictures.Add(imagePath);
        pictureChartControl.MoveTo(worksheet.Cell(17, 1));
        pictureChartControl.WithSize(1405, 300);


        //tableLayoutPanel24
        //bitmap = new Bitmap(tableLayoutPanel24.Width, tableLayoutPanel24.Height);
        //tableLayoutPanel24.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel24.Width, tableLayoutPanel24.Height));
        //imagePath = "chart2.png";
        //bitmap.Save(imagePath);
        //var pictureChartPie = worksheet.Pictures.Add(imagePath);
        //pictureChartPie.MoveTo(worksheet.Cell(10, 1));
        //pictureChartPie.WithSize(1930, 300);



        using (var saveFD = new SaveFileDialog())
        {
          saveFD.Filter = "Excel|*.xlsx|All files|*.*";
          saveFD.Title = "Save report to excel file";
          //saveFD.FileName = $"DataReport{fromDate.ToString("_dd_MM_yyyy")}_{cbShift.SelectedItem.ToString().Trim()}";
          saveFD.FileName = $"Report_{line}_{_sumaryDTO.Product.Code}_{_selectedDate.ToString("_dd_MM_yyyy")}_Shift{_selectedShift}";
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
        this.btnExport.Visible = true;
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

  }
}
