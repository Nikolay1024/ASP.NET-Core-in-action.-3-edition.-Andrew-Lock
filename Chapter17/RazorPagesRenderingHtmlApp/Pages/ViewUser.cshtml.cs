using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesRenderingHtmlApp.Pages
{
    public class ViewUserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Username { get; set; } = string.Empty;

        public void OnGet() { }
    }
}
