using app.Models;
using app.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using app.View;

namespace app.ViewModel
{

    public class ResetPasswordViewModel : BaseViewModel
    {
        private readonly UserCredentialsDatabase _userCredentialsDb;
        private string _username;

        public bool PasswordEntryIsPassword => !IsPasswordon;

        public ICommand ResetPw { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                onPropertyChanged(nameof(Username));
            }
        }

        private string _newPassword;

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                onPropertyChanged(nameof(NewPassword));
            }
        }

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                onPropertyChanged(nameof(ConfirmPassword));
            }
        }

        private bool _isPasswordon = true;

        public bool IsPasswordon
        {
            get => _isPasswordon;
            set
            {
                _isPasswordon = value;
                onPropertyChanged(nameof(IsPasswordon));
                onPropertyChanged(nameof(PasswordEntryIsPassword));
            }
        }

        public ResetPasswordViewModel()
        {
            ResetPw = new Command(async () => await ResetPasswordAsync());
            _userCredentialsDb = new UserCredentialsDatabase();
        }

        private async Task ResetPasswordAsync()
        {
            if (string.IsNullOrEmpty(Username))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a username or email.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(NewPassword) || NewPassword.Length < 6)
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a new password that is at least 6 characters long.", "OK");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var storedCredentials = await _userCredentialsDb.GetUserCredentialsAsync(Username);
            if (storedCredentials == null)
            {
                // Account does not exist
                await Shell.Current.DisplayAlert("Error", "Account does not exist.", "OK");
                return;
            }

            // Hash the new password using SHA256
            string hashedPassword = HashPassword(NewPassword);

            // Update the password in the database
            storedCredentials.Password = hashedPassword;
            await _userCredentialsDb.SetUserCredentialsAsync(storedCredentials);

            await Shell.Current.DisplayAlert("Success", "Password reset successfully.", "OK");

            // Store the user's details in the App class
            App.Username = storedCredentials.Username;
            App.UserEmail = storedCredentials.Email;
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");


        }

        // Hash the password using SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}