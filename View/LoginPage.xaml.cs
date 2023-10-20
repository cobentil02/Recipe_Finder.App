using app.ViewModel;

namespace app.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        BindingContext = new LoginPageViewModel();
    }
}