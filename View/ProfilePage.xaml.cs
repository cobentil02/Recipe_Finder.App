using Microsoft.Maui.Controls;
using app.ViewModel;

namespace app.View
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfilePageViewModel();
        }
    }
}