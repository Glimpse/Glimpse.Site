using System.Collections.Generic;

namespace Glimpse.Issues.Test
{
    public class GlimpsePackage
    {
        public GlimpsePackage()
        {
            Issues = new List<GithubIssue>();
        }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
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