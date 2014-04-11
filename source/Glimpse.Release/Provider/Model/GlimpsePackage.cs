using System.Collections.Generic;

namespace Glimpse.Release
{
    public class GlimpsePackage
    {
        public GlimpsePackage()
        {
            Tags = new List<string>();
        }

        public string Title { get; set; }

        public string Category { get; set; }

        public List<string> Tags { get; set; }

        public GlimpsePackageStatus Status { get; set; }

        public string StatusDescription { get; set; }
    }
}