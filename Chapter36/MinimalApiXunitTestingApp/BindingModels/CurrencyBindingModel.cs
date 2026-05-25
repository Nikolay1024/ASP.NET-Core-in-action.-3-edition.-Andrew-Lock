using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MinimalApiXunitTestingApp.BindingModels
{
    public class CurrencyBindingModel
    {
        [Required, DisplayName("Денежные единицы GBP")]
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required, DisplayName("Обменный курс GBP -> USD")]
        [Range(0, double.MaxValue)]
        public decimal ExchangeRate { get; set; }

        [Required, DisplayName("Кол-во десятичных знаков")]
        [Range(0, 10)]
        public int DecimalPlaces { get; set; }
    }
}