using RazorPagesModelValidation2App.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesModelValidation2App.BindingModels
{
    public class CurrencyConvertBindingModel
    {
        [Required, Display(Name = "Исходная валюта")]
        [StringLength(3, MinimumLength = 3)]
        [CurrencyCode("GBP", "USD", "CAD", "EUR")]
        public string CurrencyFrom { get; set; } = string.Empty;

        [Required, Display(Name = "Целевая валюта")]
        [StringLength(3, MinimumLength = 3)]
        [CurrencyCode("GBP", "USD", "CAD", "EUR")]
        public string CurrencyTo { get; set; } = string.Empty;

        [Required, Display(Name = "Денежные единицы")]
        [Range(1, 1000)]
        public decimal Quantity { get; set; }
    }
}
