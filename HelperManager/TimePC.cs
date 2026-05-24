using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HelperManager
{
  public static class TimePC
  {
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetSystemTime(ref SYSTEMTIME st);

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
      public ushort Year;
      public ushort Month;
      public ushort DayOfWeek;
      public ushort Day;
      public ushort Hour;
      public ushort Minute;
      public ushort Second;
      public ushort Milliseconds;
    }

    public static bool SetLocalTime(DateTime localTime)
    {
      // Convert sang UTC vì SetSystemTime chỉ nhận UTC
      DateTime utcTime = localTime.ToUniversalTime();

      SYSTEMTIME st = new SYSTEMTIME
      {
        Year = (ushort)utcTime.Year,
        Month = (ushort)utcTime.Month,
        Day = (ushort)utcTime.Day,
        Hour = (ushort)utcTime.Hour,
        Minute = (ushort)utcTime.Minute,
        Second = (ushort)utcTime.Second,
        Milliseconds = (ushort)utcTime.Millisecond
      };

      bool result = SetSystemTime(ref st);

      if (!result)
      {
        int error = Marshal.GetLastWin32Error();
        Console.WriteLine("SetSystemTime failed. Win32 Error: " + error);
      }

      return result;
    }

  }
}
