using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using System.Diagnostics;

namespace MvcApp.Controllers
{
    // Контроллер MVC часто наследуют от базового класса Controller.
    // Контроллер MVC – это класс, который содержит один или несколько логически сгруппированных методов действий.
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) => _logger = logger;

        // Метод действия (action) – это конечная точка, которая запускается в ответ на запрос.
        // Возврат View() отображает представление Razor.
        public IActionResult Index() => View();
        public IActionResult Privacy() => View();

        // Вы можете применять фильтры к методам действий.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Любой объект, возвращаемый с помощью View, передается в представление Razor как модель представления.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
