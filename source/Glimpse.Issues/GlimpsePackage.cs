using System.Collections.Generic;

namespace Glimpse.Issues
{
    public class GlimpsePackage
    {
        public GlimpsePackage()
        {
            Issues = new List<GithubIssue>();
            Tags = new List<string>();
        }
        public string Title { get; set; }
        public string Category { get; set; }
        public List<string> Tags { get; set; }
        public GlimpsePackageStatus Status { get; set; }
        public string StatusDescription { get; set; }
        public string CurrentRelease { get; set; }
        public string NextRelease { get; set; }
        public IList<GithubIssue> Issues { get; set; }

        public void AddIssue(GithubIssue issue)
        {
            if(!Issues.Contains(issue))
                Issues.Add(issue);
        }
    }

    public enum GlimpsePackageStatus
    {
        Green, Red
    }
}