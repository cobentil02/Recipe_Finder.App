using app.ViewModel;
using app.Models;
using Newtonsoft.Json;

namespace app.View;

    public partial class HomePage : ContentPage
    {

   
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomePageViewModel();
       
    }

    private async void OnRecipeSelectedAsync(object sender, ItemTappedEventArgs e)
    {
        // Deselect item
        ((ListView)sender).SelectedItem = null;

        // Navigate to RecipeDetailsPage with selected recipe as parameter
        Recipe selectedRecipe = (Recipe)e.Item;
        await Navigation.PushAsync(new RecipeDetailsPage(selectedRecipe));
    }

    private void AddToFavorites_Clicked(object sender, EventArgs e)
    {
        var recipe = (Recipe)((Button)sender).CommandParameter;

        if (App.Username == null)
        {
            // Show alert if user is not logged in
            DisplayAlert("Login Required", "You must be logged in to add recipes to favorites.", "OK");
            return;
        }

        if (!App.UserFavoriteRecipes.ContainsKey(App.Username))
        {
            App.UserFavoriteRecipes[App.Username] = new List<Recipe>();
        }

        if (!App.UserFavoriteRecipes[App.Username].Contains(recipe))
        {
            recipe.IsFavorite = true;
            App.UserFavoriteRecipes[App.Username].Add(recipe);

            // Update the IsFavorite property of the recipe
            recipe.IsFavorite = true;

            // Show alert if recipe is added to favorites for the first time
            DisplayAlert("Recipe Added", $"{recipe.Name} has been added to your favorites.", "OK");
        }
        else
        {
            // Show alert if recipe is already in favorites
            DisplayAlert("Already in Favorites", $"{recipe.Name}", "OK");
        }
    }
}