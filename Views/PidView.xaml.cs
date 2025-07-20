using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace Eagle.Views
{
    public partial class PidView : ContentView
    {
        public ObservableCollection<ObdPidItem> PidList { get; set; } = new();

        public PidView()
        {
            InitializeComponent();
            BindingContext = this;
            LoadStaticPids();
        }

        private void LoadStaticPids()
        {
            string[,] data = new string[,]
            {
                { "01 ", "Monitor status since DTCs cleared", "" },
                { "02 ", "Freeze DTC", "" },
                { "03 ", "Fuel system status", "" },
                { "04 ", "Calculated engine load", "%" },
                { "05 ", "Engine coolant temperature", "°C" },
                { "06 ", "Short term fuel trim—Bank 1", "%" },
                { "07 ", "Long term fuel trim—Bank 1", "%" },
                { "08 ", "Short term fuel trim—Bank 2", "%" },
                { "09 ", "Long term fuel trim—Bank 2", "%" },
                { "0A ", "Fuel pressure", "kPa" },
                { "0B ", "Intake manifold absolute pressure", "kPa" },
                { "0C ", "Engine RPM", "rpm" },
                { "0D ", "Vehicle speed", "km/h" },
                { "0E ", "Timing advance", "°" },
                { "0F ", "Intake air temperature", "°C" },
                { "10 ", "MAF air flow rate", "grams/sec" },
                { "11 ", "Throttle position", "%" },
                { "12 ", "Commanded secondary air status", "" },
                { "13 ", "Oxygen sensors present (in 2 banks)", "" },
                { "14 ", "O2 Sensor 1: Voltage, Short Term Fuel Trim", "V / %" },
                { "15 ", "O2 Sensor 2: Voltage, Short Term Fuel Trim", "V / %" },
                { "16 ", "O2 Sensor 3: Voltage, Short Term Fuel Trim", "V / %" },
                { "17 ", "O2 Sensor 4: Voltage, Short Term Fuel Trim", "V / %" },
                { "18 ", "O2 Sensor 5: Voltage, Short Term Fuel Trim", "V / %" },
                { "19 ", "O2 Sensor 6: Voltage, Short Term Fuel Trim", "V / %" },
                { "1A ", "O2 Sensor 7: Voltage, Short Term Fuel Trim", "V / %" },
                { "1B ", "O2 Sensor 8: Voltage, Short Term Fuel Trim", "V / %" },
                { "1C ", "OBD standards this vehicle conforms to", "" },
                { "1D ", "Oxygen sensors present (in 4 banks)", "" },
                { "1E ", "Auxiliary input status", "" },
                { "1F ", "Run time since engine start", "s" },
                { "20 ", "PIDs supported [21 - 40]", "" },
                { "21 ", "Distance traveled with MIL on", "km" },
                { "22 ", "Fuel Rail Pressure (vacuum ref.)", "kPa" },
                { "23 ", "Fuel Rail Pressure (direct injection)", "kPa" },
                { "24 ", "O2S1_WR lambda–equivalence ratio", "" },
                { "25 ", "O2S2_WR lambda–equivalence ratio", "" },
                { "26 ", "O2S3_WR lambda–equivalence ratio", "" },
                { "27 ", "O2S4_WR lambda–equivalence ratio", "" },
                { "28 ", "O2S5_WR lambda–equivalence ratio", "" },
                { "29 ", "O2S6_WR lambda–equivalence ratio", "" },
                { "2A ", "O2S7_WR lambda–equivalence ratio", "" },
                { "2B ", "O2S8_WR lambda–equivalence ratio", "" },
                { "2C ", "Commanded EGR", "%" },
                { "2D ", "EGR Error", "%" },
                { "2E ", "Commanded evaporative purge", "%" },
                { "2F ", "Fuel Tank Level Input", "%" },
                { "30 ", "Warm-ups since codes cleared", "" },
                { "31 ", "Distance since codes cleared", "km" },
                { "32 ", "Evap. system vapor pressure", "Pa" },
                { "33 ", "Absolute barometric pressure", "kPa" },
                { "34 ", "O2 Sensor 1: equivalence ratio, voltage", "" },
                { "35 ", "O2 Sensor 2: equivalence ratio, voltage", "" },
                { "36 ", "O2 Sensor 3: equivalence ratio, voltage", "" },
                { "37 ", "O2 Sensor 4: equivalence ratio, voltage", "" },
                { "38 ", "O2 Sensor 5: equivalence ratio, voltage", "" },
                { "39 ", "O2 Sensor 6: equivalence ratio, voltage", "" },
                { "3A ", "O2 Sensor 7: equivalence ratio, voltage", "" },
                { "3B ", "O2 Sensor 8: equivalence ratio, voltage", "" },
                { "3C ", "Catalyst Temperature Bank 1, Sensor 1", "°C" },
                { "3D ", "Catalyst Temperature Bank 2, Sensor 1", "°C" },
                { "3E ", "Catalyst Temperature Bank 1, Sensor 2", "°C" },
                { "3F ", "Catalyst Temperature Bank 2, Sensor 2", "°C" },
                { "40 ", "PIDs supported [41 - 60]", "" }
            };

            for (int i = 0; i < data.GetLength(0); i++)
            {
                PidList.Add(new ObdPidItem
                {
                    PID = data[i, 0],
                    Description = data[i, 1],
                    Value = "--",
                    Units = data[i, 2]
                });
            }
        }

        public void UpdatePidValue(string pid, string value)
        {
            var item = PidList.FirstOrDefault(x => x.PID == pid);
            if (item != null)
            {
                item.Value = value;
            }
        }
    }

    public class ObdPidItem
    {
        public string PID { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Units { get; set; }
    }
}
