using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using ClosedXML.Excel;
using Database.DTO;
using Database.DtoHelper;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmAutoReport : Form
  {
    public FrmAutoReport()
    {
      InitializeComponent();
      ResgisterService();
      this.Shown += FrmAutoReport_Shown;
    }

    private DatalogService _datalogService { get; set; }
    private DataChart _dataChart = new DataChart();
    private void ResgisterService()
    {
      _datalogService = AppFactory.CreateDatalogService();
    }

    private List<Datalog> datalogsLine3 = new List<Datalog>();
    private List<Datalog> datalogsLine4 = new List<Datalog>();
    private List<DatalogGroup> _resultGroups03 { get; set; }
    private List<DatalogGroup> _resultGroups04 { get; set; }
    private DateTime dateTime;
    private int shift;
    private async void FrmAutoReport_Shown(object sender, EventArgs e)
    {
      var rs = GetPreviousShift(DateTime.Now);
      dateTime = DateTime.Now.AddDays(-1);
      shift = 1;
      var (from, to) = GetShiftRange(dateTime, shift);
      datalogsLine3 = await _datalogService.GetAllDataByTimeAsync(from, to, AppCore.Ins._machineCurrent03.Id);
      datalogsLine4 = await _datalogService.GetAllDataByTimeAsync(from, to, AppCore.Ins._machineCurrent04.Id);

      _resultGroups03 = datalogsLine3
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

      _resultGroups04 = datalogsLine4
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

      ExportAuto();
    }

    private void ExportAuto()
    {
      if (_resultGroups03?.Count()>0)
      {
        foreach (var item in _resultGroups03)
        {
          if (item.Datalogs.Count()>10)
          {
            // Data của nhóm được chọn
            List<Datalog> data = item.Datalogs;

            if (data?.Count() > 0)
            {
              //Thông tin vận hành
              string op = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeOP;
              string qc = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeQC;
              string tc = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeShiftLeader;

              lbOP.ValueStr = op;
              lbQC.ValueStr = qc;
              lbShiftLeader.ValueStr = tc;

              //Thông tin sản phẩm
              var product = AppCore.Ins._products?.FirstOrDefault(x => x.Id == item.ProductId);

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
              var _sumaryDTO = AppCore.Ins.SumaryDTOData(data, product, tareSetting);


              if (product != null)
              {
                lbFGs.ValueStr = product.Code;
                lbNameProduct.ValueStr = product.Description;
                ucInformationDataSumary1.SetInforProduct(product, tareTube, tareTailTube, tareCarton);

              }

              //Data time
              var dataChartline = _sumaryDTO.DatalogPass
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


              string path = AppCore.Ins._machineCurrent03.PathReport;
              Export(path, 3, dateTime, shift, _sumaryDTO);
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
        }
      }


      if (_resultGroups04?.Count() > 0)
      {
        foreach (var item in _resultGroups04)
        {
          if (item.Datalogs.Count() > 10)
          {
            // Data của nhóm được chọn
            List<Datalog> data = item.Datalogs;

            if (data?.Count() > 0)
            {
              //Thông tin vận hành
              string op = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeOP;
              string qc = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeQC;
              string tc = data.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeShiftLeader;

              lbOP.ValueStr = op;
              lbQC.ValueStr = qc;
              lbShiftLeader.ValueStr = tc;

              //Thông tin sản phẩm
              var product = AppCore.Ins._products?.FirstOrDefault(x => x.Id == item.ProductId);

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
              var _sumaryDTO = AppCore.Ins.SumaryDTOData(data, product, tareSetting);


              if (product != null)
              {
                lbFGs.ValueStr = product.Code;
                lbNameProduct.ValueStr = product.Description;
                ucInformationDataSumary1.SetInforProduct(product, tareTube, tareTailTube, tareCarton);

              }

              //Data time
              var dataChartline = _sumaryDTO.DatalogPass
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


              string path = AppCore.Ins._machineCurrent04.PathReport;
              Export(path, 4, dateTime, shift, _sumaryDTO);
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
        }
      }

      this.Close();
    }

    public static void EnsureDirectoryExists(string folderPath)
    {
      if (!Directory.Exists(folderPath))
      {
        Directory.CreateDirectory(folderPath);
      }
    }

    private void Export(string path,int line, DateTime dt, int shift, SumaryDTO sumaryDTO)
    {
      try
      {
        string folder = path + $"\\{dt.Year}";
        EnsureDirectoryExists(folder);

        folder = path + $"\\{dt.Year}\\{dt.Month.ToString("D2")}";
        EnsureDirectoryExists(folder);

        string fileName = folder +   $"\\Report_{line}_{sumaryDTO.Product.Code}_{dt.ToString("_dd_MM_yyyy")}_Shift{shift}.xlsx";
        this.btnExport.Visible = false;
        // Load file template
        string templatePath = $@"{Application.StartupPath}\Template\FormatExcel.xlsx";
        XLWorkbook workbook = new XLWorkbook(templatePath);
        IXLWorksheet worksheet = workbook.Worksheet("Report");

        worksheet.Cell("C3").Value = dt.ToString("yyyy-MM-dd");
        worksheet.Cell("C4").Value = shift.ToString();
        worksheet.Cell("C5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        worksheet.Cell("F3").Value = $"{line}";
        worksheet.Cell("F4").Value = sumaryDTO.Product.Code;
        worksheet.Cell("F5").Value = sumaryDTO.Product.Description;

        //Sumary
        worksheet.Cell("C7").Value = sumaryDTO.EnumResult == EnumResult.Pass ? "ĐẠT" : "KHÔNG ĐẠT";
        worksheet.Cell("C8").Value = sumaryDTO.Sample;
        worksheet.Cell("C9").Value = sumaryDTO.Cpk;
        worksheet.Cell("C10").Value = sumaryDTO.Cp;
        worksheet.Cell("C11").Value = sumaryDTO.Min;
        worksheet.Cell("C12").Value = sumaryDTO.Max;


        //double accept = (double)sumaryDTO.DatalogAccept.Count();
        //double over = (double)sumaryDTO.DatalogOver.Count();
        //double reject = (double)sumaryDTO.DatalogReject.Count();
        //double total = accept + over + reject;

        //double accept_P = Math.Round((accept * 100) / total, 2);
        //double over_P = Math.Round((over * 100) / total, 2);
        //double reject_P = Math.Round((reject * 100) / total, 2);

        //worksheet.Cell("C13").Value = $"{over}  ({over_P} %)";
        //worksheet.Cell("C14").Value = $"{accept}  ({accept_P} %)";
        //worksheet.Cell("C15").Value = $"{reject}  ({reject_P} %)";

        //INfor Product
        worksheet.Cell("F7").Value = sumaryDTO.Target;
        worksheet.Cell("F8").Value = sumaryDTO.USL;
        worksheet.Cell("F9").Value = sumaryDTO.UCL;
        worksheet.Cell("F10").Value = sumaryDTO.LCL;
        worksheet.Cell("F11").Value = sumaryDTO.LSL;

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
        worksheet.Cell("A30").InsertTable(dataTable);

        string imagePath = "";
        // Chart Control
        Bitmap bitmap = new Bitmap(tableLayoutPanel23.Width, tableLayoutPanel23.Height);
        tableLayoutPanel23.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel23.Width, tableLayoutPanel23.Height));

        imagePath = "chart1.png";
        bitmap.Save(imagePath);
        var pictureChartControl = worksheet.Pictures.Add(imagePath);
        pictureChartControl.MoveTo(worksheet.Cell(14, 1));
        pictureChartControl.WithSize(1405, 300);


        //tableLayoutPanel24
        //bitmap = new Bitmap(tableLayoutPanel24.Width, tableLayoutPanel24.Height);
        //tableLayoutPanel24.DrawToBitmap(bitmap, new Rectangle(0, 0, tableLayoutPanel24.Width, tableLayoutPanel24.Height));
        //imagePath = "chart2.png";
        //bitmap.Save(imagePath);
        //var pictureChartPie = worksheet.Pictures.Add(imagePath);
        //pictureChartPie.MoveTo(worksheet.Cell(10, 1));
        //pictureChartPie.WithSize(1930, 300);

        workbook.SaveAs(fileName);
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

        if (sumaryDTO == null)
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

    public static (DateTime Date, int Shift) GetPreviousShift(DateTime now)
    {
      DateTime shiftDate;
      int currentShift;

      TimeSpan time = now.TimeOfDay;

      if (time >= TimeSpan.FromHours(6) && time < TimeSpan.FromHours(14))
      {
        // Ca 1
        shiftDate = now.Date;
        currentShift = 1;
      }
      else if (time >= TimeSpan.FromHours(14) && time < TimeSpan.FromHours(22))
      {
        // Ca 2
        shiftDate = now.Date;
        currentShift = 2;
      }
      else
      {
        // Ca 3
        currentShift = 3;

        // 00:00~05:59 thuộc ca 3 của ngày hôm trước
        shiftDate = time < TimeSpan.FromHours(6)
            ? now.Date.AddDays(-1)
            : now.Date;
      }

      // Tính ca trước
      switch (currentShift)
      {
        case 1:
          return (shiftDate.AddDays(-1), 3);

        case 2:
          return (shiftDate, 1);

        case 3:
          return (shiftDate, 2);

        default:
          throw new InvalidOperationException();
      }
    }

    private void FrmAutoReport_Load(object sender, EventArgs e)
    {

    }


    private void timerTimeOutReport_Tick(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
