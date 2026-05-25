using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiFiltersApp.Services;

namespace WebApiFiltersApp.Filters.ActionFilters
{
    public class EnsureRecipeExistsFilter : IActionFilter
    {
        readonly RecipeService _recipeService;

        // Сервис RecipeService внедряется в конструктор фильтра EnsureRecipeExistsFilter.
        public EnsureRecipeExistsFilter(RecipeService recipeService) => _recipeService = recipeService;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int recipeId = (int)context.ActionArguments["id"]!;
            if (!_recipeService.DoesRecipeExist(recipeId).Result)
                context.Result = new NotFoundResult();
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }

    // Наследует от TypeFilterAttribute, который используется для заполнения зависимостей с помощью
    // контейнера внедрения зависимостей.
    public class EnsureRecipeExists2Attribute : TypeFilterAttribute
    {
        // Передает тип EnsureRecipeExistsFilter в качестве аргумента базовому конструктору TypeFilterAttribute.
        public EnsureRecipeExists2Attribute() : base(typeof(EnsureRecipeExistsFilter)) { }
    }
}
