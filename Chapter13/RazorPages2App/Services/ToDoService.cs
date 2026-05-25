namespace RazorPages2App.Services
{
    public record ToDoListModel(string Category, string Title);

    public class ToDoService
    {
        // Обычно данные загружаются из базы данных.
        static readonly List<ToDoListModel> _items = new List<ToDoListModel>()
        {
            new ToDoListModel("simple", "Bread"),
            new ToDoListModel("simple", "Milk"),
            new ToDoListModel("simple", "Get Gas"),
            new ToDoListModel("long", "Write Book"),
            new ToDoListModel("long", "Build Application"),
        };

        public List<ToDoListModel> GetItemsForCategory(string category)
        {
            // Фильтр по предоставленной категории.
            return _items.Where(i => i.Category == category).ToList();
        }
    }
}
