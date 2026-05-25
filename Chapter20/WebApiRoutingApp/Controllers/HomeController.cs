using Microsoft.AspNetCore.Mvc;

namespace WebApiRoutingApp.Controllers
{
    // Атрибут [Route] отвечает на все HTTP-методы. Вместо этого метод действия обычно должен обрабатывать только один
    // HTTP-метод. Вместо атрибута [Route] для методов действий можно использовать [HttpPost], [HttpGet], [HttpPut].
    public class HomeController : ControllerBase
    {
        // Метод действия Index() будет выполнен, когда будет выполнен запрос к URL-адресу /.
        [Route("")]
        public void Index() { }

        // Метод действия Contact() будет выполнен, когда будет выполнен запрос к URL-адресу contact.
        [Route("contact")]
        public void Contact() { }
    }
}
