namespace MinimalApiBackgroundService2App.Data
{
    public class Album
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
    }
}
