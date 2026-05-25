using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesApp.Pages;

// Модель Razor должна наследоваться от PageModel.
public class PrivacyModel : PageModel
{
    readonly ILogger<PrivacyModel> _logger;

    // Использование внедрения зависимостей для предоставления сервисов в конструкторе.
    public PrivacyModel(ILogger<PrivacyModel> logger) => _logger = logger;

    // OnGet() - обработчик страницы по умолчанию. Поскольку метод возвращает void, выполнение обработчика выполнит
    // связанное представление Razor (Privacy.cshtml), чтобы сгенерировать HTML-код.
    public void OnGet() { }
}

