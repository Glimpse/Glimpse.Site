using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Package
{
    public class CheckReleaseDetails
    {
        public IDictionary<string, ReleaseDetailsSummaryInfo> Summary { get; set; }

        public IDictionary<string, ReleaseVersionData> Releases { get; set; }

        public string Channel { get; set; }

        public string Version { get; set; }

        public bool HasNewer { get; set; }

        public bool HasResult { get; set; }

        public int TotalNewerReleases { get; set; }

        public string ProductDescription { get; set; }

        public string ProductIconUrl { get; set; }
    }

    public static class CheckReleaseVersionDataExtension
    {
        public static ReleaseVersionData LastItemOrDefault(this IDictionary<string, ReleaseVersionData> value)
        {
            if (value == null || value.Count == 0)
                return null;

            return value.LastOrDefault().Value;
        }
    }
}