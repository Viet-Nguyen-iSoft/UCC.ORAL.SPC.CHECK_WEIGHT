using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HelperManager
{
  public static class TcpHelper
  {
    public static bool IsPing(string ip, int timeoutConnectTcp = 500)
    {
      try
      {
        using (Ping ping = new Ping())
        {
          var reply = ping.Send(ip, timeoutConnectTcp);
          return reply.Status == IPStatus.Success;
        }
      }
      catch
      {
        return false;
      }
    }
  }
}
