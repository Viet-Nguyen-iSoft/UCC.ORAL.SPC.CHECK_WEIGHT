using CheckWeigherFood.Controls;
using CheckWeigherFood.FrmChild;
using CheckWeigherFood.InitChart;
using ClosedXML.Excel;
using Database.DTO;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Bibliography;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;
using static Database.Enum;

namespace CheckWeigherFood.FormUI
{
  public partial class FrmReportLossOW : Form
  {
    public FrmReportLossOW()
    {
      InitializeComponent();
      ResgisterService();
      this.Load += FrmReportLossOW_Load;
      this.btnPreview.Click += BtnPreview_Click;
      this.btnExport.Click += BtnExport_Click;
    }

    

    #region Singleton parttern
    private static FrmReportLossOW _Instance = null;
    public static FrmReportLossOW Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmReportLossOW();
        }
        return _Instance;
      }
    }
    #endregion

    private WeekInfo _weekInfo { get;set; }
    private int _yearCurrent { get;set; }

    private ProductService _productService { get; set; }
    private DatalogService _datalogService { get; set; }
    private void ResgisterService()
    {
      _datalogService = AppFactory.CreateDatalogService();
      _productService = AppFactory.CreateProductService();
    }
    private void FrmReportLossOW_Load(object sender, EventArgs e)
    {
      var years = GetYears();
      cbbYear.DataSource = years;

      _yearCurrent = DateTime.Now.Year;
      cbbYear.SelectedItem = _yearCurrent;


      var listWeeks = GetWeeks(_yearCurrent);
      cbbWeek.DataSource = listWeeks;
      cbbWeek.DisplayMember = "Week";
      cbbWeek.ValueMember = "Week";
      _weekInfo = listWeeks.FirstOrDefault(x =>
          DateTime.Today >= x.From.Date &&
          DateTime.Today <= x.To.Date);

      if (_weekInfo != null)
      {
        cbbWeek.SelectedItem = _weekInfo;
        ShowInforWeek(_weekInfo);
      }

      cbbYear.SelectedIndexChanged += CbbYear_SelectedIndexChanged;
      cbbWeek.SelectedIndexChanged += CbbWeek_SelectedIndexChanged;
    }

    private async void BtnPreview_Click(object sender, EventArgs e)
    {
      try
      {
        LockUI(true);
        if (_weekInfo != null)
        {
          var dataLogs = await LoadData(_weekInfo);
          if (dataLogs?.Count() > 0)
          {
            var result = dataLogs
                        .GroupBy(x =>
                        {
                          var shift = GetShift((DateTime)x.CreatedAt);

                          return new
                          {
                            x.MachineId,
                            x.ProductId,
                            x.ChangeOverId,
                            shift.Date,
                            shift.Shift
                          };
                        })
                        .Select(g => new DatalogGroupCalLossOW
                        {
                          MachineId = g.Key.MachineId,
                          ProductId = g.Key.ProductId,
                          ChangeOverId = g.Key.ChangeOverId,
                          Date = g.Key.Date,
                          Shift = g.Key.Shift,
                          Items = g.ToList()
                        })
                        .OrderBy(x => x.MachineId)
                        .ToList();

            if (result?.Count() > 0)
            {
              List<DatalogOWDTO> datalogOWDTOs = new List<DatalogOWDTO>();
              foreach (var datalogOWDTO in result)
              {
                var machine = AppCore.Ins._machines?.FirstOrDefault(x => x.Id == datalogOWDTO.MachineId);
                var product = await _productService?.GetDataByIdAsync(datalogOWDTO.ProductId);
                if (product != null)
                {
                  var dataAfterFilter = datalogOWDTO.Items?.Where(x => x.Gross < 1.5 * product.Target).ToList();
                  if (dataAfterFilter?.Count() > 0)
                  {
                    double tareCarton = dataAfterFilter?.Average(x => x.TareCarton)??0.0;
                    double tareTupe = dataAfterFilter?.Average(x => x.TareTube)?? 0.0;
                    double tareTailTupe = dataAfterFilter?.Average(x => x.TareTailTube) ?? 0.0  ;

                    double target = (product?.Target ?? 0.0) + tareCarton + tareTupe - tareTailTupe;
                    var dataPass = dataAfterFilter?.Where(s => s.EnumStatusRecord == EnumStatusRecord.Accept || s.EnumStatusRecord == EnumStatusRecord.Over)?.ToList();
                    var dataReject = dataAfterFilter?.Where(s => s.EnumStatusRecord == EnumStatusRecord.Reject)?.ToList();

                    double mean = (dataPass?.Count()>0)? Math.Round(dataPass?.Average(x => x.Gross)??0.0, 3) : 0.0;
                    double ow = Math.Round(((mean - target) / target) * 100, 2);

                    double cnt = (double)(dataPass?.Count()??0);
                    double lossByReject = dataReject?.Sum(x => x.Gross)?? 0.0 ;
                    double lossByOW = ow > 0 ? (ow / 100.0) * (product?.Target ?? 0.0) * cnt : 0.0;

                    DatalogOWDTO datalogDTO = new DatalogOWDTO();
                    datalogDTO.Line = machine.Name;
                    datalogDTO.FGs = product?.Code;
                    datalogDTO.NameProduction = product.Description;
                    datalogDTO.Date = datalogOWDTO.Date.ToString("yyyy-MM-dd");
                    datalogDTO.Shift = datalogOWDTO.Shift;
                    datalogDTO.NumberDatalog = dataPass?.Count()??0;
                    datalogDTO.NumberReject = dataReject?.Count()??0;
                    datalogDTO.Operator = (dataPass?.Count() > 0) ? dataPass?.OrderByDescending(x => x.CreatedAt).FirstOrDefault().NameEmployeeOP : string.Empty;
                    datalogDTO.OW = ow;
                    datalogDTO.LossByOW = Math.Round( lossByOW/1000.0,3);
                    datalogDTO.LossByReject = Math.Round(lossByReject/1000.0,3);

                    datalogOWDTOs.Add(datalogDTO);
                  }
                }
              }

              ShowDgv(datalogOWDTOs);
            }
            else
            {
              ShowDgv(new List<DatalogOWDTO>());
              new FrmInformation().ShowMessage("Không có data !", eImage.Information);
            }
          }
          else
          {
            ShowDgv(new List<DatalogOWDTO>());
            new FrmInformation().ShowMessage("Không có data !", eImage.Information);
          }
        }
        else
        {
          ShowDgv(new List<DatalogOWDTO>());
          new FrmInformation().ShowMessage("Vui lòng chọn tuần cần xuất báo cáo !", eImage.Information);
        }
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lỗi !", eImage.Warning);
      } 
      finally
      {
        LockUI(false);
      }
    }


    private string fileName { get; set; }
    private void BtnExport_Click(object sender, EventArgs e)
    {
      try
      {
        string templatePath = $@"{Application.StartupPath}\Template\FormatExcelOW.xlsx";
        XLWorkbook workbook = new XLWorkbook(templatePath);
        IXLWorksheet worksheet = workbook.Worksheet("Report");

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
        worksheet.Cell("A3").InsertTable(dataTable);

        using (var saveFD = new SaveFileDialog())
        {
          saveFD.Filter = "Excel|*.xlsx|All files|*.*";
          saveFD.Title = "Save report to excel file";
          //saveFD.FileName = $"DataReport{fromDate.ToString("_dd_MM_yyyy")}_{cbShift.SelectedItem.ToString().Trim()}";
          saveFD.FileName = $"ReportOW_Loss_{_yearCurrent}_Week{_weekInfo.Week}";
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
        new FrmInformation().ShowMessage($"Lỗi: {ex.ToString()}", eImage.Warning);
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

    private async Task<List<Datalog>> LoadData(WeekInfo weekInfo)
    {
      return await _datalogService.GetAllDataByTimeAsync(weekInfo.From, weekInfo.To);
    }

    private void CbbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
      _yearCurrent = int.Parse(cbbYear.SelectedItem.ToString());
      var listWeeks = GetWeeks(_yearCurrent);
      ShowCbbWeek(listWeeks);
    }

    private void CbbWeek_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cbbWeek.SelectedItem is WeekInfo week)
      {
        _weekInfo = week;
        ShowInforWeek(_weekInfo);
      }
    }

    private void ShowCbbWeek(List<WeekInfo> weekInfos)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowCbbWeek(weekInfos); }));
        return;
      }

      cbbWeek.DataSource = weekInfos;
      cbbWeek.DisplayMember = "Week";
      cbbWeek.ValueMember = "Week";
    }

    private void LockUI(bool lockUI)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { LockUI(lockUI); }));
        return;
      }

      this.Enabled = !lockUI;
    }

    private void ShowDgv(List<DatalogOWDTO> datalogOWDTOs)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowDgv(datalogOWDTOs); }));
        return;
      }

      dgvData.DataSource = null;
      dgvData.DataSource = datalogOWDTOs;

      dgvData.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvData.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

      dgvData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ShowInforWeek(WeekInfo weekInfo)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforWeek(weekInfo); }));
        return;
      }

      if (weekInfo!=null)
        lbRangeDate.Text = "Thời gian:" + weekInfo?.From.ToString("dd/MM/yyyy") + " - " + weekInfo?.To.ToString("dd/MM/yyyy");
    }

    public static List<int> GetYears()
    {
      int startYear = 2025;
      int currentYear = DateTime.Now.Year;

      return Enumerable.Range(startYear, currentYear - startYear + 1).ToList();
    }

    public static List<WeekInfo> GetWeeks(int year)
    {
      var result = new List<WeekInfo>();

      DateTime firstDay = new DateTime(year, 1, 1);
      DateTime lastDay = new DateTime(year, 12, 31);

      // Tìm thứ Hai đầu tiên của tuần chứa 1/1
      DateTime weekStart = firstDay.AddDays(-(int)(firstDay.DayOfWeek == DayOfWeek.Sunday
          ? 6
          : firstDay.DayOfWeek - DayOfWeek.Monday));

      int week = 1;

      while (weekStart <= lastDay)
      {
        DateTime from = weekStart < firstDay ? firstDay : weekStart;
        DateTime to = weekStart.AddDays(6);

        if (to > lastDay)
          to = lastDay;

        result.Add(new WeekInfo
        {
          Week = week++,
          From = from,
          To = to
        });

        weekStart = weekStart.AddDays(7);
      }

      return result;
    }

    public static (DateTime Date, int Shift) GetShift(DateTime utcTime)
    {
      // UTC -> UTC+7
      DateTime local = utcTime.AddHours(7);

      int shift;
      DateTime date = local.Date;

      TimeSpan time = local.TimeOfDay;

      if (time >= new TimeSpan(6, 0, 0) &&
          time < new TimeSpan(14, 0, 0))
      {
        shift = 1;
      }
      else if (time >= new TimeSpan(14, 0, 0) &&
               time < new TimeSpan(22, 0, 0))
      {
        shift = 2;
      }
      else
      {
        shift = 3;

        // 00:00 -> 05:59 thuộc ca 3 của ngày hôm trước
        if (time < new TimeSpan(6, 0, 0))
        {
          date = date.AddDays(-1);
        }
      }

      return (date, shift);
    }
  }


  public class WeekInfo
  {
    public int Week { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
  }
}
