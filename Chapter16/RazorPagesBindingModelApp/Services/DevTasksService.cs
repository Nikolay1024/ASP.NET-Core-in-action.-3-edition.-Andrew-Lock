namespace RazorPagesBindingModelApp.Services
{
    public record DevTaskModel(int Id, string Title, string Username, string State);

    public class DevTasksService
    {
        readonly List<DevTaskModel> _devTasks = new()
        {
            new(102, "Рассмотрите возможность запрета на выпуски/сборы в День дурака.", "Dave", "Open"),
            new(100, "Файл .gitignore слишком длинный.", "Billy", "Closed"),
            new(99, "Плохое покрытие кода.", "Andrew", "Open"),
            new(97, "TL;DR", "Andrew", "Closed"),
            new(96, "Добавлены анализаторы: Sonar.Lint, FxCop, StyleCop, NDepend", "James", "Open"),
            new(95, "Размер исходных файлов.", "Andrew", "Open"),
            new(94, "System.Ben неправильно наложен.", "James", "Open"),
            new(93, "Исправление ошибок dotnet-cli из `dotnet test`.", "James", "Open"),
            new(92, "Выделение нового Бена слишком медленное.", "Slodge", "Open"),
        };

        public List<DevTaskModel> GetDevTasks(string username, string state)
        {
            return _devTasks.Where(i => string.Equals(i.Username, username, StringComparison.OrdinalIgnoreCase))
                .Where(i => string.Equals(i.State, state, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
