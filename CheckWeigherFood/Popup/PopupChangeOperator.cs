using CheckWeigherFood.Controls;
using Database.DTO;
using Database.DtoHelper;
using Database.Models;
using Database.Service;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Database.Enum;

namespace CheckWeigherFood.Popup
{
  public partial class PopupChangeOperator : Form
  {
    public event Action<Employee, Employee, Employee> OnSelectedEmployees;
    public PopupChangeOperator()
    {
      InitializeComponent();
      RegisterService();

      this.Load += PopupChangeOperator_Load;
      this.Shown += PopupChangeOperator_Shown;
    }

    

    private EmployeeService _employeeService { get; set; }
    private void RegisterService()
    {
      _employeeService = AppFactory.CreateEmployeeService();
    }


    private List<Employee> _employeeList = new List<Employee>();
    private async void PopupChangeOperator_Load(object sender, EventArgs e)
    {
      _employeeList = await LoadData(EnumTypeEmployee.None);

    }

    private void PopupChangeOperator_Shown(object sender, EventArgs e)
    {
      var op = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.OP).ToList();
      ShowCbb(cbbOP, op);

      var qc = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.QC).ToList();
      ShowCbb(cbbQC, qc);

      var tc = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.ShiftLeader).ToList();
      ShowCbb(cbbShiftLeader, tc);
    }

    private void ShowCbb(ComboBox comboBox, List<Employee> employees)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() => { ShowCbb(comboBox, employees); }));
        return;
      }


      comboBox.DataSource = employees;
      comboBox.DisplayMember = nameof(Employee.FullName);
    }

    private async Task<List<Employee>> LoadData(EnumTypeEmployee enumTypeEmployee)
    {
      return await _employeeService.GetAllAsync(enumTypeEmployee);
    }

    private void btnConfirm_Click(object sender, EventArgs e)
    {
      Employee empOP = cbbOP.SelectedItem as Employee;
      Employee empQC = cbbQC.SelectedItem as Employee;
      Employee empShiftLeader = cbbShiftLeader.SelectedItem as Employee;

      OnSelectedEmployees?.Invoke(empOP, empQC, empShiftLeader);

      this.Close();
    }
  }
}
