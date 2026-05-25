using Identity2App.Services;
using Identity2App.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity2App.Pages
{
    public class IndexModel : PageModel
    {
        readonly RecipeService _recipeService;
        public List<RecipeSummaryViewModel> Recipes { get; private set; } = new();

        public IndexModel(RecipeService recipeService) => _recipeService = recipeService;

        public async Task OnGetAsync() => Recipes = await _recipeService.GetRecipes();
    }
}
