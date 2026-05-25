using System.ComponentModel.DataAnnotations;

namespace WebApiFormattersApp.ApiModels
{
    public class Car
    {
        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        // Для форматера XML требуется, чтобы класс сериализуемого объекта имел открытый конструктор без параметров.
        public Car() { }
        public Car(string make, string model)
        {
            Make = make;
            Model = model;
        }
    }
}
