using System.Collections;

namespace RazorPagesXunitTestingApp.Test
{
    public class CurrencyServiceClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0.0001m, 0.0001m, 1m };
            yield return new object[] { decimal.MaxValue, decimal.MaxValue, 1m };
            yield return new object[] { decimal.MaxValue * 0.0001m, 0.0001m, decimal.MaxValue };
            yield return new object[] { 0.0001m, decimal.MaxValue, 0m };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
