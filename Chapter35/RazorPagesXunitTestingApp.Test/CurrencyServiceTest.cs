using RazorPagesXunitTestingApp.Services;

namespace RazorPagesXunitTestingApp.Test
{
    // Тесты xUnit должны находиться в открытых нестатических классах.
    // Данные тесты тестируют метод CurrencyService.ConvertToGbp().
    // Модульные тесты используют паттерн "Подготовка, выполнение и проверка" (Arrange act assert, AAA).
    public class CurrencyServiceTest
    {
        #region Тесты, проверяющие обычные случаи (UsualCase).
        // Атрибут [Fact] помечает метод как тест.
        // Метод должен быть открытым, не иметь параметров и ничего не возвращать (void).
        // Это также может быть асинхронный метод, который возвращает Task.
        [Fact]
        public void ConvertToGbp_Fact_UsualCase()
        {
            // Тестируемый класс, обычно называемый "Тестируемой системой" (System under test, SUT).
            var currencyService = new CurrencyService();

            // Параметры теста, которые будут переданы в тестируемый метод ConvertToGbp().
            decimal quantity = 10m;
            decimal exchangeRate = 3m;
            int decimalPlaces = 4;

            // Ожидаемый результат.
            decimal expected = 3.3333m;

            // Выполнение тестируемого метода ConvertToGbp() и сохранение фактического результата.
            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            // Проверка соответствия ожидаемого и фактического результата.
            // Если они не совпадают, выбрасывается исключение EqualException.
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Тесты, проверяющие граничные случаи (EdgeCase).
        // Атрибут [Theory] помечает метод как параметризованный тест.
        [Theory]
        // Каждый атрибут [InlineData] предоставляет параметры для одного запуска теста.
        [InlineData(0.001, 0.001, 1)]
        [InlineData(1000, 1000, 1)]
        [InlineData(1000, 0.001, 1000000)]
        [InlineData(0.001, 1000, 0)]
        public void ConvertToGbp_InlineData_EdgeCase(decimal quantity, decimal exchangeRate, decimal expected)
        {
            var currencyService = new CurrencyService();
            int decimalPlaces = 4;

            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            Assert.Equal(expected, actual);
        }

        // Атрибут [ClassData] предоставляет параметры из класса для нескольких запусков теста.
        [Theory, ClassData(typeof(CurrencyServiceClassData))]
        public void ConvertToGbp_ClassData_EdgeCase(decimal quantity, decimal exchangeRate, decimal expected)
        {
            var currencyService = new CurrencyService();
            int decimalPlaces = 4;

            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            Assert.Equal(expected, actual);
        }

        public static List<object[]> Data1 => new()
        {
            new object[] { 0.0001m, 0.0001m, 1m },
            new object[] { decimal.MaxValue, decimal.MaxValue, 1m },
            new object[] { decimal.MaxValue * 0.0001m, 0.0001m, decimal.MaxValue },
            new object[] { 0.0001m, decimal.MaxValue, 0m },
        };

        // Атрибут [MemberData] предоставляет параметры из статического свойства.
        [Theory, MemberData(nameof(Data1))]
        public void ConvertToGbp_MemberData1_EdgeCase(decimal quantity, decimal exchangeRate, decimal expected)
        {
            var currencyService = new CurrencyService();
            int decimalPlaces = 4;

            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<decimal, decimal, decimal> Data2 => new()
        {
            { 0.0001m, 0.0001m, 1m },
            { decimal.MaxValue, decimal.MaxValue, 1m },
            { decimal.MaxValue * 0.0001m, 0.0001m, decimal.MaxValue },
            { 0.0001m, decimal.MaxValue, 0m },
        };

        [Theory]
        // Атрибут [MemberData] предоставляет типизированные параметры из статического свойства.
        [MemberData(nameof(Data2))]
        public void ConvertToGbp_MemberData2_EdgeCase(decimal quantity, decimal exchangeRate, decimal expected)
        {
            var currencyService = new CurrencyService();
            int decimalPlaces = 4;

            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            Assert.Equal(expected, actual);
        }

        public static List<object[]> GetData(int skip, int take)
        {
            var data = new List<object[]>()
            {
                new object[] { 0.0001m, 0.0001m, 1m },
                new object[] { decimal.MaxValue, decimal.MaxValue, 1m },
                new object[] { decimal.MaxValue * 0.0001m, 0.0001m, decimal.MaxValue },
                new object[] { 0.0001m, decimal.MaxValue, 0m },
            };

            return data.Skip(skip).Take(take).ToList();
        }

        // Атрибут [MemberData] предоставляет параметры из статического метода.
        [Theory, MemberData(nameof(GetData), new object[] { 1, 2 })]
        public void ConvertToGbp_MemberData3_EdgeCase(decimal quantity, decimal exchangeRate, decimal expected)
        {
            var currencyService = new CurrencyService();
            int decimalPlaces = 4;

            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            Assert.Equal(expected, actual);
        }

        [Theory]
        // Атрибут [MemberData] предоставляет параметры из статического свойства другого класса.
        [MemberData(nameof(CurrencyServiceMemberData.Data1), MemberType = typeof(CurrencyServiceMemberData))]
        public void ConvertToGbp_MemberData4_EdgeCase(decimal quantity, decimal exchangeRate, decimal expected)
        {
            var currencyService = new CurrencyService();
            int decimalPlaces = 4;

            decimal actual = currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces);

            Assert.Equal(expected, actual);
        }
        #endregion

        #region Тесты, проверяющие ошибочные случаи (ErrorCase).
        [Theory]
        [MemberData(nameof(CurrencyServiceMemberData.Data2), MemberType = typeof(CurrencyServiceMemberData))]
        public void ConvertToGbp_MemberData5_ErrorCase(decimal quantity, decimal exchangeRate, int decimalPlaces)
        {
            var currencyService = new CurrencyService();

            // Проверка, что будет выброшено исключение ArgumentException.
            // Если исключение не будет выброшено или будет выброшено исключение другого типа, то тест
            // завершится неудачно.
            // Лямбда-выражение вызывает метод, который должен выбросить исключение ArgumentException.
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
                currencyService.ConvertToGbp(quantity, exchangeRate, decimalPlaces));
        }
        #endregion
    }
}
