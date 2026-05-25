using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAuthorizationApp.Pages.Airport
{
    // Применение политики CanEnterSecurity с помощью атрибута [Authorize].
    // Только пользователи, удовлетворяющие политике CanEnterSecurity, могут выполнять страницу Razor.

    [Authorize("CanEnterSecurity")]
    public class AirportSecurityModel : PageModel
    {
        public void OnGet() { }
    }
}
