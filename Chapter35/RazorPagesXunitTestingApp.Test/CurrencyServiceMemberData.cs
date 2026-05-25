namespace RazorPagesXunitTestingApp.Test
{
    public class CurrencyServiceMemberData
    {
        public static List<object[]> Data1 => new()
        {
            new object[] { 0.0001m, 0.0001m, 1m },
            new object[] { decimal.MaxValue, decimal.MaxValue, 1m },
            new object[] { decimal.MaxValue * 0.0001m, 0.0001m, decimal.MaxValue },
            new object[] { 0.0001m, decimal.MaxValue, 0m },
        };

        public static TheoryData<decimal, decimal, int> Data2 => new()
        {
            { 0m, 1m, 0 },
            { 1m, 0m, 0 },
            { 1m, 1m, -1 },
            { 1m, 1m, 11 },
        };
    }
}
