namespace app.View;
using app.ViewModel;
public partial class ResetPasswordPage : ContentPage
{
    public ResetPasswordPage()
    {
        InitializeComponent();
        BindingContext = new ResetPasswordViewModel();

    }

}