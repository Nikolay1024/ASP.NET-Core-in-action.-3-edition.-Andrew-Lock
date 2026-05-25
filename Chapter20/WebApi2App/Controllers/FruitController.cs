using Microsoft.AspNetCore.Mvc;
using WebApi2App.Services;

namespace WebApi2App.Controllers
{
    // Атрибут [ApiController] изменяет источник модели привязки по умолчанию для сложных типов с [FromForm]
    // на [FromBody].
    // Атрибут [ApiController] в методах действий неявно проверяет состояние модели, и в случае невалидности модели
    // отправлет код состояния 400 Bad Request в формате Problem Details.
    // Атрибут [ApiController] преобразует ответы с кодом состояния 4XX в формат Problem Details.
    [ApiController]
    // Класс ControllerBase предоставляет вспомогательные методы: Ok(), NotFound().
    public class FruitController : ControllerBase
    {
        readonly FruitService _fruitService;

        // Внедрение сервиса из контейнера внедрения зависимостей.
        public FruitController(FruitService fruitService) => _fruitService = fruitService;

        // Атрибут [HttpGet] определяет шаблон маршрута для метода действия.
        [HttpGet("fruit")]
        // Метод действия возвращает объект List<string>, который сериализуется в JSON.
        public List<string> Index() => _fruitService.Fruits;

        [HttpGet("fruit/{id}")]
        // Метод действия возвращает ActionResult<string>, поэтому он может возвращать string или ActionResult.
        public ActionResult<string> View(int id)
        {
            if (id < 0 || id >= _fruitService.Fruits.Count)
                // В случае возврата NotFoundResult, возвращается статусный код 404 Not found.
                return NotFound();

            // В случае возврата string, возвращается статусный код 200 OK.
            // Также можно обернуть string с помощью объекта OkResult, вызвав Ok(_fruitService.Fruits[id]).
            return _fruitService.Fruits[id];
        }
    }
}
