namespace Glimpse.Infrastructure.GitHub
{
    public interface IGithubMilestoneService
    {
        GithubMilestone GetLatestMilestoneWithIssues(string state);
        GithubMilestone GetMilestone(string title);
    }
}