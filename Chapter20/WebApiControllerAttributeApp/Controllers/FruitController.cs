using Microsoft.AspNetCore.Mvc;
using WebApiControllerAttributeApp.BindingModels;
using WebApiControllerAttributeApp.Services;

namespace WebApiControllerAttributeApp.Controllers
{
    [Route("[controller]")]
    public class FruitController : ControllerBase
    {
        readonly FruitService _fruitService;

        public FruitController(FruitService fruitService) => _fruitService = fruitService;

        [HttpGet]
        public ActionResult Update() => Ok(_fruitService.Fruits);

        [HttpPost]
        // Атрибут [FromBody] указывает на то, что параметр должен быть привязан к телу запроса.
        public ActionResult Update([FromBody] UpdateModel updateModel)
        {
            // Нужно проверить, была ли валидация модели успешной, и вернуть ответ с кодом 400, если она не удалась.
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Если отправленные данные не содержат допустимого идентификатора, вернуть ответ 404 ProblemDetails.
            if (updateModel.Id < 0 || updateModel.Id >= _fruitService.Fruits.Count)
            {
                return NotFound(new ProblemDetails()
                {
                    Status = 404,
                    Title = "Not Found",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                });
            }

            // Обновляем модель и возвращаем ответ с кодом состояния 200.
            _fruitService.Fruits[updateModel.Id] = updateModel.Name;
            return Ok();
        }
    }
}
