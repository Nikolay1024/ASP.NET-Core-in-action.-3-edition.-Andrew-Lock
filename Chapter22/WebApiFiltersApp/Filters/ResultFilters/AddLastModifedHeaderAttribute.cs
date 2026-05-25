using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiFiltersApp.ViewModels;

namespace WebApiFiltersApp.Filters.ResultFilters
{
    // "Фильтр результата" выполняется дважды (*Executing, *Executed) в конвейере фильтров.
    // Абстрактный класс ResultFilterAttribute реализует интерфейс IResultFilter и его асинхронный аналог.
    public class AddLastModifedHeaderAttribute : ResultFilterAttribute
    {
        // Фильтр результата *Executing выполняется после фильтра действия *Executed, но перед выполнением результата.
        // Выполнение результата представляет либо отрисовка страницы Razor в MVC и Razor Pages,
        // либо форматирование результата в Web API.
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Проверяет, вернул ли результат действия результат 200 Ok с моделью представления.
            if (context.Result is OkObjectResult result && result.Value is RecipeViewModel recipe)
                // Метод GetTypedHeaders() обеспечивает строго типизированный доступ к заголовкам запросов и ответов.
                // Задает заголовок Last-Modified в ответе.
                context.HttpContext.Response.GetTypedHeaders().LastModified = recipe.LastModified;
        }

        // Фильтр результата *Executed выполняется после выполнения результата, но перед фильтром ресурсов *Executed.
        // В данном примере определение не требуется, т.к. наследуется базовая реализация ResultFilterAttribute.
        //public override void OnResultExecuted(ResultExecutedContext context) { }
    }
}
