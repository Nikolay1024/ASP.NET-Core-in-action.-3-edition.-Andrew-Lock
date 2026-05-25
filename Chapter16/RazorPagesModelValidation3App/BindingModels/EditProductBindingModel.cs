using System.ComponentModel.DataAnnotations;

namespace RazorPagesModelValidation3App.BindingModels
{
    public class EditProductBindingModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000)]
        public decimal Price { get; set; }
    }
}
