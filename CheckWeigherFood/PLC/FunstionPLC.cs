using IoTClient;
using IoTClient.Clients.PLC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.PLC
{
  public class FunstionPLC
  {
    public void SendDataPLC(MitsubishiClient client, uint resgisterStart, uint lengthResgister, string data)
    {
      if (client == null) return;
      byte[] b_format_data = new byte[(uint)lengthResgister * 2];
      for (int i = 0; i < b_format_data.Length; i++)
      {
        b_format_data[i] = (byte)' ';
      }

      byte[] b_data = Encoding.UTF8.GetBytes(data);

      for (int i = 0; i < b_data.Length; i++)
      {
        b_format_data[i] = (byte)b_data[i];
      }
      client.Write($"D{resgisterStart}", b_format_data);
    }

    public void SendDataPLC(MitsubishiClient client, uint resgisterStart, ulong value)
    {
      client.Write($"D{resgisterStart}", value);
    }
    public void SendDataPLC(MitsubishiClient client, uint resgisterStart, int value)
    {
      client.Write($"D{resgisterStart}", value);
    }
    public void SendDataPLC(MitsubishiClient client, uint resgisterStart, byte[] value)
    {
      client.Write($"D{resgisterStart}", value);
    }

    public short ReadDataPLC(MitsubishiClient client, uint resgisterStart)
    {
      return client.ReadInt16($"D{resgisterStart}").Value;//ReadInt16
    }

    public List<KeyValuePair<string, short>> ReadDataPLC(MitsubishiClient client, uint resgisterStart, ushort length)
    {
      return client.ReadInt16($"D{resgisterStart}", length).Value;
    }
    //public byte[] ReadDataPLC(MitsubishiClient client, uint resgisterStar, ushort length)
    //{
    //  return client.Read($"D{resgisterStar}", length).Value;
    //}


  }
}
