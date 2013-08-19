namespace Glimpse.Issues.Test
{
    public class GithubIssueQuery
    {
        public GithubIssueQuery()
        {
            State = GithubIssueStatus.Open;
        }

        public GithubIssueStatus State { get; set; }
        public string RepoName { get; set; }
        public int MilestoneNumber { get; set; }

        protected bool Equals(GithubIssueQuery other)
        {
            return string.Equals(State, other.State) && string.Equals(RepoName, other.RepoName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GithubIssueQuery) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((State != null ? State.GetHashCode() : 0)*397) ^ (RepoName != null ? RepoName.GetHashCode() : 0);
            }
        }

    }
}