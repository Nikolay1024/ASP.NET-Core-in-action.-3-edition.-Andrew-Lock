using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApiAuthenticationApp.Data
{
    // Сущность ингредиента.
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        public required string Name { get; set; }
        // SQL Server выдает ошибку, если не задана точность для десятичных чисел.
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public required string Unit { get; set; }
    }
}
