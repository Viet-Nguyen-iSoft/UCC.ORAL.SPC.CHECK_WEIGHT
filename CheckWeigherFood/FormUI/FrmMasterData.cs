using Aspose.Cells;
using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using CheckWeigherFood.Popup;
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
      txtSearch._TextChanged += TxtSearch__TextChanged;
    }

    private void TxtSearch__TextChanged(object sender, EventArgs e)
    {
      SearchData();
    }

    private async void FrmMasterData_Shown(object sender, EventArgs e)
    {
      _products = await LoadData();
      var dto = HelperDTO.ConvertProductDTO(_products, "");
      ShowDgv(dto);
    }
    private void btnSearch_Click(object sender, EventArgs e)
    {
      SearchData();
    }

    private void SearchData()
    {
      try
      {
        //LockUI(true);

        var dto = HelperDTO.ConvertProductDTO(_products, txtSearch.Texts);
        ShowDgv(dto);
      }
      catch (Exception)
      {

      }
      finally
      {
        //LockUI(false);
      }
    }

    private async Task<List<Product>> LoadData()
    {
      return await _productService.GetAllAsync();
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      PopupImportMasterData popupImportMasterData = new PopupImportMasterData();
      popupImportMasterData.MasterDataChanged += PopupImportMasterData_MasterDataChanged;
      popupImportMasterData.ShowDialog();
    }

    private async void PopupImportMasterData_MasterDataChanged()
    {
      _products = await LoadData();
      var dto = HelperDTO.ConvertProductDTO(_products, "");
      ShowDgv(dto);
    }

    private void ShowDgv(List<ProductDTO> productDTOs)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowDgv(productDTOs); }));
        return;
      }

      if (productDTOs?.Count() > 0)
      {
        productDTOs = productDTOs.OrderBy(x => x.Code).ToList();
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




    private void LockUI(bool lockUI)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { LockUI(lockUI); }));
        return;
      }

      //btnSave.Enabled = !lockUI;
    }


  }
}
