using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPagesCustomTagHelpersApp.TagHelpers
{
    // Имя класса тег-хелпера определяет имя тега, который вы должны создать в своем шаблоне Razor.
    // В теге будет удален суффикс и будет использоваться дефис (system-info).
    // Или можно использовать атрибут [HtmlTargetElement("system-info")] для указания целевого тега.
    // Необходимо зарегистрировать тег-хелпер в _ViewImports.cshtml.
    public class SystemInfoTagHelper : TagHelper
    {
        // При записи HTML содержимого на страницу необходим HtmlEncoder.
        private readonly HtmlEncoder _htmlEncoder;

        public SystemInfoTagHelper(HtmlEncoder htmlEncoder) => _htmlEncoder = htmlEncoder;

        // Атрибут HtmlAttributeName позволяет задавать свойства из разметки Razor.
        /// <summary>
        /// Показать текущее имя машины <see cref="Environment.MachineName"/>.
        /// По умолчанию true.
        /// </summary>
        [HtmlAttributeName("add-machine")]
        public bool IncludeMachine { get; set; } = true;

        /// <summary>
        /// Показать текущую ОС <see cref="RuntimeInformation.OSDescription"/>.
        /// По умолчанию true.
        /// </summary>
        [HtmlAttributeName("add-os")]
        public bool IncludeOs { get; set; } = true;

        // Метод Process() вызывается при отрисовке элемента.
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Заменяет тег <systeminfo> на тег <div>.
            output.TagName = "div";
            // Отображает начальный и конечный тег <div></div>.
            output.TagMode = TagMode.StartTagAndEndTag;

            var sb = new StringBuilder();
            if (IncludeMachine)
            {
                sb.Append(" <strong>Machine</strong> ");
                sb.Append(_htmlEncoder.Encode(Environment.MachineName));
            }
            if (IncludeOs)
            {
                sb.Append(" <strong>OS</strong> ");
                sb.Append(_htmlEncoder.Encode(RuntimeInformation.OSDescription));
            }

            // Задает содержимое тега <div>, используя значение в кодировке HTML.
            // Всегда кодируйте свой вывод в HTML перед записью в тег с помощью метода SetHtmlContent().
            // В качестве альтернативы можно передать незакодированный ввод в метод SetContent(),
            // и вывод будет автоматически закодирован в HTML.
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
