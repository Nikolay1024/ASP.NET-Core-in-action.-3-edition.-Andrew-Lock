namespace RazorPagesPartialViewApp.Services
{
    public record HouseTask(int Id, string Title, params string[] HouseTaskSet)
    {
        public bool IsComplete => HouseTaskSet.Length == 0;
    }

    public class HouseTasksService
    {
        public List<HouseTask> HouseTasks { get; } = new()
        {
            new HouseTask(1, "Shopping List", "Buy milk", "Buy eggs", "Buy bread"),
            new HouseTask(2, "Agenda", "Pick up kids", "Take kids to school"),
            new HouseTask(3, "Car stuff", "Get fuel", "Check oil", "Check Tyre pressure"),
            new HouseTask(4, "Problems"),
            new HouseTask(5, "Writing tasks","Write blog post", "Edit book chapter"),
        };
    }
}
