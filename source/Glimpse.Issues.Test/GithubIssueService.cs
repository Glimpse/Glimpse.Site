using System.Collections.Generic;
using System.Linq;
using RestSharp;
using Xunit;

namespace Glimpse.Issues.Test
{

    public class GithubIssueServiceTests
    {
        [Fact]
        public void Test()
        {
            var issueService = new GithubIssueService();

            var issues =issueService.GetIssues(new GithubIssueQuery() {RepoName = "glimpse/glimpse", State = GithubIssueStatus.Closed, MilestoneNumber = 8}).ToList();

            Assert.True(issues.Count() > 0);
        }
    }

    public class GithubIssueService
    {
        public virtual IEnumerable<GithubIssue> GetIssues(GithubIssueQuery issueQuery)
        {
            var client = new RestClient("https://api.github.com/");
//            EasyHttp.Http.HttpClient client = new EasyHttp.Http.HttpClient("https://api.github.com/");
//            client.Request.Accept = HttpContentTypes.ApplicationJson;
//            var client = SetupHttpClient("https://api.github.com/", "application/json");
            var requestUri = BuildRequestUri(issueQuery);
            var request = new RestRequest(requestUri);
            

            var response = client.Execute(request);
            var content = response.Headers;
            return null;
//            var result = client.Get(requestUri);
//            var issues = result.StaticBody<IEnumerable<GithubIssue>>();
//            return issues;
//            var response = client.GetAsync(requestUri);
//            var result = response.Result.Headers.GetValues("ETag");
//            return response.Result.Content.ReadAsAsync<IEnumerable<GithubIssue>>().Result;
        }

//        private HttpClient SetupHttpClient(string baseAddress, string mediaType)
//        {
//            var client = new HttpClient();

//            client.BaseAddress = new Uri(baseAddress);
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
//            client.DefaultRequestHeaders
//            return client;
//        }

        private string BuildRequestUri(GithubIssueQuery issueQuery)
        {
            var requestUri = string.Format("repos/{0}/issues?page=1&per_page=2&state={1}", issueQuery.RepoName,
                issueQuery.State == GithubIssueStatus.Open ? "open" : "closed");
            if (issueQuery.MilestoneNumber != 0)
                requestUri += "&milestone=" + issueQuery.MilestoneNumber;
            return requestUri;
        }
    }
}