using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using CheckWeigherFood.FrmChild;
using CustomControls.RJControls;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.Popup
{
  public partial class PopupAddNewProduct : Form
  {
    public event Action MasterDataChanged;
    public PopupAddNewProduct()
    {
      InitializeComponent();
      RegisterService();
      this.Load += PopupAddNewProduct_Load;
    }

    private ProductService _productService { get; set; }
    private void RegisterService()
    {
      _productService = AppFactory.CreateProductService();
    }
    private void PopupAddNewProduct_Load(object sender, EventArgs e)
    {
      txtUSL.KeyPress += TextBox_PositiveDecimalOnly;
      txtUCL.KeyPress += TextBox_PositiveDecimalOnly;
      txtTarget.KeyPress += TextBox_PositiveDecimalOnly;
      txtLCL.KeyPress += TextBox_PositiveDecimalOnly;
      txtLSL.KeyPress += TextBox_PositiveDecimalOnly;
    }
    private void TextBox_PositiveDecimalOnly(object sender, KeyPressEventArgs e)
    {
      RJTextBox txt = sender as RJTextBox;
      if (char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        return;
      if (e.KeyChar == '.' && !txt.Texts.Contains("."))
        return;
      e.Handled = true;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private async void btnConfig_Click(object sender, EventArgs e)
    {
      try
      {
        bool isFillFull = (string.IsNullOrEmpty(txtCode.Texts)) ||
                           (string.IsNullOrEmpty(txtDescription.Texts)) ||
                           (string.IsNullOrEmpty(txtLSL.Texts)) ||
                           (string.IsNullOrEmpty(txtLCL.Texts)) ||
                           (string.IsNullOrEmpty(txtTarget.Texts)) ||
                           (string.IsNullOrEmpty(txtUCL.Texts)) ||
                           (string.IsNullOrEmpty(txtUSL.Texts)) ||
                           (string.IsNullOrEmpty(txtType.Texts)) ||
                           cbbGroup.SelectedIndex == -1;

        if (isFillFull)
        {
          new FrmInformation().ShowMessage("Vui lòng nhập thông tin đầy đủ !", eNumUI.eImage.Warning);
          return;
        }

        Product product = new Product();
        product.ProName = txtProname.Texts.Trim();
        product.Code = txtCode.Texts.Trim();
        product.Description = txtDescription.Texts.Trim();
        product.Type = txtType.Texts.Trim();
        product.USL = double.Parse(txtUSL.Texts);
        product.UCL = double.Parse(txtUCL.Texts);
        product.Target = double.Parse(txtTarget.Texts);
        product.LCL = double.Parse(txtLCL.Texts);
        product.LSL = double.Parse(txtLSL.Texts);
        product.IsAbsolute = IsProductTuyetDoi(cbbGroup.SelectedItem.ToString());
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        await _productService.AddAsync(product);
        MasterDataChanged?.Invoke();
        this.Close();
      }
      catch (Exception ex)
      {
        new FrmInformation().ShowMessage($"Lỗi thêm dữ liệu: {ex.ToString()}", eNumUI.eImage.Warning);
      }
    }

    public static bool IsProductTuyetDoi(string note)
    {
      return (note.Trim().ToLower() == "TL tuyệt đối".Trim().ToLower());
    }
  }
}
