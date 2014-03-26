using System.Collections.Generic;

namespace Glimpse.Release
{
    public interface IMilestoneProvider
    {
        GithubMilestone GetLatestMilestoneWithIssues(string state);

        GithubMilestone GetMilestone(string title);

        IList<GithubMilestone> GetAllMilestones();

        void Clear();
    }
}