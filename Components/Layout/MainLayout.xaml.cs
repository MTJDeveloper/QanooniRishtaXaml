
using QanooniRishta.Components.Pages;

namespace QanooniRishta.Components.Layout
{
    public partial class MainLayout : ContentView
    {
        public MainLayout()
        {
            InitializeComponent();
        }

        public View PageBody
        {
            get => PageContent.Content;
            set => PageContent.Content = value;
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}
