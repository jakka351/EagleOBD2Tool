using System;
using System.IO.Ports;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Eagle.Services;

namespace Eagle
{
    public partial class HomePage : ContentPage
    {
        private readonly IBluetoothOBDService obdService;
        public ObservableCollection<string> ComPorts { get; set; } = new();
        private string selectedPort;
        private SerialPort _serialPort;

        public HomePage(IBluetoothOBDService obdService)
        {
            InitializeComponent();
            this.obdService = obdService ?? throw new ArgumentNullException(nameof(obdService));

            BindingContext = this;
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
#if WINDOWS
            try
            {
                ActivityIndicator.IsRunning = true;
                ComPorts.Clear();
                StatusLabel.Text = "Scanning COM ports...";

                await Task.Run(() =>
                {
                    var ports = SerialPort.GetPortNames();
                    Array.Sort(ports);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var port in ports)
                            ComPorts.Add(port);

                        StatusLabel.Text = ports.Length > 0
                            ? $"Found {ports.Length} COM port(s)."
                            : "No COM ports found.";
                    });
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                StatusLabel.Text = "Error scanning ports.";
            }
            finally
            {
                ActivityIndicator.IsRunning = false;
            }
#else
            await DisplayAlert("Not Supported", "Scanning only works on Windows.", "OK");
#endif
        }

        private void DevicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DevicePicker.SelectedItem is string port)
            {
                selectedPort = port;
                ConnectButton.IsEnabled = true;
                StatusLabel.Text = $"Selected {port}.";
            }
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
#if WINDOWS
            if (string.IsNullOrEmpty(selectedPort))
            {
                await DisplayAlert("No Selection", "Please select a COM port.", "OK");
                return;
            }

            ActivityIndicator.IsRunning = true;
            StatusLabel.Text = $"Connecting to {selectedPort}...";

            try
            {
                // Create and open the SerialPort manually
                _serialPort = new SerialPort(selectedPort, 38400);
                _serialPort.NewLine = "\r";
                _serialPort.Open();

                // Optionally test communication manually
                _serialPort.DiscardInBuffer();
                _serialPort.WriteLine("ATI\r");
                Thread.Sleep(200);
                string response = _serialPort.ReadExisting();

                StatusLabel.Text = $"Connected to {selectedPort}.";
                await DisplayAlert("Connected", $"ELM327 Response: {response}", "OK");

                // Navigate to OBD2.xaml and pass the open port
                await Navigation.PushAsync(new OBD2(_serialPort));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Connection Error", ex.Message, "OK");
                StatusLabel.Text = "Connection failed.";
            }
            finally
            {
                ActivityIndicator.IsRunning = false;
            }
#else
            await DisplayAlert("Not Supported", "Bluetooth connection is only supported on Windows in this version.", "OK");
#endif
        }
    }
}
