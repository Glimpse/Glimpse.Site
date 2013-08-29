using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Glimpse.Issues
{
    public class GithubIssueService
    {
        public virtual IEnumerable<GithubIssue> GetIssues(GithubIssueQuery issueQuery)
        {
            var client = SetupHttpClient("https://api.github.com/", "application/json");
            int currentpageIndex = 1;
            var requestUri =BuildRequestUri(issueQuery, currentpageIndex);
            var result = client.GetAsync(requestUri).Result;
            var issues = new List<GithubIssue>();
            issues.AddRange(result.Content.ReadAsAsync<IEnumerable<GithubIssue>>().Result);
            var lastPageIndex = GetLastPageIndex(result);
            for (currentpageIndex = 2; currentpageIndex <= lastPageIndex; currentpageIndex++)
            {
                requestUri = BuildRequestUri(issueQuery, currentpageIndex);
                result = client.GetAsync(requestUri).Result;
                issues.AddRange(result.Content.ReadAsAsync<IEnumerable<GithubIssue>>().Result);
            }
            return issues;
        }

        private int GetLastPageIndex(HttpResponseMessage result)
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


        private HttpClient SetupHttpClient(string baseAddress, string mediaType)
        {
            var client = new HttpClient {BaseAddress = new Uri(baseAddress)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return client;
        }

        private string BuildRequestUri(GithubIssueQuery issueQuery, int pageIndex)
        {
            var requestUri = "repos/glimpse/glimpse/issues?per_page=100&page=" + pageIndex;
            requestUri += "&state=" + (issueQuery.State == GithubIssueStatus.Open ? "open" : "closed");
            if (issueQuery.MilestoneNumber.HasValue)
                requestUri += "&milestone=" + issueQuery.MilestoneNumber.Value;
            return requestUri;
        }
    }
}