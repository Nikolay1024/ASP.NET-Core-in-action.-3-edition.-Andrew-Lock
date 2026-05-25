using Microsoft.AspNetCore.Mvc;

namespace WebApiRoutingApp.Controllers
{
    // Для выделения общего префикса ко всем шаблонам маршрута контроллера можно применить атрибут [Route] к контроллеру.
    [Route("api/car")]
    public class Car2Controller : ControllerBase
    {
        // Объединяется для получения шаблона маршрута api/car/start.
        [Route("start")]
        // Объединяется для получения шаблона маршрута api/car/ignition.
        [Route("ignition")]
        // Не объединяется, потому что начинается с / и дает шаблон маршрута start-car.
        [Route("/start-car")]
        public void Start() { }

        // Объединяется для получения шаблона маршрута api/car/speed/{speed}»
        [Route("speed/{speed}")]
        // Не объединяется, потому что начинается с / и дает шаблон маршрута /set-speed/{speed}.
        [Route("/set-speed/{speed}")]
        public void SetCarSpeed(int speed) { }
    }
}
