using System;
using System.Collections.Generic;

namespace Glimpse.Release
{
    public class GithubIssue
    {
        public GithubIssue()
        {
            Labels = new List<GithubLabel>();
        }

        public string Url { get; set; }

        public string Labels_Url { get; set; }

        public string Comments_Url { get; set; }

        public string Events_Url { get; set; }

        public string Html_Url { get; set; }

        public string Id { get; set; }

        public string Number { get; set; }

        public string Title { get; set; }

        public GithubUser User { get; set; }

        public IList<GithubLabel> Labels { get; set; }

        public string State { get; set; }

        public GithubUser Assignee { get; set; }

        public GithubMilestone Milestone { get; set; }

        public int Comments { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime? Updated_At { get; set; }

        public DateTime? Closed_At { get; set; }

        public GithubPullRequest Pull_Request { get; set; }

        public string Body { get; set; }

        public GithubIssueStatus Status
        {
            get
            {
                return State == "open" ? GithubIssueStatus.Open : GithubIssueStatus.Closed;
            }
        }

        protected bool Equals(GithubIssue other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((GithubIssue)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        } 
    }
}