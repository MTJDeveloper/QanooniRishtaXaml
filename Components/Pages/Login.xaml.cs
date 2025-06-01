using Microsoft.Maui.Controls;

namespace QanooniRishta.Components.Pages
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

            LoginButton.Clicked += OnLoginButtonClicked;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Clear previous error
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;

            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            // Basic validation
            if (string.IsNullOrWhiteSpace(email))
            {
                ErrorLabel.Text = "Username is required.";
                ErrorLabel.IsVisible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorLabel.Text = "Password is required.";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Dummy credentials check
            const string dummyUsername = "Admin";
            const string dummyPassword = "Munsif@1100";

            if (email == dummyUsername && password == dummyPassword)
            {
                await Navigation.PushAsync(new Home());
            }
            else
            {
                ErrorLabel.Text = "Invalid login. Please try again.";
                ErrorLabel.IsVisible = true;
            }
        }
    }
}
