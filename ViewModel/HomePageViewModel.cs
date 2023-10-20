using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using app.Models;
using app.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using app.View;
using app.ViewModel;
using System.Diagnostics;

namespace app.ViewModel
{
    public class HomePageViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        private List<Recipe> _recipes;
        public List<Recipe> Recipes
        {
            get => _recipes;
            set
            {
                _recipes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Recipes)));
                UpdateFilteredRecipes();
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchQuery)));
                UpdateFilteredRecipes();
            }
        }

        private List<Recipe> _filteredRecipes;
        public List<Recipe> FilteredRecipes
        {
            get => _filteredRecipes;
            set
            {
                _filteredRecipes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilteredRecipes)));
            }
        }


        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set
            {
                _userEmail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserEmail)));
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        private async Task LoadRecipesAsync()
        {
            Recipes = await RecipeJson.LoadRecipesFromJson();
            FilteredRecipes = Recipes;

        }

        private void UpdateFilteredRecipes()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredRecipes = Recipes;
                return;
            }

            FilteredRecipes = Recipes.Where(r => r.Name.ToLower().Contains(SearchQuery.ToLower())).ToList();

            if (FilteredRecipes.Count == 0)
            {
                // Show message when no matching recipes are found
                App.Current.MainPage.DisplayAlert("No Results", "Sorry we don't have this recipe, yet.", "OK");
            }
        }

        public ICommand DetailsCommand { get; }

        public HomePageViewModel()
        {
            LoadRecipesAsync().ConfigureAwait(true);

        }

        public ICommand NavigateToProfileCommand => new Command(GoToProfile);

        public async void GoToProfile()
        {
            Debug.WriteLine($"Navigating to ProfilePage with Email={App.UserEmail}, Username={App.Username}");
            await Shell.Current.GoToAsync($"{nameof(ProfilePage)}?email={App.UserEmail}&username={App.Username}");
        }



    }
}

