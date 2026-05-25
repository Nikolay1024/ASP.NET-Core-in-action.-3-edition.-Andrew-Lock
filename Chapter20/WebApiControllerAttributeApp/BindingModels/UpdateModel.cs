using System.ComponentModel.DataAnnotations;

namespace WebApiControllerAttributeApp.BindingModels
{
    public class UpdateModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
