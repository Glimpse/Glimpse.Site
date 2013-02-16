using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Glimpse.Site.Models
{
    public class DocumentationViewModel
    {
        private FileInfo markdownFilePath;
        private IEnumerable<DocumentationViewModel> navigationDocuments;

/*
        public DocumentationViewModel(string markdownSlug, RouteData routeData) : this(markdownSlug, "Views/" + routeData.GetRequiredString("controller"))
        {
        }
*/

        public DocumentationViewModel(string markdownSlug, string viewsLocation) : this(markdownSlug, viewsLocation, true)
        {
        }

        private DocumentationViewModel(string markdownSlug, string viewsLocation, bool isRendered)
        {
            MarkdownSlug = markdownSlug;
            ViewsLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, viewsLocation);
            IsRendered = isRendered;
        }

        public string EditUri 
        {
            get
            {
                return string.Format("https://github.com/Glimpse/Glimpse/wiki/{0}/_edit", MarkdownSlug);
            }
        }

        public bool IsRendered { get; private set; }

        public string MarkdownSlug { get; set; }
        
        public FileInfo MarkdownFile 
        {
            get
            {
                return markdownFilePath ?? (markdownFilePath = new FileInfo(Path.Combine(ViewsLocation, MarkdownSlug + ".md")));
            }
        }

        public string Title 
        {
            get { return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(MarkdownSlug.Replace('-', ' ').ToLower()); }
        }

        public IEnumerable<DocumentationViewModel> NavigationDocuments {
            get
            {
                if (navigationDocuments != null)
                    return navigationDocuments;

                var results = new List<DocumentationViewModel>();
                foreach (var file in Directory.EnumerateFiles(ViewsLocation, "*.md"))
                {
                    var slug = Path.GetFileNameWithoutExtension(file);

                    if (slug.StartsWith("_")) continue;

                    if (slug.Equals(MarkdownSlug, StringComparison.InvariantCultureIgnoreCase))
                        results.Add(this);
                    else
                        results.Add(new DocumentationViewModel(slug, ViewsLocation, false));
                }

                return navigationDocuments = results;
            }
        }

        public string ViewsLocation { get; set; }
    }
}