using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiFiltersApp.Filters.ResourceFilters
{
    // "Фильтр ресурса" выполняется дважды (*Executing, *Executed) в конвейере фильтров.
    // Данный фильтр является "переключателем функциональности", который можно использовать, чтобы отключить доступность
    // всего API на основе свойства IsEnabled.
    public class FeatureEnabledAttribute : Attribute, IResourceFilter
    {
        // Определяет, активирована ли функциональность.
        public bool IsEnabled { get; set; }

        // Фильтр ресурса *Executing выполняется после фильтра авторизации, но перед привязкой и валидацией модели.
        // Если функциональность не активирована, прерывает выполнение конвейера фильтров, задав свойство Result.
        // Прерывание на данном этапе приводит к выполнению фильтров ресурсов *Executed. После чего следует возврат
        // клиенту ответа 400 Bad Request.
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!IsEnabled)
                context.Result = new BadRequestResult();
        }

        // Фильтр ресурса *Executed выполняется после фильтра результата *Executed, но перед отправкой ответа клиенту.
        // Должен быть реализован для удовлетворения IResourceFilter, но в данном случае он не требуется.
        public void OnResourceExecuted(ResourceExecutedContext context) { }
    }
}
