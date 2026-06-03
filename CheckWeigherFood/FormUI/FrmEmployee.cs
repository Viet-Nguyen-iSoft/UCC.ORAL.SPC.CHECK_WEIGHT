using CheckWeigherFood.Controls;
using CheckWeigherFood.FrmChild;
using CheckWeigherFood.Popup;
using Database.DTO;
using Database.DtoHelper;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckWeigherFood.eNum.eNumUI;
using static Database.Enum;

namespace CheckWeigherFood.FormUI
{
  public partial class FrmEmployee : Form
  {
    public FrmEmployee()
    {
      InitializeComponent();
      RegisterService();
      this.Load += FrmEmployee_Load;
      this.Shown += FrmEmployee_Shown;
    }

    #region Singleton parttern
    private static FrmEmployee _Instance = null;
    public static FrmEmployee Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmEmployee();
        }
        return _Instance;
      }
    }
    #endregion

    private EmployeeService _employeeService { get; set; }
    private List<Employee> _employees { get; set; }
    private void RegisterService()
    {
      _employeeService = AppFactory.CreateEmployeeService();
    }

    private void FrmEmployee_Load(object sender, System.EventArgs e)
    {
      cbbGroup.SelectedIndex = 0;
      cbbGroup.SelectedIndexChanged += CbbGroup_SelectedIndexChanged;
      this.dgv.CellContentClick += Dgv_CellContentClick;
    }

    private Employee _employeeRemove { get; set; }
    private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0)
        return;

      try
      {
        if (dgv.Columns[e.ColumnIndex].Name == "Edit")
        {
          var item = (EmployeeDTO)dgv.Rows[e.RowIndex].DataBoundItem;
          PopupAddNewEmployee popup = new PopupAddNewEmployee(item?.Employee);
          popup.OnReload += PopupEmployee_OnReload;
          popup.ShowDialog();
        }
        else if (dgv.Columns[e.ColumnIndex].Name == "Remove")
        {
          _employeeRemove = ((EmployeeDTO)dgv.Rows[e.RowIndex].DataBoundItem)?.Employee;
          if (_employeeRemove != null)
          {
            FrmConfirm frmConfirm = new FrmConfirm("Bạn có chắc chắn xóa thông tin nhân viên này ?", eImage.Question);
            frmConfirm.OnSendOKClicked += FrmConfirm_OnSendOKClicked;
            frmConfirm.ShowDialog();
          }

        }
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Lỗi !", eImage.Warning);
      }
    }

    private async void FrmConfirm_OnSendOKClicked(object sender)
    {
      try
      {
        await _employeeService.RemoveAsync(_employeeRemove);
        PopupEmployee_OnReload();
      }
      catch (Exception)
      {
        new FrmInformation().ShowMessage("Xóa thất bại !", eImage.Warning);
      }
    }

    private async void CbbGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      EnumTypeEmployee enumType = (EnumTypeEmployee)cbbGroup.SelectedIndex;
      _employees = await LoadData(enumType);
      var dto = HelperDTO.ConvertEmployeeDTO(_employees);
      ShowDgv(dto);
    }

    private async void FrmEmployee_Shown(object sender, System.EventArgs e)
    {
      EnumTypeEmployee enumType = (EnumTypeEmployee)cbbGroup.SelectedIndex;
      _employees = await LoadData(enumType);
      var dto = HelperDTO.ConvertEmployeeDTO(_employees);
      ShowDgv(dto);
    }

   

    private async void btnSearch_Click(object sender, EventArgs e)
    {
      EnumTypeEmployee enumType = (EnumTypeEmployee)cbbGroup.SelectedIndex;
      _employees = await LoadData(enumType);
      var dto = HelperDTO.ConvertEmployeeDTO(_employees);
      ShowDgv(dto);
    }

    private void btnAddNew_Click(object sender, System.EventArgs e)
    {
      PopupAddNewEmployee popupAddNewEmployee = new PopupAddNewEmployee();
      popupAddNewEmployee.OnReload += PopupEmployee_OnReload;
      popupAddNewEmployee.ShowDialog();
    }

    private async void PopupEmployee_OnReload()
    {
      EnumTypeEmployee enumType = (EnumTypeEmployee)cbbGroup.SelectedIndex;
      _employees = await LoadData(enumType);
      var dto = HelperDTO.ConvertEmployeeDTO( _employees);
      ShowDgv(dto);
    }

    private async Task<List<Employee>> LoadData(EnumTypeEmployee enumTypeEmployee)
    {
      return await _employeeService.GetAllAsync(enumTypeEmployee);
    }

    private void ShowDgv(List<EmployeeDTO> employeeDTOs)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowDgv(employeeDTOs); }));
        return;
      }


      dgv.DataSource = employeeDTOs;
      dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

      if (dgv.Columns.Contains("Edit"))
      {
        dgv.Columns.Remove("Edit");
      }
      DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
      btnEdit.Name = "Edit";
      btnEdit.HeaderText = "";
      btnEdit.Text = "Chỉnh sửa";
      btnEdit.UseColumnTextForButtonValue = true;
      btnEdit.Width = 200;
      btnEdit.MinimumWidth = 200;
      btnEdit.Resizable = DataGridViewTriState.False;
      btnEdit.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dgv.Columns.Add(btnEdit);


      if (dgv.Columns.Contains("Remove"))
      {
        dgv.Columns.Remove("Remove");
      }
      DataGridViewButtonColumn btnRemove = new DataGridViewButtonColumn();
      btnRemove.Name = "Remove";
      btnRemove.HeaderText = "";
      btnRemove.Text = "Xóa";
      btnRemove.UseColumnTextForButtonValue = true;
      btnRemove.Width = 200;
      btnRemove.MinimumWidth = 200;
      btnRemove.Resizable = DataGridViewTriState.False;
      btnRemove.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dgv.Columns.Add(btnRemove);
    }

    


  }
}
