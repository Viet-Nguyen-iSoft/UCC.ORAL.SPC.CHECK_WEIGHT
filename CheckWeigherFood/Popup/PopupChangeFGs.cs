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
using static System.Runtime.CompilerServices.RuntimeHelpers;

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
      lbCode.SetLabelAlign(ContentAlignment.MiddleLeft);

      listBox1.DrawMode = DrawMode.OwnerDrawFixed;
      listBox1.ItemHeight = 40;
    }

    private ProductService _productService { get; set; }
    private List<Product> _allProducts { get; set; }
    private Product _product { get; set; }
    private void RegisterService()
    {
      _productService = AppFactory.CreateProductService();
    }

    private async void PopupChangeFGs_Load(object sender, EventArgs e)
    {
      _allProducts = await _productService.GetAllAsync();

      ShowListbox(_allProducts);

      this.listBox1.DrawItem += listBox1_DrawItem;
      this.listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
      this.txtSearch._TextChanged += TxtSearch__TextChanged;
    }

    private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listBox1.SelectedItem is Product product)
      {
        _product = product;
        ShowProduct(product);
      }
    }

    private void TxtSearch__TextChanged(object sender, EventArgs e)
    {
      string keyword = txtSearch.Texts.Trim().ToLower();

      var filtered = _allProducts
          .Where(x =>
              (!string.IsNullOrEmpty(x.Code) &&
               x.Code.ToLower().Contains(keyword))
              ||
              (!string.IsNullOrEmpty(x.Description) &&
               x.Description.ToLower().Contains(keyword))
              ||
              (!string.IsNullOrEmpty(x.ProName) &&
               x.ProName.ToLower().Contains(keyword))
          )
          .ToList();

      ShowListbox(filtered);
    }

    private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0) return;

      e.DrawBackground();

      Product product = (Product)listBox1.Items[e.Index];

      string text = $"{product.Code} - {product.Description}";

      TextRenderer.DrawText(
          e.Graphics,
          text,
          e.Font,
          e.Bounds,
          e.ForeColor,
          TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

      e.DrawFocusRectangle();
    }


    private void ShowListbox(List<Product> products)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowListbox(products); }));
        return;
      }


      listBox1.DataSource = products;
      listBox1.DisplayMember = nameof(Product.DisplayText);
    }

    private void ShowProduct(Product product)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowProduct(product); }));
        return;
      }

      btnConfirm.Visible = product != null;
      lbCode.ValueStr = product?.Code ?? string.Empty;
      lbName.ValueStr = product?.Description ?? string.Empty;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnConfirm_Click(object sender, EventArgs e)
    {
      OnSelectedProduct?.Invoke(_product);
      this.Close();
    }
  }
}
