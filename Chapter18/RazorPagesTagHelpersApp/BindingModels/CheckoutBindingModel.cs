using System.ComponentModel.DataAnnotations;

namespace RazorPagesTagHelpersApp.BindingModels
{
    public class CheckoutBindingModel
    {
        [Required, Display(Name = "Имя")]
        [StringLength(100, ErrorMessage = "Максимальная длина {1}.")]
        public string FirstName { get; set; } = string.Empty;

        [Required, Display(Name = "Фамилия")]
        [StringLength(100, ErrorMessage = "Максимальная длина {1}.")]
        public string LastName { get; set; } = string.Empty;

        [Required, Display(Name = "Почтовый адрес")]
        [EmailAddress(ErrorMessage = "Недействительный почтовый адрес.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Номер телефона")]
        [Phone(ErrorMessage = "Недействительный номер телефона.")]
        public string? PhoneNumber { get; set; }
    }
}
