using Identity2App.Commands;
using Identity2App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity2App.Pages.Recipes
{
    public class EditModel : PageModel
    {
        readonly RecipeService _recipeService;
        [BindProperty]
        public RecipeUpdateCommand? Input { get; set; }

        public EditModel(RecipeService recipeService) => _recipeService = recipeService;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Input = await _recipeService.GetRecipeForUpdate(id);
            if (Input is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _recipeService.UpdateRecipe(Input!);
                    return RedirectToPage("View", new { id = Input!.Id });
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
