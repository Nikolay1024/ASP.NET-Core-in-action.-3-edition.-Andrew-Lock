using Identity2App.Data;
using System.ComponentModel.DataAnnotations;

namespace Identity2App.Commands
{
    public class IngredientCreateCommand
    {
        [Required, StringLength(100)]
        public required string Name { get; set; }
        [Range(0, int.MaxValue)]
        public decimal Quantity { get; set; }
        [Required, StringLength(20)]
        public required string Unit { get; set; }

        // Отображаем каждую команду CreateIngredientCommand в сущность Ingredient.
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
