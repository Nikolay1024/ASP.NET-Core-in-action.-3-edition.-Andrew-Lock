using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesBindingModel3App.ViewModels;

namespace RazorPagesBindingModel3App.Pages
{
    public class CalculateSquareModel : PageModel
    {
        public CalculateSquareViewModel ViewModel { get; set; } = new();

        public void OnGet(int @base)
        {
            ViewModel = new CalculateSquareViewModel(@base);
        }
    }
}
