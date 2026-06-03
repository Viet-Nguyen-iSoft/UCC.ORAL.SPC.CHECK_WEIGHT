using CheckWeigherFood.Controls;
using CheckWeigherFood.eNum;
using CheckWeigherFood.FrmChild;
using Database.Models;
using Database.Service;
using System;
using System.Windows.Forms;
using static Database.Enum;

namespace CheckWeigherFood.Popup
{
  public partial class PopupAddNewEmployee : Form
  {
    public event Action OnReload;
    public PopupAddNewEmployee()
    {
      InitializeComponent();
      RegisterService();
    }

    private EnumTypePopup _enumTypePopup = EnumTypePopup.Add;
    private Employee _employee { get;set; }
    public PopupAddNewEmployee(Employee employee):this()
    {
      _enumTypePopup = EnumTypePopup.Edit;
      _employee = employee;
      this.btnAdd.Text = "Cập nhật";
      ShowInforEmployee(employee);
    }

    private EmployeeService _employeeService { get; set; }
    private void RegisterService()
    {
      _employeeService = AppFactory.CreateEmployeeService();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtName.Texts.Trim()))
        {
          new FrmInformation().ShowMessage("Vui lòng nhập tên !", eNumUI.eImage.Warning);
          return;
        }
        if (cbbGroup.SelectedIndex < 0)
        {
          new FrmInformation().ShowMessage("Vui lòng chọn nhóm nhân viên !", eNumUI.eImage.Warning);
          return;
        }

        if (_enumTypePopup == EnumTypePopup.Add)
        {
          Employee employee = new Employee();
          employee.FullName = txtName.Texts;
          employee.Code = txtName.Texts;
          employee.EnumTypeEmployee = (EnumTypeEmployee)(cbbGroup.SelectedIndex + 1);
          employee.CreatedAt = DateTime.UtcNow;
          employee.UpdatedAt = DateTime.UtcNow;

          await _employeeService.AddAsync(employee);
          OnReload?.Invoke();
          this.Close();
        }
        else if (_enumTypePopup == EnumTypePopup.Edit)
        {
          _employee.FullName = txtName.Texts;
          _employee.EnumTypeEmployee = (EnumTypeEmployee)(cbbGroup.SelectedIndex + 1);
          _employee.UpdatedAt = DateTime.UtcNow;

          await _employeeService.UpdateAsync(_employee);
          OnReload?.Invoke();
          this.Close();
        }
      }
      catch (Exception)
      {

      }
    }

    private void ShowInforEmployee(Employee employee)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowInforEmployee(employee); }));
        return;
      }

      txtName.Texts = employee.FullName;
      cbbGroup.SelectedIndex = (int)employee.EnumTypeEmployee - 1;
    }
  }
}
