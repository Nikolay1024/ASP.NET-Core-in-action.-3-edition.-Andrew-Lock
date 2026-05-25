using Identity2App.Commands;
using Identity2App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity2App.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        readonly RecipeService _recipeService;
        [BindProperty]
        public RecipeCreateCommand Input { get; set; } = new();

        public CreateModel(RecipeService recipeService) => _recipeService = recipeService;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = await _recipeService.CreateRecipe(Input);
                    return RedirectToPage("View", new { id });
                }
            }
            catch (Exception)
            {
                // Добавление ошибки на уровне модели, используя пустой строковый ключ.
                ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении рецепта.");
            }

            return Page();
        }
    }
}
