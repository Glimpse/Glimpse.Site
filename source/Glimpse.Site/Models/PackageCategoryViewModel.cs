using System.Collections.Generic;

namespace Glimpse.Site.Models
{
    public class PackageCategoryViewModel
    {
        public PackageCategoryViewModel()
        {
            AcknowledgedIssues = new List<IssueViewModel>();
            CompletedIssues = new List<IssueViewModel>();
            Packages = new List<PackageViewModel>();
        }
        public string Name { get; set; }
        public string CurrentRelease { get; set; }
        public string NextRelease { get; set; }
        public List<PackageViewModel> Packages { get; set; }
        public List<IssueViewModel> AcknowledgedIssues { get; set; }
        public List<IssueViewModel> CompletedIssues { get; set; }
    }
}