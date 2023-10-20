namespace app.Services;
using app.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class RecipeJson : ContentPage
{
    public static async Task<List<Recipe>> LoadRecipesFromJson()
    {
        var recipes = new List<Recipe>();

        using var stream = await FileSystem.OpenAppPackageFileAsync("recipes.json");
        using var reader = new StreamReader(stream);

        var json = await reader.ReadToEndAsync();

        if (!string.IsNullOrEmpty(json))
        {
            recipes = JsonConvert.DeserializeObject<List<Recipe>>(json);
        }

        return recipes;
    }

}
//Deserialize an Object. (2013). Newtonsoft.com.
//https://www.newtonsoft.com/json/help/html/deserializeobject.htm