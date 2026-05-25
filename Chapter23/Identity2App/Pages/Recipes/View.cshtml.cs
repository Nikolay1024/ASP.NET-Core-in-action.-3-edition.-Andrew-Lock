using Identity2App.Services;
using Identity2App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity2App.Pages.Recipes
{
    public class ViewModel : PageModel
    {
        readonly RecipeService _recipeService;
        public RecipeViewModel? Recipe { get; set; }

        public ViewModel(RecipeService recipeService) => _recipeService = recipeService;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Recipe = await _recipeService.GetRecipeById(id);
            if (Recipe is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _recipeService.DeleteRecipe(id);

            return RedirectToPage("/Index");
        }
    }
}
