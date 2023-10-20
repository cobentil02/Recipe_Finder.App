namespace app;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

    }
    public static string UserEmail { get; set; }
    public static string Username { get; set; }


    public static Dictionary<string, List<Recipe>> UserFavoriteRecipes { get; set; } = new Dictionary<string, List<Recipe>>();

}