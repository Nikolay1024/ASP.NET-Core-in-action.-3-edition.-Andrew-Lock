using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesViewComponentsApp.Commands;
using RazorPagesViewComponentsApp.Data;
using RazorPagesViewComponentsApp.Services;

namespace RazorPagesViewComponentsApp.Pages.Recipes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userService;
        private readonly RecipeService _recipeService;
        [BindProperty]
        public RecipeCreateCommand Input { get; set; } = new();

        public CreateModel(UserManager<AppUser> userService, RecipeService recipeService)
        {
            _userService = userService;
            _recipeService = recipeService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppUser appUser = (await _userService.GetUserAsync(User))!;
                    int id = await _recipeService.CreateRecipe(Input, appUser);
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
