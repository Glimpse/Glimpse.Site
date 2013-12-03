using System;
using Glimpse.Infrastructure.GitHub;

namespace Glimpse.Issues.Test
{
    public class IssueBuilder
    {
        private readonly GithubIssue _issue = new GithubIssue();

        public GithubIssue Build()
        {
            _issue.User = new GithubUser() {Id = Guid.NewGuid().ToString()};
            return _issue;
        }

        public IssueBuilder WithLabel(string tag)
        {
            _issue.Labels.Add(new GithubLabel() {Name = tag});
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
            _issue.Html_Url = "/issues/" + issueId;
            return this;
        }

        public IssueBuilder WithMilestone(string milestoneName)
        {
            _issue.Milestone = new GithubMilestone() {Title = milestoneName};
            return this;
        }
    }
}