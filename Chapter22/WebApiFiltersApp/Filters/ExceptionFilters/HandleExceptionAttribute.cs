using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiFiltersApp.Filters.ExceptionFilters
{
    // "Фильтр исключений" перехватывает исключения, возникающие на этапе привязки модели, валидации модели,
    // фильтров действия, метода действия.
    // Абстрактный класс ExceptionFilterAttribute реализует интерфейс IExceptionFilter и его асинхронный аналог.
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var problemDetails = new ProblemDetails()
            {
                Title = "An error occurred",
                Detail = context.Exception.Message,
                Status = 500,
                Type = " https://httpwg.org/specs/rfc9110.html#status.500",
            };

            // Установка свойств Result и ExceptionHandled прерывает выполнение конвейера фильтров.
            // ExceptionHandled помечает исключение как обработанное, чтобы предотвратить его распространение
            // в конвейер промежуточного ПО.
            context.Result = new ObjectResult(problemDetails) { StatusCode = 500, };
            context.ExceptionHandled = true;
        }
    }
}
