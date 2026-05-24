using Aspose.Cells;
using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using ClosedXML.Excel;
using Database.DTO;
using Database.DtoHelper;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmMasterData : Form
  {
    public delegate void SendChangeMasterData();
    public event SendChangeMasterData OnSendChangeMasterData;
    public FrmMasterData()
    {
      InitializeComponent();
      RegisterService();
      this.Shown += FrmMasterData_Shown;
    }

    #region Singleton parttern
    private static FrmMasterData _Instance = null;
    public static FrmMasterData Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmMasterData();
        }
        return _Instance;
      }
    }
    #endregion


    private ProductService _productService { get; set; }
    private List<Product> _products { get; set; }
    private void RegisterService()
    {
      _productService = AppFactory.CreateProductService();
    }

    private void FrmMasterData_Load(object sender, EventArgs e)
    {
      //List<MasterData> list = new List<MasterData>();
      //list = AppCore.Ins._listMasterData.Where(x => x.isDelete == false).ToList();
      //if (list.Count > 0)
      //{
      //  SetDgvFull(list);
      //}
    }

    private async void FrmMasterData_Shown(object sender, EventArgs e)
    {
      _products = await LoadData();
      var dto = HelperDTO.ConvertProductDTO(_products, "");
      ShowDgv(dto);
    }

    private async Task<List<Product>> LoadData()
    {
      return await _productService.GetAllAsync();
    }


    private void btnImport_Click(object sender, EventArgs e)
    {
      DialogResult result = this.openFileDialog1.ShowDialog();
      if (result == DialogResult.OK)
      {
        if (backgroundWorker1.IsBusy == false)
        {
          this.backgroundWorker1.RunWorkerAsync();
        }
      }
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      fileName = openFileDialog1.FileName;
    }


    private List<ProductDTO> productDTOs_Import { get; set; }
    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        productDTOs_Import = PareXlsxByAspose(fileName, 1);
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Không load được File Excel MasterData !", eNumUI.eImage.Warning);
        //TODO
      }
    }

    private void ShowDgv(List<ProductDTO> productDTOs)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowDgv(productDTOs); }));
        return;
      }


      dgvDataProducts.DataSource = productDTOs;


      dgvDataProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvDataProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvDataProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvDataProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvDataProducts.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvDataProducts.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgvDataProducts.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgvMachine.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      VisibleSave(true);
      ShowDgv(productDTOs_Import);
    }



    private void VisibleSave(bool visible)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { VisibleSave(visible); }));
        return;
      }

      btnSave.Visible = visible;
    }

    private void LockUI(bool lockUI)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { LockUI(lockUI); }));
        return;
      }

      btnSave.Enabled = lockUI;
    }












    private string fileName = string.Empty;
    //private List<MasterData> masterData = new List<MasterData>();
    //private List<DataExcelImport> dataExcelImports = new List<DataExcelImport>();






    private void UpdateDGV()
    {
      //if (this.InvokeRequired)
      //{
      //  this.Invoke(new Action(() => { UpdateDGV(); }));
      //  return;
      //}
      //if (this.dataExcelImports.Count > 0)
      //{
      //  this.btnSave.Visible = true;
      //  SetDgvExcel(dataExcelImports);
      //  new FrmInformation().ShowMessage("File excel đã được load thành công.", eImage.Information);
      //}
      //else
      //{
      //  new FrmInformation().ShowMessage("File excel load không tìm thấy dữ liệu !", eImage.Warning);
      //}
    }

    //private void SetDgvExcel(List<DataExcelImport> datas)
    //{
    //  if (this.InvokeRequired)
    //  {
    //    this.Invoke(new Action(() =>
    //    {
    //      SetDgvExcel(datas);
    //    }));
    //    return;
    //  }

    //  this.dgvDataProducts.DataSource = null;
    //  this.dgvDataProducts.DataSource = datas;

    //  int i = 0;
    //  this.dgvDataProducts.Columns[i++].HeaderText = "STT";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "SKU";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "FGs Code";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "FGs Name";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "Target (g)";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "T (Lượng thiếu cho phép)";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "Lower control (g)";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "Uper control (g)";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "Min (g)";
    //  this.dgvDataProducts.Columns[i++].HeaderText = "Max (g)";
    //}

    #region Read Excel
    public List<ProductDTO> PareXlsxByAspose(string file_path, int updateby)
    {
      List<ProductDTO> productDTOs = new List<ProductDTO>();

      FileInfo dest_file_info = new FileInfo(file_path);
      Workbook wb = new Workbook(dest_file_info.FullName);
      WorksheetCollection collection = wb.Worksheets;
      bool is_exit_loop = false;


      for (int worksheetIndex = 0; worksheetIndex < collection.Count && is_exit_loop == false; worksheetIndex++)
      {
        Worksheet worksheet = collection[worksheetIndex];
        if (worksheet.Name.Trim().ToLower() == "data")
        {
          int max_rows = worksheet.Cells.MaxDataRow;
          int max_cols = worksheet.Cells.MaxDataColumn;

          for (int row = 2; row <= max_rows; row++)
          {
            try
            {
              ProductDTO row_data = new ProductDTO();
              row_data.No = row - 1;
              row_data.ProName = GetText(worksheet, row, 0);
              row_data.Code = GetText(worksheet, row, 1);
              row_data.Description = GetText(worksheet, row, 2);
              row_data.Type = GetText(worksheet, row, 3);
              row_data.WeightOnPack = GetText(worksheet, row, 4);
              row_data.Unit = GetText(worksheet, row, 5);
              row_data.UCL = GetDouble(worksheet, row, 6);
              row_data.LCL = GetDouble(worksheet, row, 7);
              row_data.USL = GetDouble(worksheet, row, 8);
              row_data.LSL = GetDouble(worksheet, row, 9);
              row_data.Target = GetDouble(worksheet, row, 10);
              row_data.Density = GetDouble(worksheet, row, 11);
              row_data.T = GetDouble(worksheet, row, 12);
              row_data.Tare = GetDouble(worksheet, row, 13);
              row_data.Note = GetText(worksheet, row, 15);
              row_data.Datetime = ((DateTime)DateTime.UtcNow).AddHours(7).ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A";
              if (!string.IsNullOrEmpty(row_data.Code))
                productDTOs.Add(row_data);
            }
            catch (Exception ex)
            {
              throw ex;
            }
          }
        }
      }
      return productDTOs;
    }

    private static string GetText(Worksheet worksheet, int row, int column)
    {
      string ret = "";
      try
      {
        object textObj = worksheet.Cells[row, column].Value;
        if (textObj != null)
          ret = textObj.ToString().Trim();
      }
      catch
      {
      }
      return ret;
    }

    private static double GetDouble(Worksheet worksheet, int row, int column)
    {
      double ret = 0;
      try
      {
        object textObj = worksheet.Cells[row, column].Value;
        if (textObj != null)
          double.TryParse(textObj.ToString(), out ret);
      }
      catch
      {
      }
      return ret;
    }
    #endregion






    private async void btnSave_Click(object sender, EventArgs e)
    {
      try
      {
        //if (dataExcelImports.Count > 0)
        //{
        //  masterData = new List<MasterData>();

        //  foreach (DataExcelImport item in dataExcelImports)
        //  {
        //    MasterData data = new MasterData();
        //    data.UpdatedAt = DateTime.Now;
        //    data.CreatedAt = DateTime.Now;
        //    data.SKU = item.SKU;
        //    data.FGs = item.FGs;
        //    data.Description = item.Description;
        //    data.Target = item.Target;
        //    data.ValueT = item.ValueT;
        //    data.LowerControl = item.LowerControl;
        //    data.UpperControl = item.UpperControl;
        //    data.Min = item.Min;
        //    data.Max = item.Max;
        //    data.isEnable = false;
        //    data.isDelete = false;
        //    data.NormalSpeed = item.NormalSpeed;
        //    masterData.Add(data);
        //  }
        //  await AppCore.Ins.UpdateRangeMasterDataOld();
        //  await AppCore.Ins.AddMasterData(masterData);
        //  SetDgvFull(masterData);
        //  this.btnSave.Visible = false;
        //  new FrmInformation().ShowMessage("Dữ liệu đã được cập nhật.", eImage.Information);
        //  if (OnSendChangeMasterData != null)
        //    OnSendChangeMasterData();
        //}
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lưu dữ liệu thất bại !", eImage.Information);
      }
    }

    //private void SetDgvFull(List<MasterData> datas)
    //{
    //  if (this.InvokeRequired)
    //  {
    //    this.Invoke(new Action(() =>
    //    {
    //      SetDgvFull(datas);
    //    }));
    //    return;
    //  }

    //  dgvDataProducts.DataSource = null;
    //  dgvDataProducts.DataSource = datas;


    //  this.dgvDataProducts.Columns[0].HeaderText = "SKU";
    //  this.dgvDataProducts.Columns[1].HeaderText = "FGs Code";
    //  this.dgvDataProducts.Columns[2].HeaderText = "FGs Name";
    //  this.dgvDataProducts.Columns[3].HeaderText = "Target (g)";
    //  this.dgvDataProducts.Columns[4].HeaderText = "T (Lượng thiếu cho phép)";
    //  this.dgvDataProducts.Columns[5].HeaderText = "Lower control (g)";
    //  this.dgvDataProducts.Columns[6].HeaderText = "Uper control (g)";
    //  this.dgvDataProducts.Columns[7].HeaderText = "Min (g)";
    //  this.dgvDataProducts.Columns[8].HeaderText = "Max (g)";
    //  this.dgvDataProducts.Columns[11].HeaderText = "Normal Speed";

    //  this.dgvDataProducts.Columns[9].Visible = false;
    //  this.dgvDataProducts.Columns[10].Visible = false;
    //  this.dgvDataProducts.Columns[12].Visible = false;
    //  this.dgvDataProducts.Columns[14].Visible = false;

    //  //this.dgvDataProducts.Columns[0].Width = 30;
    //  //this.dgvDataProducts.Columns[1].Width = 50;
    //  //this.dgvDataProducts.Columns[2].Width = 220;
    //  //this.dgvDataProducts.Columns[5].Width = 40;
    //  //this.dgvDataProducts.Columns[5].Width = 90;
    //  //this.dgvDataProducts.Columns[6].Width = 80;
    //  //this.dgvDataProducts.Columns[7].Width = 40;
    //  //this.dgvDataProducts.Columns[8].Width = 40;
    //  //dgvDataProducts.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    //}



    private static string fileSaveExportExcel = "";
    private void btnExport_Click(object sender, EventArgs e)
    {
      try
      {
        string startupPath = $@"{Application.StartupPath}\\Template\\MasterDataTemplate.xlsx";
        if (!File.Exists(startupPath))
        {
          new FrmInformation().ShowMessage("Không có file Template Excel MasterData !", eImage.Warning);
          return;
        }

        using (var workbook = new XLWorkbook(startupPath))
        {
          // Hiển thị hộp thoại lưu file
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx|All files (*.*)|*.*";
          saveFileDialog.Title = "Save Excel File";
          if (saveFileDialog.ShowDialog() == DialogResult.OK)
          {
            var worksheet = workbook.Worksheets.First();

            for (int i = 0; i < dgvDataProducts.Rows.Count; i++)
            {
              worksheet.Cell($"A{i + 2}").Value = dgvDataProducts.Rows[i].Cells[0].Value.ToString();
              worksheet.Cell($"B{i + 2}").Value = dgvDataProducts.Rows[i].Cells[1].Value.ToString();
              worksheet.Cell($"C{i + 2}").Value = dgvDataProducts.Rows[i].Cells[2].Value.ToString();
              worksheet.Cell($"D{i + 2}").Value = dgvDataProducts.Rows[i].Cells[3].Value.ToString();
              worksheet.Cell($"E{i + 2}").Value = dgvDataProducts.Rows[i].Cells[4].Value.ToString();
              worksheet.Cell($"F{i + 2}").Value = dgvDataProducts.Rows[i].Cells[5].Value.ToString();
              worksheet.Cell($"G{i + 2}").Value = dgvDataProducts.Rows[i].Cells[6].Value.ToString();
              worksheet.Cell($"H{i + 2}").Value = dgvDataProducts.Rows[i].Cells[7].Value.ToString();
              worksheet.Cell($"I{i + 2}").Value = dgvDataProducts.Rows[i].Cells[8].Value.ToString();
              worksheet.Cell($"J{i + 2}").Value = dgvDataProducts.Rows[i].Cells[11].Value.ToString();
            }

            // Lưu workbook vào tệp Excel đã chọn
            fileSaveExportExcel = saveFileDialog.FileName;
            workbook.SaveAs(fileSaveExportExcel);

            FrmConfirm frmConfirm = new FrmConfirm("Export thành công !\n Bạn có muốn mở file bây giờ ?", eImage.Question);
            frmConfirm.OnSendOKClicked += FrmConfirm_OnSendOKClicked; ;
            frmConfirm.ShowDialog();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void FrmConfirm_OnSendOKClicked(object sender)
    {
      try
      {
        Process.Start(fileSaveExportExcel);
      }
      catch (Exception)
      {
      }
    }

    private async void btnSave_Click_1(object sender, EventArgs e)
    {
      try
      {
        List<Product> products_add = new List<Product>();
        List<Product> products_exits = new List<Product>();
        if (productDTOs_Import?.Count() > 0)
        {
          foreach (var item in productDTOs_Import)
          {
            Product product = new Product();
            product.ProName = item.ProName;
            product.Code = item.Code;
            product.Description = item.Description;
            product.Type = item.Type;
            product.WeightOnPack = item.WeightOnPack;
            product.Unit = item.Unit;
            product.USL = item.USL;
            product.UCL = item.UCL;
            product.Target = item.Target;
            product.LCL = item.LCL;
            product.LSL = item.LSL;
            product.Density = item.Density;
            product.T = item.T;
            product.Tare = item.Tare;
            product.Note = item.Note;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            products_add.Add(product);
          }

          if (products_add?.Count() > 0)
          {
            await _productService.AddRangeAsync(products_add);
            new FrmInformation().ShowMessage("Lưu thành công !", eImage.Information);
          }
          else
          {
            new FrmInformation().ShowMessage("Không có dữ liệu để lưu !", eImage.Information);
          }
        }
        else
        {
          new FrmInformation().ShowMessage("Không có dữ liệu để lưu !", eImage.Information);
        }
      }
      catch (Exception)
      {

        throw;
      }
      finally
      {
        VisibleSave(false);
      }
    }
  }
}
