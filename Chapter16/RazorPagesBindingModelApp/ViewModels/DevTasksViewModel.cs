namespace RazorPagesBindingModelApp.ViewModels
{
    public class DevTasksViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public List<DevTaskViewModel> DevTasks { get; set; } = new List<DevTaskViewModel>();

        public DevTasksViewModel() { }
        public DevTasksViewModel(string username, string state)
        {
            Username = username;
            State = state;
        }
    }
}
