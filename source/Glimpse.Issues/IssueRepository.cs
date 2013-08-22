using System.Collections.Generic;

namespace Glimpse.Issues
{
    public class IssueRepository : IIssueRepository
    {
        private readonly GithubIssueService _githubIssueService;

        public IssueRepository(GithubIssueService githubIssueService)
        {
            _githubIssueService = githubIssueService;
        }

        public IEnumerable<GithubIssue> GetAllIssues()
        {
            var githubIssues = new List<GithubIssue>();
            var openIssues = _githubIssueService.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Open});
            var closedIssues = _githubIssueService.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Closed, MilestoneNumber = 8});
            githubIssues.AddRange(openIssues);
            githubIssues.AddRange(closedIssues);
            return githubIssues;
        }
    }

}