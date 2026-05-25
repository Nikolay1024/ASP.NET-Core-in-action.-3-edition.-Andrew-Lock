namespace IdentityAuthorization2App.ViewModels
{
    public class RecipeSummaryViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string TimeToCook { get; set; }
        public int NumberOfIngredients { get; set; }
    }
}
