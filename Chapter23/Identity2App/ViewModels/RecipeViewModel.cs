namespace Identity2App.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Method { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public IEnumerable<IngredientViewModel> Ingredients { get; set; } = new List<IngredientViewModel>();
    }
}
