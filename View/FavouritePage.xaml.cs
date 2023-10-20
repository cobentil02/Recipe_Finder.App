using System;
using System.Linq;

namespace app.View
{
    public partial class FavouritePage : ContentPage
    {
        public FavouritePage()
        {
            InitializeComponent();
            Title = "Favorite Recipes";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.Username != null)
            {
                if (App.UserFavoriteRecipes.ContainsKey(App.Username))
                {
                    favoritesListView.ItemsSource = App.UserFavoriteRecipes[App.Username].Where(r => r.IsFavorite).ToList();
                }
                else
                {
                    favoritesListView.ItemsSource = new List<Recipe>();
                }
            }
        }


        private async void RemoveFromFavorites_Clicked(object sender, EventArgs e)
        {
            var recipe = (Recipe)((Button)sender).CommandParameter;

            if (await DisplayAlert("Remove from Favorites", $"Are you sure you want to remove {recipe.Name} from your favorites?", "Yes", "No"))
            {
                recipe.IsFavorite = false;

                if (App.UserFavoriteRecipes.ContainsKey(App.Username))
                {
                    var favoriteRecipes = App.UserFavoriteRecipes[App.Username];
                    if (favoriteRecipes.Contains(recipe))
                    {
                        favoriteRecipes.Remove(recipe);
                    }
                }

                OnAppearing();
            }
        }

        private void FavoritesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}