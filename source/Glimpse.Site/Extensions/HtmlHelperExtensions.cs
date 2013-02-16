using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MarkdownSharp;

namespace Glimpse.Site.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static readonly IDictionary<string, IHtmlString> cache = new Dictionary<string, IHtmlString>();

        public static IHtmlString RenderMarkdown(this HtmlHelper htmlHelper, FileInfo mdFile)
        {
            var cacheKey = mdFile.FullName.ToLower();

            if (cache.ContainsKey(cacheKey))
                return cache[cacheKey];

            var markdown = string.Empty;
            using (var reader = mdFile.OpenText())
            {
                markdown = reader.ReadToEnd();
            }

            var result = htmlHelper.RenderMarkdown(markdown);

            cache[cacheKey] = result;

            return result;
        }

        public static IHtmlString RenderMarkdown(this HtmlHelper htmlHelper, string markdown)
        {
            return htmlHelper.Raw(new Markdown().Transform(markdown));
        }
    }
}