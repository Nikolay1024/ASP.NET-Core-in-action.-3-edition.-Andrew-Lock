namespace MinimalApiXunitTestingApp.Services
{
    public class CurrencyService : ICurrencyService
    {
        public decimal ConvertToGbp(decimal quantity, decimal exchangeRate, int decimalPlaces)
        {
            if (quantity <= 0)
                throw new ArgumentException("Денежные единицы GBP должны быть больше 0.", nameof(quantity));

            if (exchangeRate <= 0)
                throw new ArgumentException("Обменный курс должен быть больше 0.", nameof(exchangeRate));

            if (decimalPlaces < 0)
                throw new ArgumentException("Кол-во десятичных знаков не должно быть меньше 0.",
                    nameof(decimalPlaces));

            if (decimalPlaces > 10)
                throw new ArgumentException("Кол-во десятичных знаков не должно быть больше 10.",
                    nameof(decimalPlaces));

            return decimal.Round(quantity / exchangeRate, decimalPlaces);
        }
    }
}
