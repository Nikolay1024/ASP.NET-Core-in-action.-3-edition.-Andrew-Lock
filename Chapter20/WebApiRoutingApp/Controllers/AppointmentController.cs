using Microsoft.AspNetCore.Mvc;

namespace WebApiRoutingApp.Controllers
{
    // Атрибут [Route] отвечает на все HTTP-методы. Вместо этого метод действия обычно должен обрабатывать только один
    // HTTP-метод. Вместо атрибута [Route] для методов действий можно использовать [HttpPost], [HttpGet], [HttpPut].
    // Если приложение получает запрос, совпадающий с шаблоном маршрута метода действия, но не совпадающий с нужным
    // HTTP-методом, вы получите ошибку 405 Method not allowed.
    public class AppointmentController : ControllerBase
    {
        // Выполняется только в ответ на GET запрос к URL-адресу appointments.
        [HttpGet("appointments")]
        public void ListAppointments() { }

        // Выполняется только в ответ на POST запрос к URL-адресу appointments.
        [HttpPost("appointments")]
        public void CreateAppointment() { }
    }
}
