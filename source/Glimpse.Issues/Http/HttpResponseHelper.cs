using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Glimpse.Infrastructure.Http
{
    public class HttpResponseHelper
    {
        public int GetLastPageIndex(HttpResponseMessage result)
        {
            IEnumerable<string> linkHeader;
            var pages = new Dictionary<string, string>();
            if (result.Headers.TryGetValues("Link", out linkHeader))
            {
                var links = linkHeader.First().Split(',');
                foreach (var link in links)
                {
                    var linkSections = link.Split(';');
                    var urlSection = linkSections[0].Trim();
                    var url = urlSection.Substring(1, urlSection.IndexOf(">") - 1);
                    var rel = linkSections[1].Trim().Replace("rel=\"", "").Replace("\"", "");
                    pages.Add(rel, url);
                }
                var matches = Regex.Match(pages["last"], "[?&]page=(\\d*)");
                var lastPageIndex = Convert.ToInt32(matches.Groups[1].Value);
                return lastPageIndex;
            }
            return 1;
        }
    }
}