using IdentityAuthorization2App.Data;
using IdentityAuthorization2App.Services;
using IdentityAuthorization2App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAuthorization2App.Pages.Recipes
{
    public class ViewModel : PageModel
    {
        private readonly IAuthorizationService _authService;
        private readonly RecipeService _recipeService;
        public RecipeViewModel? Recipe { get; set; }
        // Свойство CanEditRecipe будет использоваться для управления отрисовкой кнопок "Изменить" и "Удалить".
        public bool CanEditRecipe { get; set; }

        public ViewModel(IAuthorizationService authService, RecipeService recipeService)
        {
            _authService = authService;
            _recipeService = recipeService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Recipe = await _recipeService.GetRecipeById(id);
            if (Recipe is null)
                return NotFound();

            // Загружает ресурс Recipe для использования с IAuthorizationService.
            Recipe? recipe = await _recipeService.GetRecipe(id);
            // Проверяет, имеет ли пользователь право редактировать рецепт.
            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            // Задает свойство CanEditRecipe модели страницы соответствующим образом.
            CanEditRecipe = authResult.Succeeded;

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Recipe? recipe = await _recipeService.GetRecipe(id);
            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            if (!authResult.Succeeded)
                return new ForbidResult();

            await _recipeService.DeleteRecipe(id);

            return RedirectToPage("/Index");
        }
    }
}
