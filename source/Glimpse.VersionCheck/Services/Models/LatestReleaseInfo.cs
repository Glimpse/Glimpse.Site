using System.Collections.Generic;

namespace Glimpse.VersionCheck
{
    public class LatestReleaseInfo
    {
        public bool HasNewer { get; set; }

        public string ViewLink { get; set; }

        public IDictionary<string, LatestReleaseDetails> Details { get; set; }
    }
}