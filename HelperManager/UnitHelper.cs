using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace HelperManager
{
  public static class UnitHelper
  {
    //public static double PxToMm(int px)
    //{
    //  return px / dpi * 25.4;
    //}

    public static int MmToPixels(double mm, eTypeSize typeSize)
    {
      float dpi = 0;
      if (typeSize == eTypeSize.Width)
      {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        {
          dpi = g.DpiX;
        }
      }
      else
      {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        {
          dpi = g.DpiY;
        }
      }  
      return (int)(mm / 25.4 * dpi);
    }
  }

  public enum eTypeSize
  {
    Width, 
    Height
  }
}
