using CheckWeigherFood.Controls;
using CheckWeigherFood.InitChart;
using Database.DTO;
using Database.Models;
using DocumentFormat.OpenXml.Math;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

      if (product!=null)
      {
        this.lbT.Text = $"{product.T }";
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
      }

    }
  }
}
