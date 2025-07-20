namespace Eagle
{
    public partial class App : Application
    {
        public static IServiceProvider Services;
        public App(IServiceProvider services)
        {

            InitializeComponent();
            Services = services;
            MainPage = new NavigationPage(services.GetRequiredService<MainPage>());
        }
    }
}

