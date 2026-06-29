using Database.DTO;
using Database.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckWeigherFood.UC
{
  public partial class UcInformationDataSumary : UserControl
  {
    public UcInformationDataSumary()
    {
      InitializeComponent();
    }


    private void ResetDashBoard()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          ResetDashBoard();
        }));
        return;
      }

      this.lbSample.Text = "0";
      this.lbMin.Text = "0";
      this.lbMax.Text = "0";
      this.lbCp.Text = "0";
      this.lbCpk.Text = "0";
    }

    public void SetInforProduct(Product product, double tube, double tailTube, double carton)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetInforProduct(product, tube, tailTube, carton);
        }));
        return;
      }

      if (product != null)
      {
        this.lbUpper.Text = $"{product.USL + tube + carton - tailTube}";
        this.lbUpperControl.Text = $"{product.UCL + tube + carton - tailTube}";
        this.lbTarget.Text = $"{product.Target + tube + carton - tailTube}";

        if (product.IsAbsolute)
        {
          this.lbGroup.Text = "TL tuyệt đối";
          this.lbLowerControl.Text = $"-";
          this.lbLower.Text = $"-";
        }
        else
        {
          this.lbGroup.Text = "TL trung bình";
          this.lbLowerControl.Text = $"{product.LCL + tube + carton - tailTube}";
          this.lbLower.Text = $"{product.LSL + tube + carton - tailTube}";
        }
      }
      else
      {
        this.lbUpper.Text = $"-";
        this.lbUpperControl.Text = $"-";
        this.lbTarget.Text = $"-";

        this.lbGroup.Text = "N/A";
        this.lbLowerControl.Text = $"-";
        this.lbLower.Text = $"-";
      }  
    }

    public void SetWeightRealtime(double weight)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetWeightRealtime(weight);
        }));
        return;
      }

      lbWeightRealtime.Text = weight.ToString();
    }

    public void SetSumaryDTO(SumaryDTO sumaryDTO)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetSumaryDTO(sumaryDTO);
        }));
        return;
      }

      if (sumaryDTO != null)
      {
        this.lbMax.Text = $"{sumaryDTO.Max}";
        this.lbMin.Text = $"{sumaryDTO.Min}";
        this.lbCp.Text = $"{sumaryDTO.Cp}";
        this.lbCpk.Text = $"{sumaryDTO.Cpk}";
        this.lbSample.Text = $"{sumaryDTO.Sample}";

        switch (sumaryDTO.EnumResult)
        {
          case EnumResult.None:
            this.lbResult.Text = "N/A";
            this.lbResult.BackColor = Color.Gray;
            break;
          case EnumResult.Pass:
            this.lbResult.Text = "ĐẠT";
            this.lbResult.BackColor = Color.DarkGreen;
            break;
          case EnumResult.Fail:
            this.lbResult.Text = "KHÔNG ĐẠT";
            this.lbResult.BackColor = Color.Red;
            break;
          default:
            break;
        }
      }
      else
      {
        this.lbMax.Text = $"0.0";
        this.lbMin.Text = $"0.0";
        this.lbCp.Text = $"0.0";
        this.lbCpk.Text = $"0.0";
        this.lbSample.Text = $"0.0";

        this.lbResult.Text = "N/A";
        this.lbResult.BackColor = Color.Gray;
      }  
    }

    public void SetValueWeightRealtime(double value)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(() =>
        {
          SetValueWeightRealtime(value);
        }));
        return;
      }

      this.lbWeightRealtime.Text = $"{value}";

    }
  }
}
