using Glimpse.Issues;

namespace Glimpse.Site.Models
{
    public class IssueViewModel
    {
        public string IssueId { get; set; }
        public string IssueLinkUrl { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public GithubUser User { get; set; }
        public string Number { get; set; }
    }
}