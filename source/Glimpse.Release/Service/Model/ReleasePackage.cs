using System.Collections.Generic;

namespace Glimpse.Release
{
    public class ReleasePackage
    {        
        public ReleasePackage()
        {
            AcknowledgedIssues = new List<ReleaseIssue>();
            CompletedIssues = new List<ReleaseIssue>();
            PackageItem = new List<ReleasePackageItem>();
        }

        public string Name { get; set; }

        public List<ReleasePackageItem> PackageItem { get; set; }

        public List<ReleaseIssue> AcknowledgedIssues { get; set; }

        public List<ReleaseIssue> CompletedIssues { get; set; }
    }
}