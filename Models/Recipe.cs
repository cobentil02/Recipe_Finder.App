using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public List<string> Ingredients { get; set; }
    public List<string> Method { get; set; }
    public bool IsFavorite { get; set; }

  
}
