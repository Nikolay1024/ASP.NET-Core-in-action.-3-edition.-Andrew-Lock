using System.ComponentModel.DataAnnotations;

namespace RazorPagesModelValidationApp.BindingModels
{
    public class UserBindingModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Почтовый адрес")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Номер телефона")]
        public string? PhoneNumber { get; set; }
    }
}
