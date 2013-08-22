using System.Collections.Generic;

namespace Glimpse.Issues
{
    public interface IIssueRepository
    {
        IEnumerable<GithubIssue> GetAllIssues();
    }
}