using System.Collections.Generic;

namespace Glimpse.Package
{
    public class VersionCheckDetails
    {
        public IEnumerable<VersionCheckDetailsItem> Packages { get; set; }

        public string Stamp { get; set; }
    }
}