namespace RazorPagesBindingModel3App.ViewModels
{
    public class CalculateSquareViewModel
    {
        public int Base { get; set; }
        public int Square { get; set; }

        public CalculateSquareViewModel() { }
        public CalculateSquareViewModel(int @base)
        {
            Base = @base;
            Square = Base * Base;
        }
    }
}
