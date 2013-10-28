using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Glimpse.Issues
{
    public class GithubIssueService
    {
        private readonly GithubIssueRequestBuilder _requestBuilder;
        private readonly HttpResponseHelper _httpResponseHelper;

        public GithubIssueService()
        {
            _httpResponseHelper = new HttpResponseHelper();
            _requestBuilder = new GithubIssueRequestBuilder();
        }

        public virtual IEnumerable<GithubIssue> GetIssues(GithubIssueQuery issueQuery)
        {
            var client = SetupHttpClient("https://api.github.com/", "application/json");
            var issues = QueryGitHubHttpService(issueQuery, client);
            return issues;
        }

        private IEnumerable<GithubIssue> QueryGitHubHttpService(GithubIssueQuery issueQuery, HttpClient client)
        {
            int currentpageIndex = 1;
            var issues = new List<GithubIssue>();
            var result = CreateGithubIssuesFromQuery(issueQuery, client, currentpageIndex, issues);
            var lastPageIndex = _httpResponseHelper.GetLastPageIndex(result);
            for (currentpageIndex = 2; currentpageIndex <= lastPageIndex; currentpageIndex++)
            {
                CreateGithubIssuesFromQuery(issueQuery, client, currentpageIndex, issues);
            }
            return issues;
        }

        private HttpResponseMessage CreateGithubIssuesFromQuery(GithubIssueQuery issueQuery, HttpClient client,
            int currentpageIndex, List<GithubIssue> issues)
        {
            var requestUri = _requestBuilder.BuildRequestUri(issueQuery, currentpageIndex);
            var result = SendGetRequest(client, requestUri);
            issues.AddRange(ConvertToGithubIssues(result));
            return result;
        }

        private static HttpResponseMessage SendGetRequest(HttpClient client, string requestUri)
        {
            return client.GetAsync(requestUri).Result;
        }

        private static IEnumerable<GithubIssue> ConvertToGithubIssues(HttpResponseMessage result)
        {
            return result.Content.ReadAsAsync<IEnumerable<GithubIssue>>().Result;
        }


        private HttpClient SetupHttpClient(string baseAddress, string mediaType)
        {
            var client = new HttpClient {BaseAddress = new Uri(baseAddress)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return client;
        }
    }
}