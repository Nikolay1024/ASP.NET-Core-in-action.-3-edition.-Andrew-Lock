namespace WorkerServiceQuartzApp.Data
{
    public class Album
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
    }
}
