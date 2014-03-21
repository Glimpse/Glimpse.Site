using System.Collections.Generic;
using Glimpse.Infrastructure.GitHub;

namespace Glimpse.Infrastructure.Repositories
{
    public interface IIssueRepository
    {
        IEnumerable<GithubIssue> GetAllIssuesFromMilestone(int milestoneNumber);
    }
}