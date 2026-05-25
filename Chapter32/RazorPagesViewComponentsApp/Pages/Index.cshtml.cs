using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesViewComponentsApp.Services;
using RazorPagesViewComponentsApp.ViewModels;

namespace RazorPagesViewComponentsApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RecipeService _recipeService;
        public List<RecipeSummaryViewModel> Recipes { get; private set; } = new();

        public IndexModel(RecipeService recipeService) => _recipeService = recipeService;

        public async Task OnGetAsync() => Recipes = await _recipeService.GetRecipes();
    }
}
