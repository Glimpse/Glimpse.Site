using System.Collections.Generic;

namespace Glimpse.VersionCheck
{
    public class UpdateReleaseRepositoryResults
    {
        public MergedReleaseDetails ReleaseDetails { get; set; }

        public MergedStatisticReleaseDetails StatisticReleaseDetails { get; set; }

        public IEnumerable<ReleasePersistencyItem> Results { get; set; }
    }
}