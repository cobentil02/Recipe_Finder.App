using app.ViewModel;
namespace app.View;

public partial class RecipeDetailsPage : ContentPage
{
    public RecipeDetailsPage(Recipe selectedRecipe)
    {
        InitializeComponent();
        BindingContext = new RecipeDetailsPageViewModel { SelectedRecipe = selectedRecipe };
    }
}
