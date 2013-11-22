using System.Collections.Generic;

namespace Glimpse.Issues
{
    public interface IGithubMilestoneService
    {
        GithubMilestone GetLatestMilestoneWithIssues(string state);
        GithubMilestone GetMilestone(string title);
    }
}