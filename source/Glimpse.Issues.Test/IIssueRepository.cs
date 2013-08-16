using System.Collections.Generic;

namespace Glimpse.Issues.Test
{
    public interface IIssueRepository
    {
        IEnumerable<GithubIssue> GetAllIssues();
    }
}