using System.ComponentModel;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace app.ViewModel
{
    public class RecipeDetailsPageViewModel : BindableObject
    {
        private Recipe _selectedRecipe;

        public Recipe SelectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged(nameof(SelectedRecipe));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Author));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Ingredients));
                OnPropertyChanged(nameof(Method));
            }
        }

        public string Name
        {
            get { return SelectedRecipe?.Name; }
        }

        public string Author
        {
            get { return SelectedRecipe?.Author; }
        }

        public string Description
        {
            get { return SelectedRecipe?.Description; }
        }

        public List<string> Ingredients
        {
            get { return SelectedRecipe?.Ingredients; }
        }

        public List<string> Method
        {
            get { return SelectedRecipe?.Method; }
        }

        public RecipeDetailsPageViewModel()
        {
        }
    }

}