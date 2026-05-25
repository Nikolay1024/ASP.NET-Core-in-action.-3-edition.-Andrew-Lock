using Microsoft.AspNetCore.Mvc;

namespace WebApiRoutingApp.Controllers
{
    public class CarController : ControllerBase
    {
        // Метод действия Start() будет выполнен при совпадении любого из этих шаблонов маршрута.
        [Route("car/start")]
        [Route("car/ignition")]
        [Route("start-car")]
        // Имя метода действия не влияет на шаблон маршрута.
        public void Start() { }

        // Шаблон маршрута может содержать параметры маршрута, в данном случае {speed}.
        // Также поддерживаются все функции маршрутов, что и в минимальных API: обязательный параметр,
        // необязательный параметр, параметр со значением по умолчанию, ограничения параметров.
        [Route("car/speed/{speed=20:int}")]
        [Route("set-speed/{speed=20:int}")]
        public void SetCarSpeed(int speed) { }
    }
}
