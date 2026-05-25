using Markdig;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPagesCustomTagHelpersApp.TagHelpers
{
    // Тег-хелпер Markdown будет использовать тег <markdown>.
    public class MarkdownTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Получаем содержимое тега <markdown>.
            TagHelperContent markdownRazorContent = await output.GetChildContentAsync();
            // Отрисовка содержимого Razor в строку.
            string markdown = markdownRazorContent.GetContent();
            // Преобразовываем строку Markdown в HTML с помощью Markdig.
            string html = Markdown.ToHtml(markdown);

            // Пишем содержимое HTML в вывод.
            output.Content.SetHtmlContent(html);
            // Удаляем элемент <markdown> из содержимого.
            output.TagName = null;
        }
    }
}
