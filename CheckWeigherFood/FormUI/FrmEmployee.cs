using CheckWeigherFood.Controls;
using CheckWeigherFood.Popup;
using Database.DTO;
using Database.DtoHelper;
using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
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
      popupAddNewEmployee.OnReload += PopupAddNewEmployee_OnReload;
      popupAddNewEmployee.ShowDialog();
    }

    private async void PopupAddNewEmployee_OnReload()
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
      //dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      //dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    
  }
}
