namespace EntityFrameworkCoreApp.ViewModels
{
    public class IngredientViewModel
    {
        public required string Name { get; set; }
        public decimal Quantity { get; set; }
        public required string Unit { get; set; }
    }
}
