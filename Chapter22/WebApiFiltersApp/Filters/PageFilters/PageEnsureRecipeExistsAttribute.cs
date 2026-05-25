using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiFiltersApp.Services;

namespace WebApiFiltersApp.Filters.PageFilters
{
    // "Фильтр страницы" выполняется трижды (*Selected, *Executing, *Executed) в конвейере фильтров Razor Pages (не MVC).
    // Фильтр наследуется от Attribute, чтобы можно было декорировать PageModel страницы Razor Pages.
    public class PageEnsureRecipeExistsAttribute : Attribute, IPageFilter
    {
        // Фильтр страницы *Selected выполняется после выбора обработчика страницы, но перед привязкой модели.
        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }

        // Фильтр страницы *Executing выполняется после привязки и валидации модели, но перед выполнением
        // обработчика страницы.
        // В данном примере выполняет роль аналогичную EnsureRecipeExistsAttribute, но не используется в проекте.
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            RecipeService recipeService = context.HttpContext.RequestServices.GetService<RecipeService>()!;
            int recipeId = (int)context.HandlerArguments["id"]!;
            if (!recipeService.DoesRecipeExist(recipeId).Result)
                context.Result = new NotFoundResult();
        }

        // Фильтр страницы *Executed выполняется после обработчика страницы, но перед фильтром результата *Executing.
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
