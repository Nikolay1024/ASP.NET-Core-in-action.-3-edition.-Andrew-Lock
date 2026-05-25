using Microsoft.AspNetCore.Mvc;
using WebApiControllerAttributeApp.BindingModels;
using WebApiControllerAttributeApp.Services;

namespace WebApiControllerAttributeApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Fruit2Controller : ControllerBase
    {
        readonly FruitService _fruitService;

        public Fruit2Controller(FruitService fruitService) => _fruitService = fruitService;

        [HttpGet]
        public ActionResult Update() => Ok(_fruitService.Fruits);

        [HttpPost]
        // Атрибут [ApiController] изменяет источник модели привязки по умолчанию для сложных типов с [FromForm]
        // на [FromBody].
        public ActionResult Update(UpdateModel updateModel)
        {
            // Атрибут [ApiController] в методах действий неявно проверяет состояние модели, и в случае невалидности
            // модели отправлет код состояния 400 Bad Request в формате Problem Details.

            if (updateModel.Id < 0 || updateModel.Id >= _fruitService.Fruits.Count)
                // Атрибут [ApiController] преобразует ответы с кодом состояния 4XX в формат Problem Details.
                return NotFound();

            _fruitService.Fruits[updateModel.Id] = updateModel.Name;
            return Ok();
        }
    }
}
