using Microsoft.Maui.Controls;

namespace Eagle.Views
{
    public partial class VinView : ContentView
    {
        public VinView()
        {
            InitializeComponent();
        }
        public void SetVin(string vin)
        {
            VinLabel.Text = $"VIN: {vin}";
        }

        private void OnReadVehicleDataClicked(object sender, EventArgs e)
        {
           
        }
    }
}
