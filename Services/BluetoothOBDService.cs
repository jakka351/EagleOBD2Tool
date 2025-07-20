using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Services
{
    public interface IBluetoothOBDService
    {
        void Connect(string portName, int baudRate = 9600);
        void Disconnect();
        string SendCommand(string command);
        bool IsConnected { get; }
    }
}
