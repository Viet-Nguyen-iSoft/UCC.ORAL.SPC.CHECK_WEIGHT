using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static CheckWeigherFood.Modbus.ModbusData;

namespace CheckWeigherFood.Modbus
{
  public class ModbusDataEventArgs : EventArgs
  {
    public ushort[] Registers { get; set; }
      = Array.Empty<ushort>();
  }

  public class ModbusTcpService : IDisposable
  {
    private readonly string _ip;
    private readonly int _port;
    private readonly byte _slaveId;

    private TcpClient _tcpClient;
    private IModbusMaster _master;

    private Timer _timer;

    private bool _isRunning;
    private bool _isReading;
    private bool _disposed;

    public bool IsConnected =>
        _tcpClient != null &&
        _tcpClient.Connected;

    public event EventHandler<bool> ConnectionChanged;
    public event EventHandler<ModbusDataEventArgs> DataReceived;
    public event EventHandler<Exception> Error;

    public ModbusTcpService(
        string ip,
        int port = 502,
        byte slaveId = 1)
    {
      _ip = ip;
      _port = port;
      _slaveId = slaveId;
    }

    public void Start(int periodMs = 1000)
    {
      if (_isRunning)
        return;

      _isRunning = true;

      _timer = new Timer(
          async _ => await TimerCallback(),
          null,
          0,
          periodMs);
    }

    public void Stop()
    {
      _isRunning = false;

      _timer?.Dispose();
      _timer = null;

      Disconnect();
    }

    private async Task TimerCallback()
    {
      if (_isReading)
        return;

      _isReading = true;

      try
      {
        if (!IsConnected)
        {
          await ConnectAsync();
        }

        if (!IsConnected)
          return;

        // ===== READ HOLDING REGISTER =====

        ushort startAddress = 0;
        ushort length = 10;

        ushort[] data =
            _master.ReadHoldingRegisters(
                _slaveId,
                startAddress,
                length);

        DataReceived?.Invoke(
            this,
            new ModbusDataEventArgs()
            {
              Registers = data
            });
      }
      catch (Exception ex)
      {
        Error?.Invoke(this, ex);

        Disconnect();
      }
      finally
      {
        _isReading = false;
      }
    }

    public async Task<bool> ConnectAsync()
    {
      try
      {
        Disconnect();

        _tcpClient = new TcpClient();

        await _tcpClient.ConnectAsync(
            _ip,
            _port);

        _tcpClient.ReceiveTimeout = 3000;
        _tcpClient.SendTimeout = 3000;

        _master = ModbusIpMaster.CreateIp(_tcpClient);

        ConnectionChanged?.Invoke(this, true);

        return true;
      }
      catch (Exception ex)
      {
        Error?.Invoke(this, ex);

        Disconnect();

        return false;
      }
    }

    public void Disconnect()
    {
      try
      {
        _master?.Dispose();
      }
      catch
      {

      }

      try
      {
        _tcpClient?.Close();
      }
      catch
      {

      }

      try
      {
        _tcpClient?.Dispose();
      }
      catch
      {

      }

      _master = null;
      _tcpClient = null;

      ConnectionChanged?.Invoke(this, false);
    }

    public ushort[] ReadHoldingRegisters(
        ushort startAddress,
        ushort length)
    {
      if (!IsConnected)
        throw new Exception("Modbus disconnected");

      return _master.ReadHoldingRegisters(
          _slaveId,
          startAddress,
          length);
    }

    public bool[] ReadCoils(
        ushort startAddress,
        ushort length)
    {
      if (!IsConnected)
        throw new Exception("Modbus disconnected");

      return _master.ReadCoils(
          _slaveId,
          startAddress,
          length);
    }

    public void WriteSingleRegister(
        ushort address,
        ushort value)
    {
      if (!IsConnected)
        throw new Exception("Modbus disconnected");

      _master.WriteSingleRegister(
          _slaveId,
          address,
          value);
    }

    public void WriteSingleCoil(
        ushort address,
        bool value)
    {
      if (!IsConnected)
        throw new Exception("Modbus disconnected");

      _master.WriteSingleCoil(
          _slaveId,
          address,
          value);
    }

    public static float ConvertToFloat(
        ushort highRegister,
        ushort lowRegister)
    {
      byte[] bytes = new byte[4];

      bytes[0] = (byte)(highRegister >> 8);
      bytes[1] = (byte)highRegister;

      bytes[2] = (byte)(lowRegister >> 8);
      bytes[3] = (byte)lowRegister;

      Array.Reverse(bytes);

      return BitConverter.ToSingle(bytes, 0);
    }

    public void Dispose()
    {
      if (_disposed)
        return;

      _disposed = true;

      Stop();
    }
  }
}

