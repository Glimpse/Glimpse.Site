using System.Collections.Generic;

namespace Glimpse.Issues
{
    public class IssueRepository : IIssueRepository
    {
        private readonly GithubIssueService _githubIssueService;
        private readonly GithubMilestoneService _milestoneService;

        public IssueRepository(GithubIssueService githubIssueService, GithubMilestoneService milestoneService)
        {
            _githubIssueService = githubIssueService;
            _milestoneService = milestoneService;
        }

        public IEnumerable<GithubIssue> GetAllIssuesFromMilestone(int milestoneNumber)
        {
            var githubIssues = new List<GithubIssue>();
            var closedIssues = GetClosedIssues(milestoneNumber);
            var openIssues = _githubIssueService.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Open, MilestoneNumber = milestoneNumber});
            githubIssues.AddRange(openIssues);
            githubIssues.AddRange(closedIssues);
            return githubIssues;
        }

        private IEnumerable<GithubIssue> GetClosedIssues(int milestoneNumber)
        {
            var closedIssues = _githubIssueService.GetIssues(new GithubIssueQuery()
                                                  {
                                                      State = GithubIssueStatus.Closed,
                                                      MilestoneNumber = milestoneNumber
                                                  });
            return closedIssues;
        }
    }
}