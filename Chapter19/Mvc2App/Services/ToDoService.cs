namespace Mvc2App.Services
{
    public record ToDoListModel(string Category, string Title);

    public class ToDoService
    {
        // Обычно данные загружаются из базы данных.
        static readonly List<ToDoListModel> _items = new()
        {
            new("simple", "Bread"),
            new("simple", "Milk"),
            new("simple", "Get Gas"),
            new("long", "Write Book"),
            new("long", "Build Application"),
        };

        public List<ToDoListModel> GetItemsForCategory(string category)
        {
            // Фильтр по предоставленной категории.
            return _items.Where(i => i.Category == category).ToList();
        }
    }
}
