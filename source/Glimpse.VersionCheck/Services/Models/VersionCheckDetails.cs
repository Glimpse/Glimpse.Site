using System.Collections.Generic;

namespace Glimpse.VersionCheck
{
    public class VersionCheckDetails
    {
        public IEnumerable<VersionCheckDetailsItem> Packages { get; set; }

        public string Stamp { get; set; }
    }
}