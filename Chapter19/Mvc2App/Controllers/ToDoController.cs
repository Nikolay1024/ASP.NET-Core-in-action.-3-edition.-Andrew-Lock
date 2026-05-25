using Microsoft.AspNetCore.Mvc;
using Mvc2App.Services;

namespace Mvc2App.Controllers
{
    public class ToDoController : Controller
    {
        readonly ToDoService _service;

        // ToDoService предоставляется в конструкторе контроллера с использованием внедрения зависимостей.
        public ToDoController(ToDoService service) => _service = service;

        // Метод действия Category() принимает параметр id с помощью модели привязки.
        public ActionResult Category(string id)
        {
            // Метод действия вызывает ToDoService для получения данных и построения модели представления.
            List<ToDoListModel> items = _service.GetItemsForCategory(id);
            // Возвращает ViewResult, указывающий, что представление Razor должно быть отображено, передавая
            // модель представления.
            // В этом примере метод View() возвращает объект ViewResult без указания имени запускаемого представления.
            // Имя используемого представление основано на имени контроллера и имени метода действия. Т.е. вызывается
            // представление Category.cshtml в папке Views/ToDo.
            // Вы также можете явно передать имя представления методу View("Category"),
            // View("Views/ToDo/Category.cshtml").
            return View(items);
        }
    }
}
