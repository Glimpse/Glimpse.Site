using System;
using System.Collections.Generic;

namespace Glimpse.Issues.Test
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GithubIssue) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }

    public enum GithubIssueStatus
    {
        Open, Closed
    }


    public class GithubLabel
    {
        public string Url {get;set;}
        public string Name {get;set;}
        public string Color {get;set;}
}

    public class GithubMilestone
    {
        public string Url { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Open_Issues { get; set; }
        public int Closed_Issues { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Due_On { get; set; }
    }

    public class GithubPullRequest
    {
        public string Html_Url { get; set; }
        public string Diff_Url { get; set; }
        public string Patch_Url { get; set; }
    }

    public class GithubAssignee
    {
        public string Login {get;set;}
         public string Id {get;set;}
        public string Avataer_Url {get;set;}
        public string Gravater_Id {get;set;}
        public string Url {get;set;}
   }

    public class GithubUser
    {
        public string Login { get; set; }
        public string Id { get; set; }
        public string Avatar_Url { get; set; }
        public string Gravatar_Id { get; set; }
        public string Url { get; set; }
        public string Html_Url { get; set; }
        public string Followers_Url { get; set; }
        public string Type { get; set; }
    }
}