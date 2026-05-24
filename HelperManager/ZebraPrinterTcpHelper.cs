using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestConnectPrinter
{
  public class ZebraPrinterTcpHelper
  {
    public static EnumStatusConnect PrintLabelWeight(string ip, int port, string zpl, int timeoutConnectTcp = 500)
    {
      try
      {
        if (!IsPing(ip, timeoutConnectTcp))
        {
          return EnumStatusConnect.Disconnect;
        }

        string rsStr = GetPrinterStatus(ip, port);
        var rsStatus = ParseZebraHS(rsStr);

        if (rsStatus[0][1] == "1")
        {
          return EnumStatusConnect.PaperOut;
        }
        else if (rsStatus[0][2] == "1")
        {
          return EnumStatusConnect.Pause;
        }
        else if (rsStatus[0][10] == "1")
        {
          return EnumStatusConnect.OverTemperature;
        }
        else if (rsStatus[0][11] == "1")
        {
          return EnumStatusConnect.UnderTemperature;
        }

        PrintZebra(ip, zpl);


        return EnumStatusConnect.Sucess;
      }
      catch (Exception ex)
      {
        return EnumStatusConnect.Error;
      }
    }

    public static EnumStatusConnect GetStatusPrint(string ip, int port = 9100, int timeoutConnectTcp = 1000)
    {
      try
      {
        if (!IsPing(ip, timeoutConnectTcp))
        {
          return EnumStatusConnect.Disconnect;
        }

        string rsStr = GetPrinterStatus(ip, port);
        var rsStatus = ParseZebraHS(rsStr);

        if (rsStatus[0][1] == "1")
        {
          return EnumStatusConnect.PaperOut;
        }
        else if (rsStatus[0][2] == "1")
        {
          return EnumStatusConnect.Pause;
        }
        else if (rsStatus[0][10] == "1")
        {
          return EnumStatusConnect.OverTemperature;
        }
        else if (rsStatus[0][11] == "1")
        {
          return EnumStatusConnect.UnderTemperature;
        }

        return EnumStatusConnect.Sucess;
      }
      catch (Exception ex)
      {
        return EnumStatusConnect.Error;
      }
    }

    public static void PrintZebra(string ip, string zpl, int port = 9100)
    {
      using (TcpClient client = new TcpClient())
      {
        client.Connect(ip, port);

        using (NetworkStream stream = client.GetStream())
        {
          byte[] data = Encoding.UTF8.GetBytes(zpl);
          stream.Write(data, 0, data.Length);
          stream.Flush();
        }
      }
    }

    public static string GetPrinterStatus(string ip, int port)
    {
      return SendCommand(ip, "~HS", port);
    }

    public static string SendCommand(string ip, string command, int port = 9100)
    {
      using (TcpClient client = new TcpClient())
      {
        client.ReceiveTimeout = 3000;
        client.SendTimeout = 3000;

        client.Connect(ip, port);

        NetworkStream stream = client.GetStream();

        byte[] data = Encoding.ASCII.GetBytes(command);
        stream.Write(data, 0, data.Length);
        stream.Flush();

        byte[] buffer = new byte[4096];
        int bytes = stream.Read(buffer, 0, buffer.Length);

        return Encoding.ASCII.GetString(buffer, 0, bytes);
      }
    }


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

    public static List<string[]> ParseZebraHS(string raw)
    {
      var result = new List<string[]>();

      if (string.IsNullOrWhiteSpace(raw))
        return result;

      // tách từng block
      var blocks = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);

      foreach (var block in blocks)
      {
        string line = block
            .Replace("\u0002", "")
            .Replace("\u0003", "")
            .Replace("\r", "")
            .Trim();

        if (!string.IsNullOrEmpty(line))
        {
          var fields = line.Split(',');
          result.Add(fields);
        }
      }

      return result;
    }
  }

  public enum EnumStatusConnect
  {
    None,
    Disconnect,
    Error,
    PaperOut,
    Pause,
    OverTemperature,
    UnderTemperature,
    Sucess,

  }

}
