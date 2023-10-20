using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using app.Models;
using app.Services;
using app.View;
using System.Diagnostics;


namespace app.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        public readonly UserCredentialsDatabase _userCredentialsDb;

        public ICommand GoHomeCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand SignupCommand { get; set; }
        public ICommand ShowPasswordCommand { get; set; }
        public ICommand Logwithoutaccount { get; set; }
        public ICommand ResetPass { get; }

        public bool PasswordEntryIsPassword => !IsPasswordVisible;

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private bool _isPasswordVisible = true;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                onPropertyChanged(nameof(IsPasswordVisible));
                onPropertyChanged(nameof(PasswordEntryIsPassword));
            }
        }

        public string ErrorMessage { get; set; }
        public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

        public LoginPageViewModel()
        {
            _userCredentialsDb = new UserCredentialsDatabase();

            GoHomeCommand = new Command(GoHome);
            LoginCommand = new Command(async () => await LoginAsync());
            SignupCommand = new Command(GoRegister);
            ShowPasswordCommand = new Command(ShowPasswordClicked);
            Logwithoutaccount = new Command(enter);
            ResetPass = new Command(navToReset);



        }



        private async Task<bool> ValidateInputAsync()
        {
            if (string.IsNullOrEmpty(Username))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a username.", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a password that is at least 6 characters long.", "OK");
                return false;
            }

            return true;
        }

        //(Chand, 2020)
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = Convert.ToBase64String(hashedBytes);
            return hash;
        }

        public async Task<bool> LoginAsync()
        {
            if (!await ValidateInputAsync())
            {
                return false;
            }

            var storedCredentials = await _userCredentialsDb.GetUserCredentialsAsync(Username);
            if (storedCredentials == null)
            {
                ErrorMessage = "Account does not exist.";
                onPropertyChanged(nameof(ErrorMessage));
                onPropertyChanged(nameof(IsErrorVisible));
                return false;
            }

            var hashedPassword = HashPassword(Password);
            if (storedCredentials.Password != hashedPassword)
            {
                ErrorMessage = "Incorrect password";
                onPropertyChanged(nameof(ErrorMessage));
                onPropertyChanged(nameof(IsErrorVisible));
                return false;
            }

            // Store user email and username in App class
            App.UserEmail = storedCredentials.Email;
            App.Username = storedCredentials.Username;
            // Navigate to the home page
            Username = "";
            Password = "";
            ErrorMessage = "";
            onPropertyChanged(nameof(ErrorMessage));
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            return true;

        }





        private async void GoHome()
        {

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");

        }




        private async void GoRegister()
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        private void ShowPasswordClicked()
        {
            IsPasswordVisible = !IsPasswordVisible;

        }

        private async void enter()
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }


        private async void navToReset()
        {
            await Shell.Current.GoToAsync($"{nameof(ResetPasswordPage)}");
        }
    }
}
