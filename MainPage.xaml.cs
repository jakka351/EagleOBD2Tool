using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Eagle
{
    public partial class MainPage : ContentPage
    {
        private readonly IServiceProvider services;
        public MainPage()
        {
            InitializeComponent();
            this.services = services;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Simulate some startup work (like initializing a connection with the ELM327 device, etc.)
            // Adjust the delay as needed. If you are waiting for an actual operation, replace this delay with real logic.
            await Task.Delay(10000);

            // After the initialization, navigate to the main part of your app.
            // This could be a new Page in your Shell or a NavigationPage.
            // For demonstration, we'll just show how you might push a new page:
            // Navigate to the home page
            var homePage = App.Services.GetRequiredService<HomePage>();

            await Navigation.PushAsync(homePage);
            //await Navigation.PushAsync(new HomePage());
        }
    }
}
