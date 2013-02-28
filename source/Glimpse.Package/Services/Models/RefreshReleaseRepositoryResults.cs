using System.Collections.Generic;

namespace Glimpse.Package
{
    public class RefreshReleaseRepositoryResults
    {
        public MergedReleaseDetails ReleaseDetails { get; set; }

        public MergedStatisticReleaseDetails StatisticReleaseDetails { get; set; }

        public IEnumerable<ReleasePersistencyItem> Results { get; set; }
    }
}