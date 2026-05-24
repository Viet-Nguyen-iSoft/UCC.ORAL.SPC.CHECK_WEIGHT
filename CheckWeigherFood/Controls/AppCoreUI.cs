using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    //public FrmMain frmMain;
    //private void StartShowUI()
    //{
    //  frmMain = new FrmMain() { WindowState = FormWindowState.Maximized };
    //  frmMain.StartPosition = FormStartPosition.CenterScreen;
    //  Application.Run(frmMain);
    //}

    //public FrmMain frmMain;
    private void StartShowUI()
    {
      //frmMain = new FrmMain() { WindowState = FormWindowState.Maximized };
      //frmMain.StartPosition = FormStartPosition.CenterScreen;
      Application.Run(FrmMain.Instance);
    }
  }
}
