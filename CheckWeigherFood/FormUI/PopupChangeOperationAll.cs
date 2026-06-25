using CheckWeigherFood.Controls;
using CheckWeigherFood.FrmChild;
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
using static CheckWeigherFood.eNum.eNumUI;
using static Database.Enum;

namespace CheckWeigherFood.FormUI
{
  public partial class PopupChangeOperationAll : Form
  {
    public event Action<Employee, Employee, Employee, Employee> OnSelectedEmployees;
    public PopupChangeOperationAll()
    {
      InitializeComponent();
      RegisterService();
      this.Load += PopupChangeOperator_Load;
    }

    private string _op3 {  get; set; }
    private string _op4 {  get; set; }
    private string _qc {  get; set; }
    private string _shitLeader {  get; set; }
    public PopupChangeOperationAll(string op03, string op04, string qc, string shiftleader):this()
    {
      _op3 = op03;
      _op4 = op04;
      _qc = qc;
      _shitLeader = shiftleader;
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

      var op03 = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.OP).ToList();
      ShowCbb(cbbOP03, op03);

      var op04 = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.OP).ToList();
      ShowCbb(cbbOP04, op04);

      var qc = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.QC).ToList();
      ShowCbb(cbbQC, qc);

      var tc = _employeeList?.Where(x => x.EnumTypeEmployee == EnumTypeEmployee.ShiftLeader).ToList();
      ShowCbb(cbbShiftLeader, tc);

      if (!string.IsNullOrEmpty(_op3))
      {
        var rs = _employeeList?.FirstOrDefault(x => x.FullName == _op3);
        cbbOP03.SelectedItem = rs;
      }
      if (!string.IsNullOrEmpty(_op4))
      {
        var rs = _employeeList?.FirstOrDefault(x => x.FullName == _op4);
        cbbOP04.SelectedItem = rs;
      }
      if (!string.IsNullOrEmpty(_qc))
      {
        var rs = _employeeList?.FirstOrDefault(x => x.FullName == _qc);
        cbbQC.SelectedItem = rs;
      }
      if (!string.IsNullOrEmpty(_shitLeader))
      {
        var rs = _employeeList?.FirstOrDefault(x => x.FullName == _shitLeader);
        cbbShiftLeader.SelectedItem = rs;
      }
    }

    private async Task<List<Employee>> LoadData(EnumTypeEmployee enumTypeEmployee)
    {
      return await _employeeService.GetAllAsync(enumTypeEmployee);
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
      comboBox.SelectedIndex = -1;
    }

    private void btnConfirm_Click(object sender, EventArgs e)
    {
      try
      {
        Employee empOP03 = cbbOP03.SelectedItem as Employee;
        Employee empOP04 = cbbOP04.SelectedItem as Employee;
        Employee empQC = cbbQC.SelectedItem as Employee;
        Employee empShiftLeader = cbbShiftLeader.SelectedItem as Employee;

        if (empOP03 == null)
        {
          new FrmInformation().ShowMessage("Vui lòng chọn Vận hành máy line 03 !", eImage.Warning);
          return;
        }
        if (empOP04 == null)
        {
          new FrmInformation().ShowMessage("Vui lòng chọn Vận hành máy line 04!", eImage.Warning);
          return;
        }
        if (empQC == null)
        {
          new FrmInformation().ShowMessage("Vui lòng chọn Chất lượng (QC) !", eImage.Warning);
          return;
        }
        if (empShiftLeader == null)
        {
          new FrmInformation().ShowMessage("Vui lòng chọn Trưởng ca !", eImage.Warning);
          return;
        }

        OnSelectedEmployees?.Invoke(empOP03, empOP04, empQC, empShiftLeader);

        this.Close();
      }
      catch (Exception ex)
      {
        new FrmInformation().ShowMessage($"Lỗi: {ex.ToString()}", eImage.Warning);
      }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close(); 
    }
  }
}
