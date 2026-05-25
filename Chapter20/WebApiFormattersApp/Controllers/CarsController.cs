using Microsoft.AspNetCore.Mvc;
using WebApiFormattersApp.ApiModels;

namespace WebApiFormattersApp.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        static readonly List<Car> CarsList = new() { new Car("Nissan", "Micra"), new Car("Ford", "Focus"), };

        // При получении данных в параметрах метода действия связыватель заполняет модель привязки с использованием
        // "входного форматера". При отправке модели API клиенту из метода действия применяется "выходной форматер".
        // Процесс определения "форматера" для согласования формата запросов и ответов между клиентом и сервером
        // называется "согласованием содержимого" content negotiation (conneg).

        [HttpGet, Tags("Get")]
        // Неявно применяется выходной форматер по умолчанию JSON, если не запрошен иной форматер в заголовке Accept.
        // Запрос форматера выполняется клиентом при заполнении заголовка Accept: text/json или text/xml.
        public ActionResult<List<Car>> GetCars() => CarsList;

        // Явно применяется выходной форматер JSON независимо от запрашиваемого формата в заголовке Accept.
        [HttpGet("json"), Produces("text/json"), Tags("Get")]
        public ActionResult<List<Car>> GetCarsAsJson() => CarsList;

        // Явно применяется выходной форматер XML независимо от запрашиваемого формата в заголовке Accept.
        [HttpGet("xml"), Produces("text/xml"), Tags("Get")]
        public ActionResult<List<Car>> GetCarsAsXml() => CarsList;

        // Применяется выходной форматер указанный в параметре маршрута (JSON или XML) независимо от запрашиваемого
        // формата в заголовке Accept.
        [HttpGet("format/{format}"), FormatFilter, Tags("Get")]
        public ActionResult<List<Car>> GetCarsAsFormat() => CarsList;


        [HttpPost, Tags("Post")]
        // Модель привязки может применять один из запрашиваемых входных форматеров JSON или XML.
        // Если сервер поддерживает запрошенный формат, то ответ форматируется соответственно, и в ответе указывается
        // заголовок Content-type: text/json или text/xml. Если сервер не поддерживает запрошенный формат,
        // то ответ форматируется по умолчанию в JSON.
        public IActionResult PostCar(Car car)
        {
            CarsList.Add(car);
            return Ok();
        }

        // Атрибут [Consumes] используется для ограничения разрешенных форматов, которые может принимать метод действия.
        // Если клиент отправляет запрос в неразрешенном формате, то метод действия не выполняется и возвращается
        // ответ 415 Unsupported Media Type.
        [HttpPost("json"), Consumes("text/json"), Tags("Post")]
        public IActionResult PostCarAsJson(Car car)
        {
            CarsList.Add(car);
            return Ok();
        }

        [HttpPost("xml"), Consumes("text/xml"), Tags("Post")]
        public IActionResult PostCarAsXml(Car car)
        {
            CarsList.Add(car);
            return Ok();
        }


        [HttpGet("null"), Tags("Spec")]
        // По умолчанию если вы возвращаете null в качестве модели API, будь то из метода действия или путем передачи
        // null в StatusCodeResult, то промежуточное ПО вернет ответ 204 No Content.
        public IActionResult GetNull() => Ok(null);

        [HttpGet("content"), Tags("Spec")]
        // Когда вы возвращаете строку в качестве модели API, если не задан заголовок Accept, промежуточное ПО
        // отформатирует ответ как text/plain. В этом приложении форматер text/plain отключен, поэтому применяется
        // форматер по умолчанию JSON, если не запрошен иной форматер в заголовке Accept.
        public ActionResult<string> GetString() => "Какая-то строка.";
    }
}
