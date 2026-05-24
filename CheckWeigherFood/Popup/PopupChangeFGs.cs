using CheckWeigherFood.Controls;
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
  public partial class PopupChangeFGs : Form
  {
    public event Action<Product> OnSelectedProduct;
    public PopupChangeFGs()
    {
      InitializeComponent();
      RegisterService();
      CustomUI();
      this.Load += PopupChangeFGs_Load;
    }

    private void CustomUI()
    {
      lbName.SetLabelAlign(ContentAlignment.MiddleLeft);
    }

    private ProductService _productService { get; set; }
    private void RegisterService()
    {
      _productService = AppFactory.CreateProductService();
    }

    private async void PopupChangeFGs_Load(object sender, EventArgs e)
    {
      var datas = await _productService.GetAllAsync();
      if (datas?.Count()>0)
      {
        datas = datas.OrderBy(x => x.Code).ToList();
      }  
      ShowCbb(datas);

      this.cbbFGs.SelectedIndexChanged += CbbFGs_SelectedIndexChanged;
    }

    private void ShowCbb(List<Product> products)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowCbb(products); }));
        return;
      }


      cbbFGs.DataSource = products;
      cbbFGs.DisplayMember = nameof(Product.Code);
    }

    private void ShowProduct(Product product)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowProduct(product); }));
        return;
      }


      lbName.ValueStr = product?.Description ?? string.Empty;
    }

    private void CbbFGs_SelectedIndexChanged(object sender, EventArgs e)
    {
      Product product = cbbFGs.SelectedItem as Product;
      ShowProduct(product);
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnConfirm_Click(object sender, EventArgs e)
    {
      Product product = cbbFGs.SelectedItem as Product;
      OnSelectedProduct?.Invoke(product);
      this.Close();
    }
  }
}
