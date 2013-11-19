using System.Collections.Generic;

namespace Glimpse.Issues
{
    public interface IGithubMilestoneService
    {
        IEnumerable<GithubMilestone> GetMilestones();
    }
}