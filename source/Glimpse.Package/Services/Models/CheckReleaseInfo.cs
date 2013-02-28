using System.Collections.Generic;

namespace Glimpse.Package
{
    public class CheckReleaseInfo
    {
        public bool HasNewer { get; set; }

        public string ViewLink { get; set; }

        public IDictionary<string, CheckReleaseDetails> Details { get; set; }
    }
}