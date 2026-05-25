using RazorPagesModelValidation3App.Models;

namespace RazorPagesModelValidation3App.Services
{
    public class ProductService
    {
        public readonly Dictionary<int, ProductDetails> Products = new()
        {
            { 1, new ProductDetails("Apple iPod", 200, 50) },
            { 2, new ProductDetails("Surface Book", 2200, 10) },
            { 3, new ProductDetails("XPS 15", 1600, 3) },
        };

        public ProductDetails? GetProduct(int id)
        {
            if (Products.TryGetValue(id, out ProductDetails? product))
                return product;

            return null;
        }
        public void UpdateProduct(int id, string name, decimal price)
        {
            ProductDetails? product = GetProduct(id);
            if (product is null)
                return;

            product.Name = name;
            product.Price = price;
        }
    }
}
