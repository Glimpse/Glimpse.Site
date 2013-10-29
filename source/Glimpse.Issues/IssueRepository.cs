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

        public IEnumerable<GithubIssue> GetAllIssues()
        {
            var githubIssues = new List<GithubIssue>();
            var closedIssues = GetClosedIssues();
            var openIssues = _githubIssueService.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Open});
            githubIssues.AddRange(openIssues);
            githubIssues.AddRange(closedIssues);
            return githubIssues;
        }

        private IEnumerable<GithubIssue> GetClosedIssues()
        {
            var vnextMilestone = _milestoneService.GetMilestone("vnext");
            var closedIssues = _githubIssueService.GetIssues(new GithubIssueQuery()
                                                  {
                                                      State = GithubIssueStatus.Closed,
                                                      MilestoneNumber = vnextMilestone.Number
                                                  });
            return closedIssues;
        }
    }
}