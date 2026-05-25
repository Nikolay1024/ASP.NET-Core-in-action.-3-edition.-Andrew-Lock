namespace RazorPagesBindingModelApp.ViewModels
{
    public class DevTaskViewModel
    {
        public int Id { get; }
        public string Title { get; set; } = string.Empty;

        public DevTaskViewModel() { }
        public DevTaskViewModel(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
