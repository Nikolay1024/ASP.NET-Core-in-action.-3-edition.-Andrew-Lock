namespace RazorPagesRenderingHtml3App.Services
{
    public record HouseTask(int Id, params string[] HouseTaskSet)
    {
        public bool IsComplete => HouseTaskSet.Length == 0;
    }

    public class HouseTasksService
    {
        public List<HouseTask> HouseTasks { get; } = new()
        {
            new HouseTask(1, "Buy milk", "Buy eggs", "Buy bread"),
            new HouseTask(2, "Pick up kids", "Take kids to school"),
            new HouseTask(3, "Get fuel", "Check oil", "Check Tyre pressure"),
        };
    }
}
