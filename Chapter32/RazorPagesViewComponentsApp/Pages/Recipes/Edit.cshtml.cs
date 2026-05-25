using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesViewComponentsApp.Commands;
using RazorPagesViewComponentsApp.Data;
using RazorPagesViewComponentsApp.Services;

namespace RazorPagesViewComponentsApp.Pages.Recipes
{
    // Только авторизованные пользователи должны иметь право редактировать рецепты.
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IAuthorizationService _authService;
        private readonly RecipeService _recipeService;
        [BindProperty]
        public RecipeUpdateCommand? Input { get; set; }

        // Сервис IAuthorizationService внедряется в конструктор из контейнера внедрения зависимостей.
        public EditModel(IAuthorizationService authService, RecipeService recipeService)
        {
            _authService = authService;
            _recipeService = recipeService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Вы должны загрузить сущность Recipe из базы данных, прежде чем узнаете, кто ее создал.
            Recipe? recipe = await _recipeService.GetRecipe(id);
            // Вы должны авторизовать текущего пользователя, чтобы убедиться, что ему разрешено редактировать
            // этот конкретный рецепт.
            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            // Метод действия может продолжить работу только в том случае, если пользователь авторизован.
            // Если авторизация прошла неудачно, возвращается результат 403 Forbidden.
            if (!authResult.Succeeded)
                return new ForbidResult();

            Input = await _recipeService.GetRecipeForUpdate(id);
            if (Input is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Recipe? recipe = await _recipeService.GetRecipe(Input!.Id);
                AuthorizationResult authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
                if (!authResult.Succeeded)
                    return new ForbidResult();

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
