using app.ViewModel;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using app.Services;
using app.Models;
using System.Text;
using System.Security.Cryptography;
namespace app.View;

public partial class RegistrationPage : ContentPage
{

    public RegistrationPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string emailAddress = emailEntry.Text;
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        // Validate the input
        if (string.IsNullOrEmpty(emailAddress) || !Regex.IsMatch(emailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(username))
        {
            await DisplayAlert("Error", "Please enter a username.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            await DisplayAlert("Error", "Please enter a password that is at least 6 characters long.", "OK");
            return;
        }

        // Check if a user with the same email already exists
        var database = new UserCredentialsDatabase();
        var existingUser = await database.GetUserCredentialsByEmailAsync(emailAddress);
        if (existingUser != null)
        {
            await DisplayAlert("Error", "An account with the same email already exists.", "OK");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            return;
        }

        //check if a user with the same username already exists
        var database2 = new UserCredentialsDatabase();
        var existingUser2 = await database2.GetUserCredentialsAsync(username);
        if (existingUser2 != null)
        {
            await DisplayAlert("Error", " Username already exists.", "OK");
            return;
        }

        // Create a new UserCredentials object with the entered email, username, and password
        var newUserCredentials = new UserCredentials
        {
            Email = emailAddress,
            Username = username,
            Password = HashPassword(password)
        };

        // Insert the new user credentials into the database
        await database.SetUserCredentialsAsync(newUserCredentials);
        
        // Store the user's registration details in the App class
        App.UserEmail = newUserCredentials.Email;
        App.Username = newUserCredentials.Username;

        emailEntry.Text = "";
        usernameEntry.Text = "";
        passwordEntry.Text = "";
        await DisplayAlert("Success", "Registration successful.", "OK");
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

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // navigate back to the login page
    }

}



