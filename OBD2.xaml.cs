using Eagle.Views;
using Microsoft.Maui.Controls;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Eagle;

public partial class OBD2 : ContentPage
{
    private readonly SerialPort _elmPort;
    private readonly DtcView dtcView = new();
    private readonly PidView pidView = new();
    private readonly VinView vinView = new();
    private readonly MonitorStatusView monitorStatusView = new();
    private readonly string connectedPort;

    public OBD2(SerialPort existingPort)
    {
        InitializeComponent();
        _elmPort = existingPort ?? throw new ArgumentNullException(nameof(existingPort));
        connectedPort = _elmPort.PortName;
        Title = $"Eagle OBD2 connected to {connectedPort}";

        if (!_elmPort.IsOpen)
        {
            throw new InvalidOperationException($"Port {connectedPort} is not open.");
        }

        InitializeELM327();
        ContentContainer.Content = dtcView;
    }

    private void InitializeELM327()
    {
        SendELMCommand("ATZ");
        SendELMCommand("ATE0");
        SendELMCommand("ATL0");
        SendELMCommand("ATS0");
        SendELMCommand("ATH1");
        SendELMCommand("ATSP0");
    }

    private string SendELMCommand(string command)
    {
        if (_elmPort == null || !_elmPort.IsOpen)
            return "ERROR: Port not open.";

        try
        {
            _elmPort.DiscardInBuffer();
            _elmPort.WriteLine(command + "\r");
            Thread.Sleep(200);
            return _elmPort.ReadExisting();
        }
        catch (Exception ex)
        {
            return $"ERROR: {ex.Message}";
        }
    }

    private async void OnReadDTCClicked(object sender, EventArgs e)
    {
        dtcView.ClearDtc();
        var response = SendELMCommand("03");

        if (response.Contains("43"))
        {
            var lines = response.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Where(l => l.StartsWith("43")))
            {
                string hex = line.Replace(" ", "").Substring(2);
                for (int i = 0; i + 4 <= hex.Length; i += 4)
                {
                    string dtc = DecodeDtc(hex.Substring(i, 4));
                    dtcView.AddDtc(dtc, "Unknown (requires lookup)");
                }
            }

            ContentContainer.Content = dtcView;
            await DisplayAlert("Success", "DTCs retrieved from ECU.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "No DTCs found or response error.", "OK");
        }
    }

    private string DecodeDtc(string hex)
    {
        char type = "PCBU"[(Convert.ToInt32(hex.Substring(0, 1), 16) & 0xC0) >> 6];
        int code = Convert.ToInt32(hex.Substring(1), 16);
        return $"{type}{code:X04}";
    }

    private async void OnClearDTCClicked(object sender, EventArgs e)
    {
        bool confirmed = await DisplayAlert("Clear Fault Codes?", "This will erase all DTCs. Proceed?", "Yes", "Cancel");
        if (confirmed)
        {
            var response = SendELMCommand("04");
            dtcView.ClearDtc();
            ContentContainer.Content = dtcView;
            await DisplayAlert("Cleared", "Fault codes cleared successfully.", "OK");
        }
    }

    private void OnCheckMonitorsClicked(object sender, EventArgs e)
    {
        var response = SendELMCommand("0101");
        monitorStatusView.SetRawResponse(response); // Add this method to your MonitorStatusView
        ContentContainer.Content = monitorStatusView;
    }

    private void OnCheckVinClicked(object sender, EventArgs e)
    {
        var response = SendELMCommand("0902");
        string vin = ParseVin(response);
        vinView.SetVin(vin); // Add this method to your VinView
        ContentContainer.Content = vinView;
    }

    private void OnDisplayPidClicked(object sender, EventArgs e)
    {
        ContentContainer.Content = pidView;
    }

    private void OnFreezeFrameClicked(object sender, EventArgs e)
    {
        ContentContainer.Content = new Label
        {
            Text = "Freeze Frame Data will be shown here.",
            FontSize = 18,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    private string ParseVin(string response)
    {
        if (!response.Contains("49 02")) return "Invalid response";
        var hex = string.Concat(response.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(s => s.Length == 2 && int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out _))
            .Skip(4));

        return string.Concat(Enumerable.Range(0, hex.Length / 2)
            .Select(i => (char)Convert.ToByte(hex.Substring(i * 2, 2), 16)));
    }
}
     