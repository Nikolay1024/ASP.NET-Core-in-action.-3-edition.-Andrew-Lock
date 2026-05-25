using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesRenderingHtmlApp.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// WARNING: Только для демонстрации, не потокобезопасно, вы можете сохранить эти значения в базе данных и т. д.
        /// </summary>
        static readonly List<string> _users = new() { "Bart", "Jimmy", "Robbie", };

        [BindProperty, Required]
        public string NewUser { get; set; } = string.Empty;
        public List<string> ExistingUsers { get; set; } = new();

        public void OnGet() => ExistingUsers = _users;
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            ExistingUsers = _users;

            if (_users.Contains(NewUser))
            {
                // Обратите внимание: обычно это можно извлечь в атрибут проверки.
                // Но я делаю это здесь, так как список пользователей жестко закодирован выше.
                ModelState.AddModelError(nameof(NewUser), "Этот пользователь уже существует.");
                return Page();
            }

            // Все ок, добавьте пользователя и очистите существующее значение.
            _users.Insert(0, NewUser);
            return RedirectToPage();
        }
    }
}
