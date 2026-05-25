using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAuthorizationApp.Pages.Airport
{
    [Authorize("CanAccessLounge")]
    public class AirportLoungeModel : PageModel
    {
        public void OnGet() { }
    }
}
