namespace WebApiFiltersApp.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Method { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public required IEnumerable<IngredientViewModel> Ingredients { get; set; }
    }
}
