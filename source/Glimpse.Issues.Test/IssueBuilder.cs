namespace Glimpse.Issues.Test
{
    public class IssueBuilder
    {
        readonly GithubIssue _issue = new GithubIssue();

        public GithubIssue Build()
        {
            return _issue;
        }

        public IssueBuilder WithTag(string tag)
        {
            _issue.Body = string.Format("Random content but tagged with {0} ", tag);
            return this;
        }

        public IssueBuilder WithState(string state)
        {
            _issue.State = state;
            return this;
        }

        public IssueBuilder WithId(string issueId)
        {
            _issue.Id = issueId;
            return this;
        }

        public IssueBuilder WithMilestone(string milestoneName)
        {
            _issue.Milestone = new GithubMilestone() {Title = milestoneName};
            return this;
        }
    }
}