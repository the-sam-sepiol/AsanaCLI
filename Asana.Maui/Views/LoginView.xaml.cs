using Asana.Library.Models;
using Asana.Maui.Services;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginViewModel ViewModel { get; set; }

        public LoginView()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
            BindingContext = ViewModel;
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            try
            {
                if (ViewModel.SelectedUser != null)
                {
                    CurrentUserService.Current.Login(ViewModel.SelectedUser);
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await DisplayAlert("Login Error", "Please select a user first.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
        }
    }
}
