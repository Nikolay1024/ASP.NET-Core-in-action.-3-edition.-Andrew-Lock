using RazorPagesTagHelpersApp.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesTagHelpersApp.BindingModels
{
    public class ConvertBindingModel// : IValidatableObject
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

        // Метод проверяет, чтобы исходная (CurrencyFrom) и целевая валюта (CurrencyTo) не были одинаковыми,
        // иначе возвращает ошибку.
        // Выполняется только в том случае, если соблюдены все условия атрибутов DataAnnotation.
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (CurrencyFrom == CurrencyTo)
        //        yield return new ValidationResult("Невозможно конвертировать валюту в себя.",
        //            new[] { nameof(CurrencyFrom), nameof(CurrencyTo) });
        //}
    }
}
