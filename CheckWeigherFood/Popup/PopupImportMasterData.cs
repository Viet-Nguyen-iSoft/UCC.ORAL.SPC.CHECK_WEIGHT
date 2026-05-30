using Aspose.Cells;
using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using CheckWeigherFood.FrmChild;
using Database.DTO;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;

namespace CheckWeigherFood.Popup
{
  public partial class PopupImportMasterData : Form
  {
    public event Action MasterDataChanged;

    private ProductService _productService { get; set; }
    private string fileName = string.Empty;

    private List<Product> _productDB = new List<Product>();
    private List<Product> _productNew = new List<Product>();
    private List<Product> _productRemove = new List<Product>();
    private List<ProductDTO> _excelImport = new List<ProductDTO>();
    private int _numberNew = 0;
    private int _numberSam = 0;
    private int _numberRemove = 0;

    public PopupImportMasterData()
    {
      InitializeComponent();
      RegisterService();

      this.btnConfirm.Click += BtnConfirm_Click;
      this.btnExit.Click += BtnExit_Click;
      this.btnImport.Click += BtnImport_Click;
    }

    private void BtnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void BtnImport_Click(object sender, EventArgs e)
    {
      DialogResult result = this.openFileDialog1.ShowDialog();
      if (result == DialogResult.OK)
      {
        if (backgroundWorker1.IsBusy == false)
        {
          LockUI(true);
          this.backgroundWorker1.RunWorkerAsync();
        }
      }
    }

    private async void BtnConfirm_Click(object sender, EventArgs e)
    {
      if (_productRemove?.Count() > 0 || _productNew?.Count() > 0)
      {
        if (_productRemove?.Count() > 0)
        {
          _productRemove.ForEach(x => x.DeletedFlag = true);
          await _productService.UpdateRangeAsync(_productRemove);
        }
        if (_productNew?.Count() > 0)
        {
          await _productService.AddRangeAsync(_productNew);
        }

        new FrmInformation().ShowMessage("Lưu dữ liệu thành công !", eImage.Information);
        MasterDataChanged?.Invoke();
        this.Close();
      }
      else
      {
        new FrmInformation().ShowMessage("Không có dữ liệu để lưu !", eImage.Information);
      }
    }

    public static List<Product> GetNotExistInB(
    List<Product> db,
    List<ProductDTO> excel)
    {
      return db
          .Where(a => !excel.Any(b => b.Code == a.Code))
          .ToList();
    }

    private void RegisterService()
    {
      _productService = AppFactory.CreateProductService();
    }

    private async Task<List<Product>> LoadData()
    {
      return await _productService.GetAllAsync();
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      fileName = openFileDialog1.FileName;
      ShowStatus("Đang tải ...");
    }

    private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        _excelImport = PareXlsxByAspose(fileName, 1);
        _productNew = new List<Product>();
        if (_excelImport?.Count > 0)
        {
          _productDB = await LoadData();
          foreach (var item in _excelImport)
          {
            EnumProductCheck isUpdate = CheckExits(item, _productDB);

            if (isUpdate == EnumProductCheck.New)
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

              _productNew.Add(product);
              _numberNew++;
            }
            else if (isUpdate == EnumProductCheck.Sam)
            {
              _numberSam++;
            }
          }
        }

        //Check xóa
        _productRemove = GetNotExistInB(_productDB, _excelImport);
        if (_productRemove?.Count() > 0)
        {
          _numberRemove = _productRemove.Count();
        }

        ShowNumberData();
        ShowStatus("Tải xong");
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Không load được File Excel MasterData !", eNumUI.eImage.Warning);
        ShowStatus("Lỗi khi tải");
      }
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      LockUI(false);
    }

    private EnumProductCheck CheckExits(ProductDTO productDTO, List<Product> products)
    {
      var listData = products?.Where(x => x.Code == productDTO.Code).ToList();
      if (listData?.Count() > 1)
      {
        return EnumProductCheck.Sam;
      }

      var exits = listData.FirstOrDefault();
      if (exits != null)
      {
        bool sam = exits.Code == productDTO.Code &&
                    exits.Description == productDTO.Description &&
                    exits.ProName == productDTO.ProName &&
                    exits.Type == productDTO.Type &&
                    exits.WeightOnPack == productDTO.WeightOnPack &&
                    exits.USL == productDTO.USL &&
                    exits.UCL == productDTO.UCL &&
                    exits.Target == productDTO.Target &&
                    exits.LCL == productDTO.LCL &&
                    exits.LSL == productDTO.LSL &&
                    exits.T == productDTO.T &&
                    exits.Note == productDTO.Note;
        return sam == true ? EnumProductCheck.None : EnumProductCheck.New;
      }
      else
      {
        return EnumProductCheck.New;
      }
    }

    private void ShowStatus(string msg)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowStatus(msg); }));
        return;
      }

      lbTitle.Text = msg;
    }

    private void ShowNumberData()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowNumberData(); }));
        return;
      }

      lbNumberDataAdd.Text = _numberNew.ToString();
      lbNumberDataSam.Text = _numberSam.ToString();
      lbNumberDataRemove.Text = _numberRemove.ToString();
    }

    private void LockUI(bool lockUI)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { LockUI(lockUI); }));
        return;
      }

      this.btnConfirm.Enabled = !lockUI;
      this.btnImport.Enabled = !lockUI;
    }



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

              bool valid = (!string.IsNullOrEmpty(row_data.Code)) &&
                            (!string.IsNullOrEmpty(row_data.Description)) &&
                            (!string.IsNullOrEmpty(row_data.Type)) &&
                            (row_data.USL > 0) &&
                            (row_data.UCL > 0) &&
                            (row_data.Target > 0) &&
                            (row_data.LCL > 0) &&
                            (row_data.LSL > 0);

              if (valid)
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

  }
}
