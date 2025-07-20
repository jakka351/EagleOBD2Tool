using System.IO.Ports;
using System.Text;
using Eagle.Services; // Make sure this matches your interface location

[assembly: Microsoft.Maui.Controls.Dependency(typeof(Eagle.Platforms.Windows.BluetoothOBDServiceWindows))]

namespace Eagle.Platforms.Windows
{
    public class BluetoothOBDServiceWindows : IBluetoothOBDService
    {
        private SerialPort _serialPort;

        public bool IsConnected => _serialPort != null && _serialPort.IsOpen;

        public void Connect(string portName, int baudRate = 9600)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();

            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 2000,
                WriteTimeout = 2000,
                Encoding = Encoding.ASCII
            };

            _serialPort.Open();
            Thread.Sleep(1500);
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
        }

        public void Disconnect()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();
        }

        public string SendCommand(string command)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
                throw new InvalidOperationException("Port not open.");

            _serialPort.Write(command + "\r");
            Thread.Sleep(300);
            return _serialPort.ReadExisting().Replace(">", "").Trim();
        }
    }
}
