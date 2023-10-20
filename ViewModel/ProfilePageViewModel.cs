using Microsoft.Maui.Controls;
using app.Models;
using app.Services;
using app.ViewModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using app.View;



namespace app.ViewModel
{

    public class ProfilePageViewModel : BaseViewModel
    {
        public ICommand LogOutCommand { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public ProfilePageViewModel()
        {
            Email = App.UserEmail;
            Username = App.Username;
            LogOutCommand = new Command(Logout);
            Debug.WriteLine($"Retrieved Email from App class: {Email}");
            Debug.WriteLine($"Retrieved Username from App class: {Username}");
        }


        private async void Logout()
        {
            App.UserEmail = null;
            App.Username = null;

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");


        }
    }

}

//test