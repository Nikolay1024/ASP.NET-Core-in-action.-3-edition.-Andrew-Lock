namespace RazorPagesRouteTemplatesApp.Services
{
    public record Product(string Name, decimal Price);

    public class ProductService
    {
        static readonly IDictionary<string, Product> _allProducts = new Dictionary<string, Product>()
        {
            { "big-widget", new Product("Big widget", 123) },
            { "super-fancy-widget", new Product("Super fancy widget", 456) },
        };

        public Product? GetProduct(string name)
        {
            if (_allProducts.TryGetValue(name, out Product? product))
                return product;

            return null;
        }

        public List<Product> Search(string term, StringComparison comparisonType)
        {
            return _allProducts.Where(p => p.Value.Name.Contains(term, comparisonType)).Select(p => p.Value).ToList();
        }
    }
}
