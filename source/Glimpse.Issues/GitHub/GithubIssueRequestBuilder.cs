namespace Glimpse.Issues
{
    public class GithubIssueRequestBuilder
    {
        public string BuildRequestUri(GithubIssueQuery issueQuery, int pageIndex)
        {
            var requestUri = "repos/glimpse/glimpse/issues?per_page=100&page=" + pageIndex;
            requestUri += "&state=" + (issueQuery.State == GithubIssueStatus.Open ? "open" : "closed");
            if (issueQuery.MilestoneNumber.HasValue)
                requestUri += "&milestone=" + issueQuery.MilestoneNumber.Value;
            return requestUri;
        }
    }
}