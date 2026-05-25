using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiFiltersApp.Services;

namespace WebApiFiltersApp.Filters.ActionFilters
{
    // "Фильтр действия" выполняется дважды (*Executing, *Executed) в конвейере фильтров MVC (не Razor Pages).
    // Абстрактный класс ActionFilterAttribute реализует интерфейсы IActionFilter и IResultFilter, а также их асинхронные
    // аналоги, поэтому вы можете переопределить нужные вам методы по мере необходимости.
    public class EnsureRecipeExistsAttribute : ActionFilterAttribute
    {
        // Фильтр действия *Executing выполняется после привязки и валидации модели, но перед методом действия.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Получает экземпляр RecipeService из контейнера внедрения зависимостей с помощью антипаттерна
            // "локатор службы". Предпочтительней использовать внедрение зависимостей как в EnsureRecipeExists2Attribute.
            RecipeService recipeService = context.HttpContext.RequestServices.GetService<RecipeService>()!;

            // Извлекает параметр id, который будет передан методу действия при его выполнении.
            int recipeId = (int)context.ActionArguments["id"]!;

            // Если сущности нет, прерывает выполнение конвейера фильтров, задав свойство Result.
            // Прерывание на данном этапе приводит к выполнению фильтров действия *Executed. После чего следует обход
            // метода действия. Остальная часть конвейера фильтров продолжит выполнение.
            if (!recipeService.DoesRecipeExist(recipeId).Result)
                context.Result = new NotFoundResult();
        }

        // Фильтр действия *Executed выполняется после метода действия, но перед фильтром результата *Executing.
        // В данном примере определение не требуется, т.к. наследуется базовая реализация ActionFilterAttribute.
        //public override void OnActionExecuted(ActionExecutedContext context) { }
    }
}
