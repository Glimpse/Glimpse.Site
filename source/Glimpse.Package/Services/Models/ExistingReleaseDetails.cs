using System.Collections.Generic;

namespace Glimpse.Package
{
    public class ExistingReleaseDetails
    {
        public IDictionary<string, LatestReleaseDetailsSummaryInfo> Summary { get; set; }

        public ReleaseVersionData Release { get; set; }

        public string Channel { get; set; }

        public string Version { get; set; }

        public bool HasNewer { get; set; }

        public bool HasResult { get; set; }

        public int TotalNewerReleases { get; set; } 
    }
}