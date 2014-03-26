using System.Collections.Generic;

namespace Glimpse.Release
{
    public interface IIssueProvider
    {
        GithubIssue GetIssue(string id);

        IList<GithubIssue> GetAllIssuesByMilestone(int number);

        IList<GithubIssue> GetAllIssuesByMilestoneThatHasTag(int number, IList<string> tags);

        IList<GithubIssue> GetAllIssues();

        void Clear();
    }
}