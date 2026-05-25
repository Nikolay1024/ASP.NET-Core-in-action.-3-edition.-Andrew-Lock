namespace RazorPagesFluentValidationApp.BindingModels
{
    public class CurrencyConverterBindingModel
    {
        public string CurrencyFrom { get; set; } = null!;
        public string CurrencyTo { get; set; } = null!;
        public decimal Quantity { get; set; }
    }
}
