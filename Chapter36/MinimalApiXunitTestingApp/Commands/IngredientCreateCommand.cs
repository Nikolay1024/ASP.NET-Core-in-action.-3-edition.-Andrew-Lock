using MinimalApiXunitTestingApp.Data;
using System.ComponentModel.DataAnnotations;

namespace MinimalApiXunitTestingApp.Commands
{
    public class IngredientCreateCommand
    {
        [Required, StringLength(100)]
        public required string Name { get; set; }
        [Range(0, int.MaxValue)]
        public decimal Quantity { get; set; }
        [Required, StringLength(20)]
        public required string Unit { get; set; }

        public Ingredient ToIngredient()
        {
            return new Ingredient()
            {
                Name = Name,
                Quantity = Quantity,
                Unit = Unit,
            };
        }
    }
}
