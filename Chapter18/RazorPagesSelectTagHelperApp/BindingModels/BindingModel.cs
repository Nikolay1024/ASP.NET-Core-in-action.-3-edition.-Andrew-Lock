using RazorPagesSelectTagHelperApp.Enums;

namespace RazorPagesSelectTagHelperApp.BindingModels
{
    // Value1..Value7
    // Эти свойства будут содержать значения, выбранные в полях выбора с выбором одного элемента.
    // Values3, Values4
    // Эти свойства будут содержать значения, выбранные в полях выбора с выбором множества элементов.
    public class BindingModel
    {
        public string Value1 { get; set; } = string.Empty;
        public string Value2 { get; set; } = string.Empty;
        public List<string> Values3 { get; set; } = new();
        public List<string> Values4 { get; set; } = new();
        public string Value5 { get; set; } = string.Empty;
        public string Value6 { get; set; } = string.Empty;
        public string Value7 { get; set; } = string.Empty;
        public ProgrammingLanguages Value8 { get; set; } = ProgrammingLanguages.JavaScript;
    }
}
