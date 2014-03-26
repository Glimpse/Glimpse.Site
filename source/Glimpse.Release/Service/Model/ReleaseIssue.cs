namespace Glimpse.Release
{
    public class ReleaseIssue
    {
        public string IssueId { get; set; }
        
        public string IssueLinkUrl { get; set; }
        
        public string Title { get; set; }
        
        public string Category { get; set; }

        public ReleaseUser User { get; set; }

        public string Number { get; set; }

        public string State { get; set; }
    }
}