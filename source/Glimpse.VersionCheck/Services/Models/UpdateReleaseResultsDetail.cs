using System;

namespace Glimpse.VersionCheck
{
    public class UpdateReleaseResultsDetail
    { 
        public bool UpdateWasForced { get; set; }

        public DateTime NextUpdateCanOccur { get; set; }

        public MergedReleaseDetails ReleaseDetails { get; set; }

        public MergedStatisticReleaseDetails StatisticReleaseDetails { get; set; }

        public DateTime TimeOccured { get; set; }
    }
}