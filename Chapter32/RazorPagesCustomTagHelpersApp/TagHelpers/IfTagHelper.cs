using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPagesCustomTagHelpersApp.TagHelpers
{
    // Cвойство Attributes указывает, что тег-хелпер запускается атрибутом if и
    // может быть использован в любом теге (<div if="true" />).
    [HtmlTargetElement(Attributes = "if")]
    public class IfTagHelper : TagHelper
    {
        // Гарантирует, что этот тег-хелпер запускается раньше всех остальных, прикрепленных к элементу.
        public override int Order => int.MinValue;

        // Привязывает значение атрибута if к свойству RenderContent.
        [HtmlAttributeName("if")]
        public bool RenderContent { get; set; } = true;

        // Движок Razor вызывает метод Process() для выполнения тег-хелпера.
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (RenderContent == false)
            {
                // Задает для тега, где находится тег-хелпер, значение null, удаляя его со страницы.
                output.TagName = null;
                // Не отображает и не вычисляет внутреннее содержимое тега.
                output.SuppressOutput();
            }
            output.Attributes.RemoveAll("if");
        }
    }
}
