namespace RazorPagesPageHandlers.Services
{
    public record Product(string Name);

    public class SearchService
    {
        static readonly List<Product> _products = new List<Product>()
        {
            new Product("iPad"),
            new Product("iPod"),
            new Product("iMac"),
            new Product("Mac Pro"),
            new Product("Mac mini"),
        };

        public List<Product> SearchProducts(string term)
        {
            return _products.Where(p => p.Name.Contains(term)).ToList();
        }
    }
}
