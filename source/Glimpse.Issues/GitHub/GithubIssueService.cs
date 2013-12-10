using System.Collections.Generic;
using System.Net.Http;
using Glimpse.Infrastructure.Http;

namespace Glimpse.Infrastructure.GitHub
{

    public class GithubIssueService
    {
        private readonly GithubIssueRequestBuilder _requestBuilder;
        private readonly HttpResponseHelper _httpResponseHelper;
        private readonly IHttpClient _httpClient;

        public GithubIssueService(IHttpClient httpClient)
        {
            _httpResponseHelper = new HttpResponseHelper();
            _requestBuilder = new GithubIssueRequestBuilder();
            _httpClient = httpClient;
        }

        public virtual IEnumerable<GithubIssue> GetIssues(GithubIssueQuery issueQuery)
        {
            int currentpageIndex = 1;
            var issues = new List<GithubIssue>();
            var result = CreateGithubIssuesFromQuery(issueQuery, currentpageIndex, issues);
            var lastPageIndex = _httpResponseHelper.GetLastPageIndex(result);
            for (currentpageIndex = 2; currentpageIndex <= lastPageIndex; currentpageIndex++)
            {
                CreateGithubIssuesFromQuery(issueQuery, currentpageIndex, issues);
            }
            return issues;
        }

        private HttpResponseMessage CreateGithubIssuesFromQuery(GithubIssueQuery issueQuery, int currentpageIndex, List<GithubIssue> issues)
        {
            var requestUri = _requestBuilder.BuildRequestUri(issueQuery, currentpageIndex);
            var result = _httpClient.GetAsync(requestUri).Result;
            issues.AddRange(ConvertToGithubIssues(result));
            return result;
        }

        private static IEnumerable<GithubIssue> ConvertToGithubIssues(HttpResponseMessage result)
        {
            return result.Content.ReadAsAsync<IEnumerable<GithubIssue>>().Result;
        }
    }
}