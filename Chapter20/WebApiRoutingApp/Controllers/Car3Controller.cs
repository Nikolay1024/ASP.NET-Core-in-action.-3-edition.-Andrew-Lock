using Microsoft.AspNetCore.Mvc;

namespace WebApiRoutingApp.Controllers
{
    // Для уменьшения дублирования кода можно использовать маркеры [controller] и [action], которые заменяются на
    // соответствующие имена контроллера (сar3) и методов действий (start, setcarspeed).
    [Route("api/[controller]")]
    public class Car3Controller : ControllerBase
    {
        // Объединяет и заменяет маркеры, чтобы получить шаблон api/car3/start.
        [Route("[action]")]
        // Объединяет и заменяет маркеры, что бы получить шаблон api/car3/ignition.
        [Route("ignition")]
        // Не объединяется, потому что начинается с / и дает шаблон маршрута start-car.
        [Route("/start-car")]
        public void Start() { }
    }
}
