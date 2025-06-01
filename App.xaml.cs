namespace QanooniRishta
{
    public partial class App : Application
    {
        public static IServiceProvider Services => MauiProgram.ServiceProvider;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
