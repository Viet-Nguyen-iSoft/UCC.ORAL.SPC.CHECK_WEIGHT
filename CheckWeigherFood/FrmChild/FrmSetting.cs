using CheckWeigherFood.Controls;
using CheckWeigherFood.Models;
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

namespace CheckWeigherFood.FrmChild
{
  public partial class FrmSetting : Form
  {
    public FrmSetting()
    {
      InitializeComponent();
    }

    public delegate void SendSavePLC(object sender, string PLCIP, int Port);
    public event SendSavePLC OnSendSavePLC;

    public delegate void SendAddOP();
    public event SendAddOP OnSendAddOP;
    public delegate void SendAddQC();
    public event SendAddQC OnSendAddQC;
    public delegate void SendAddTC();
    public event SendAddTC OnSendAddTC;

    public delegate void SendSavePathReport(object sender, string path);
    public event SendSavePathReport OnSendSavePathReport;

    public delegate void SendSaveNumberSafe(object sender, ulong NumberSafe);
    public event SendSaveNumberSafe OnSendSaveNumberSafe;

    public delegate void SendSaveNumverQuality(object sender, ulong NumberQuality);
    public event SendSaveNumverQuality OnSendSaveNumverQuality;


    public delegate void SendSaveLoBB(object sender, string LoBB);
    public event SendSaveLoBB OnSendSaveLoBB;

    public delegate void SendSaveOR(object sender, int OR);
    public event SendSaveOR OnSendSaveOR;

    public delegate void SendSaveOLE(object sender, int OLE);
    public event SendSaveOLE OnSendSaveOLE;


    #region Singleton parttern
    private static FrmSetting _Instance = null;
    public static FrmSetting Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new FrmSetting();
        }
        return _Instance;
      }
    }

    #endregion

    private void FrmSetting_Load(object sender, EventArgs e)
    {
      LoadData();
    }



    private void LoadData()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          LoadData();
        }));
        return;
      }
      //Setting SMC
      this.numericUpDownLow.Value = Properties.Settings.Default.valueLow;
      this.numericUpDownMid.Value = Properties.Settings.Default.valueMid;
      this.numericUpDownHigh.Value = Properties.Settings.Default.valueHigh;

      //OLE, OR
      this.txtSettingTargetOLE.Text = Properties.Settings.Default.OLE.ToString();
      this.txtSettingTargetOR.Text = Properties.Settings.Default.OR.ToString();

      //Date
      this.dateTimePickerSafe.Value = ConverStrToDateTime(Properties.Settings.Default.DateSafe);
      this.dateTimePickerQuality.Value = ConverStrToDateTime(Properties.Settings.Default.DateQuality);

      if (AppCore.Ins._lineCurrent!=null)
      {
        this.txtIpPLC.Text = AppCore.Ins._lineCurrent.IpPLC.ToString();
        this.txtPort.Text = AppCore.Ins._lineCurrent.Port.ToString();
        this.txtReport.Text = AppCore.Ins._lineCurrent.PathReport.ToString();
      }
    }

    private DateTime ConverStrToDateTime(string datetime)
    {
      DateTime now = DateTime.Now;
      try
      {
        if (datetime != "")
        {
          now = Convert.ToDateTime(datetime);
        }
      }
      catch
      {
      }
      return now;
    }

    private async void btnSaveOP_Click(object sender, EventArgs e)
    {
      string userName = textBoxOP.Text.Trim();
      if (userName=="")
      {
        new FrmInformation().ShowMessage($"Vui lòng nhập tên OP !", eImage.Warning);
        return;
      }

      User user = new User()
      {
        Name = userName,
        Role = "OP",
        CreatedAt = DateTime.Now,
        isEnable = true,
      };
      await AppCore.Ins.AddUser(user);

      OnSendAddOP?.Invoke();

      new FrmInformation().ShowMessage($"{this.textBoxOP.Text} đã được thêm vào danh sách OP !", eImage.Information);
      textBoxOP.Text = "";

      //List<User> users = new List<User>();
      //users = await AppCore.Ins.LoadUser();

      //string userName = this.textBoxOP.Text.ToString().Trim();
      //if (users.Count>0 && users!=null)
      //{
      //  bool isExitUser = await AppCore.Ins.CheckUserExit(userName);
      //  await AppCore.Ins.ClearActiveUser("OP");
      //  if (!isExitUser)
      //  {
      //    User user = new User()
      //    {
      //      Name = userName,
      //      Role = "OP",
      //      CreatedAt = DateTime.Now,
      //      isEnable = true,
      //    };
      //    await AppCore.Ins.AddUser(user);
      //  }
      //  else
      //  {
      //    await AppCore.Ins.ActiveUser(userName,"OP");
      //  }
      //}
      //else
      //{
      //  await AppCore.Ins.ClearActiveUser("OP");
      //  User user = new User()
      //  {
      //    Name = userName,
      //    Role = "OP",
      //    isEnable = true,
      //    CreatedAt = DateTime.Now,
      //  };
      //  await AppCore.Ins.AddUser(user);
      //}

      //if (OnSendSaveOP != null)
      //  OnSendSaveOP(this, userName);

    }

    private async void btnSaveQC_Click(object sender, EventArgs e)
    {
      string userName = textBoxQC.Text.Trim();
      if (userName == "")
      {
        new FrmInformation().ShowMessage($"Vui lòng nhập tên QC !", eImage.Warning);
        return;
      }

      User user = new User()
      {
        Name = userName,
        Role = "QC",
        CreatedAt = DateTime.Now,
        isEnable = true,
      };
      await AppCore.Ins.AddUser(user);

      OnSendAddQC?.Invoke();

      new FrmInformation().ShowMessage($"{this.textBoxQC.Text} đã được thêm vào danh sách QC !", eImage.Information);
      textBoxQC.Text = "";
      //List<User> users = new List<User>();
      //users = await AppCore.Ins.LoadUser();

      //string userName = this.cbQC.Text.ToString().Trim();
      //if (users.Count > 0 && users != null)
      //{
      //  bool isExitUser = await AppCore.Ins.CheckUserExit(userName);
      //  await AppCore.Ins.ClearActiveUser("QC");
      //  if (!isExitUser)
      //  {
      //    User user = new User()
      //    {
      //      Name = userName,
      //      Role = "QC",
      //      CreatedAt = DateTime.Now,
      //      isEnable = true,
      //    };
      //    await AppCore.Ins.AddUser(user);
      //  }
      //  else
      //  {
      //    await AppCore.Ins.ActiveUser(userName, "QC");
      //  }
      //}
      //else
      //{
      //  await AppCore.Ins.ClearActiveUser("QC");
      //  User user = new User()
      //  {
      //    Name = userName,
      //    Role = "QC",
      //    isEnable = true,
      //    CreatedAt = DateTime.Now,
      //  };
      //  await AppCore.Ins.AddUser(user);
      //}
      //if (OnSendSaveQC != null)
      //  OnSendSaveQC(this, userName);
    }

    private async void btnSaveTC_Click(object sender, EventArgs e)
    {
      string userName = textBoxTC.Text.Trim();
      if (userName == "")
      {
        new FrmInformation().ShowMessage($"Vui lòng nhập tên TC !", eImage.Warning);
        return;
      }

      User user = new User()
      {
        Name = userName,
        Role = "TC",
        CreatedAt = DateTime.Now,
        isEnable = true,
      };
      await AppCore.Ins.AddUser(user);

      OnSendAddTC?.Invoke();

      new FrmInformation().ShowMessage($"{this.textBoxTC.Text} đã được thêm vào danh sách TC !", eImage.Information);
      textBoxTC.Text = "";
      //List<User> users = new List<User>();
      //users = await AppCore.Ins.LoadUser();

      //string userName = this.cbTC.Text.ToString().Trim();
      //if (users.Count > 0 && users != null)
      //{
      //  bool isExitUser = await AppCore.Ins.CheckUserExit(userName);
      //  await AppCore.Ins.ClearActiveUser("TC");
      //  if (!isExitUser)
      //  {
      //    User user = new User()
      //    {
      //      Name = userName,
      //      Role = "TC",
      //      CreatedAt = DateTime.Now,
      //      isEnable = true,
      //    };
      //    await AppCore.Ins.AddUser(user);
      //  }
      //  else
      //  {
      //    await AppCore.Ins.ActiveUser(userName, "TC");
      //  }
      //}
      //else
      //{
      //  await AppCore.Ins.ClearActiveUser("TC");
      //  User user = new User()
      //  {
      //    Name = userName,
      //    Role = "TC",
      //    isEnable = true,
      //    CreatedAt = DateTime.Now,
      //  };
      //  await AppCore.Ins.AddUser(user);
      //}
      //if (OnSendSaveTC != null)
      //  OnSendSaveTC(this, userName);
    }


    private ulong SettingNumberSafe = 0;
    private ulong SettingNumberQuality = 0;
    private void dateTimePickerSafe_ValueChanged(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          dateTimePickerSafe_ValueChanged(sender, e);
        }));
        return;
      }

      try
      {
        DateTime currentDate = DateTime.Now;
        DateTime selectedDateSafety = dateTimePickerSafe.Value;
        if (selectedDateSafety > currentDate)
        {
          //new FrmInformation().ShowMessage("Chọn ngày an toàn phải nhỏ hơn ngày hiện tại", eImage.Warning);
        }
        else
        {
          TimeSpan numbersSafety = currentDate.Subtract(selectedDateSafety);
          SettingNumberSafe = (ulong)numbersSafety.Days +1;
          txtSettingSafe.Text = SettingNumberSafe.ToString();
        }
      }
      catch (Exception)
      {
        MessageBox.Show("Dữ liệu nhập vào lỗi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void dateTimePickerQuality_ValueChanged(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          dateTimePickerQuality_ValueChanged(sender, e);
        }));
        return;
      }


      try
      {
        DateTime currentDate = DateTime.Now;
        DateTime selectedDateQuality = dateTimePickerQuality.Value;
        if (selectedDateQuality > currentDate)
        {
          new FrmInformation().ShowMessage("Chọn ngày an toàn phải nhỏ hơn ngày hiện tại", eImage.Warning);
        }
        else
        {
          TimeSpan numbersQuality = currentDate.Subtract(selectedDateQuality);
          SettingNumberQuality = (ulong)numbersQuality.Days + 1;
          txtSettingQuality.Text = SettingNumberQuality.ToString();
        }
      }
      catch (Exception)
      {
        MessageBox.Show("Dữ liệu nhập vào lỗi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnSaveQuality_Click(object sender, EventArgs e)
    {
      if (OnSendSaveNumverQuality != null)
      {
        OnSendSaveNumverQuality(this, SettingNumberQuality);
      }
      Properties.Settings.Default.DateQuality = dateTimePickerQuality.Value.ToString();
      Properties.Settings.Default.Save();
    }

    private void btnSaveSafe_Click(object sender, EventArgs e)
    {
      if (OnSendSaveNumberSafe != null)
      {
        OnSendSaveNumberSafe(this, SettingNumberSafe);
      }

      Properties.Settings.Default.DateSafe = dateTimePickerSafe.Value.ToString();
      Properties.Settings.Default.Save();
    }


    private void btnOLE_Click(object sender, EventArgs e)
    {
      int OLE = 0;
      int.TryParse(this.txtSettingTargetOLE.Text, out OLE);

      if (OnSendSaveOLE != null)
      {
        OnSendSaveOLE(this, OLE);
      }
      Properties.Settings.Default.OLE = OLE;
      Properties.Settings.Default.Save();
    }

    private void btnOR_Click(object sender, EventArgs e)
    {
      int OR = 0;
      int.TryParse(this.txtSettingTargetOR.Text, out OR);

      if (OnSendSaveOR != null)
      {
        OnSendSaveOR(this, OR);
      }
      Properties.Settings.Default.OR = OR;
      Properties.Settings.Default.Save();
    }

    private void btnSaveChangePLC_Click(object sender, EventArgs e)
    {
      if (btnSaveChangePLC.Text.Trim() == "Change")
      {
        this.txtIpPLC.Enabled = true;
        this.txtPort.Enabled = true;
        btnSaveChangePLC.Text = "Save";
      }
      else
      {
        int port = 0;
        int.TryParse(this.txtPort.Text, out port);

        if (OnSendSavePLC != null)
        {
          OnSendSavePLC(this, this.txtIpPLC.Text, port);
        }
        this.txtIpPLC.Enabled = false;
        this.txtPort.Enabled = false;
        btnSaveChangePLC.Text = "Change";
      }
      
      
    }

    private void picPathReport_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Chọn thư mục cần lưu Report";
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        this.txtReport.Text = folderBrowserDialog.SelectedPath;
        this.txtReport.ForeColor = Color.Red;
      }
    }

    private void btnSavePath_Click(object sender, EventArgs e)
    {
      this.txtReport.ForeColor = Color.Black;
      if (OnSendSavePathReport != null)
      {
        OnSendSavePathReport(this, this.txtReport.Text);
      }
    }





    private void tableLayoutPanel60_Paint(object sender, PaintEventArgs e)
    {

    }

    private void btnSetTemperature_Click(object sender, EventArgs e)
    {
      int lowValue = (int)numericUpDownLow.Value;
      int midValue = (int)numericUpDownMid.Value;
      int highValue = (int)numericUpDownHigh.Value;

      if (midValue <=0 || midValue >=100|| midValue <= 0 || midValue >= 100|| highValue <= 0 || highValue >= 100)
      {
        new FrmInformation().ShowMessage("Các giá trị nằm trong khoảng 0-100 °C", eImage.Warning);
        return;
      }

      if (midValue<= lowValue)
      {
        new FrmInformation().ShowMessage("Giá trị Mid phải lớn hơn Low", eImage.Warning);
        return;
      }
      if (highValue <= midValue)
      {
        new FrmInformation().ShowMessage("Giá trị High phải lớn hơn Mid", eImage.Warning);
        return;
      }

      Properties.Settings.Default.valueLow = lowValue;
      Properties.Settings.Default.valueMid = midValue;
      Properties.Settings.Default.valueHigh = highValue;
      Properties.Settings.Default.Save();

      AppCore.Ins.SendSettingTemperatureSMC(lowValue, midValue, highValue);
    }
  }
}
