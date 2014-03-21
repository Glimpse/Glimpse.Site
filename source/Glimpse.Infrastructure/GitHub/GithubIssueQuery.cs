using System.Collections.Generic;

namespace Glimpse.Infrastructure.GitHub
{
    public class GithubIssueQuery
    {
        public GithubIssueQuery()
        {
            State = GithubIssueStatus.Open;
            Labels = new List<string>();
        }

        public GithubIssueStatus State { get; set; }
        public int? MilestoneNumber { get; set; }
        public List<string> Labels { get; set; }
    }
}