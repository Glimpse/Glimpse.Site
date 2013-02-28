using System.Collections.Generic;

namespace Glimpse.Package
{
    public class ReleaseQueryDetails
    {
        public IDictionary<string, ReleaseQuerySummaryInfo> Summary { get; set; }

        public IDictionary<string, ReleaseQueryVersionData> AvailableReleases { get; set; }

        public IDictionary<string, ReleaseQueryVersionData> RequestedReleases { get; set; }

        public ReleaseQueryVersionData Release { get; set; }

        public string Channel { get; set; }

        public string Version { get; set; }

        public bool HasNewer { get; set; }

        public bool HasResult { get; set; }

        public int TotalNewerReleases { get; set; }

        public string PackageDescription { get; set; }

        public string PackageIconUrl { get; set; }
    }
}