using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Eagle.Views
{
    public partial class MonitorStatusView : ContentView
    {
        public MonitorStatusView()
        {
            InitializeComponent();
            LoadMonitors(); // Default load with "Unknown" status
        }

        private void LoadMonitors()
        {
            var monitors = new List<MonitorStatus>
            {
                new MonitorStatus { MonitorName = "Misfire", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Fuel System", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Comprehensive Components", Status = "Unknown", StatusColor = Colors.Gray },

                new MonitorStatus { MonitorName = "Catalyst", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Heated Catalyst", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Evaporative System", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Secondary Air System", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "O2 Sensor", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "O2 Sensor Heater", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "EGR System", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Boost Pressure Control", Status = "Unknown", StatusColor = Colors.Gray },
                new MonitorStatus { MonitorName = "Exhaust Gas Sensor", Status = "Unknown", StatusColor = Colors.Gray }
            };

            MonitorCollectionView.ItemsSource = monitors;
        }

        public void SetRawResponse(string response)
        {
            ResponseLabel.Text = $"Raw: {response}";

            // NOTE: Actual decoding of Mode 01 PID 01 response not yet implemented.
            // Replace the below logic with real parsing from the bitfields.
            if (MonitorCollectionView.ItemsSource is List<MonitorStatus> list)
            {
                foreach (var monitor in list)
                {
                    // Dummy toggle logic: everything after Catalyst is "Ready"
                    bool isReady = monitor.MonitorName switch
                    {
                        "Misfire" or "Fuel System" or "Comprehensive Components" => true,
                        _ => false
                    };

                    monitor.Status = isReady ? "Ready" : "Not Ready";
                    monitor.StatusColor = isReady ? Colors.Green : Colors.Red;
                }

                MonitorCollectionView.ItemsSource = null;
                MonitorCollectionView.ItemsSource = list;
            }
        }

        public class MonitorStatus
        {
            public string MonitorName { get; set; }
            public string Status { get; set; }
            public Color StatusColor { get; set; }
        }
    }
}
