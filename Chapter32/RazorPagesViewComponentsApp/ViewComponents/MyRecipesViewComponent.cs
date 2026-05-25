using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorPagesViewComponentsApp.Data;
using RazorPagesViewComponentsApp.Services;
using RazorPagesViewComponentsApp.ViewModels;

namespace RazorPagesViewComponentsApp.ViewComponents
{
    // Наследование от базового класса ViewComponent предоставляет полезные методы, такие как View().
    // Имя компонента представления происходит от имени класса. В качестве альтернативы можно применить
    // к классу атрибут [ViewComponent] и задать другое имя.
    // Необходимо зарегистрировать компонент представления в _ViewImports.cshtml.
    public class MyRecipesViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RecipeService _recipeService;

        // Вы можете использовать внедрение зависимостей в компоненте представления.
        public MyRecipesViewComponent(UserManager<AppUser> userManager, RecipeService recipeService)
        {
            _userManager = userManager;
            _recipeService = recipeService;
        }

        // InvokeAsync отображает компонент представления.
        // Вы можете передавать параметры компоненту из представления (number-of-recipes).
        public async Task<IViewComponentResult> InvokeAsync(int numberOfRecipes)
        {
            if (User.Identity?.IsAuthenticated != true)
                // Вызов метода View() отобразит частичное представление с именем Unauthenticated.cshtml.
                return View("Unauthenticated");

            // Вы можете использовать внешние асинхронные сервисы, что позволяет инкапсулировать логику
            // в вашей предметной области.
            string createdBy = _userManager.GetUserId(HttpContext.User)!;
            List<RecipeSummaryViewModel> recipes =
                await _recipeService.GetRecipesByUser(createdBy, numberOfRecipes);

            // Вы можете передать модель представления частичному представлению.
            // Частичное представление Default.cshtml используется по умолчанию.
            return View(recipes);
        }
    }
}
