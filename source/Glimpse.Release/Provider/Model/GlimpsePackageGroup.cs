using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Release
{
    public class GlimpsePackageGroup
    {
        public GlimpsePackageGroup()
        {
            Tags = new List<string>();
            Packages = new List<GlimpsePackage>();
        }

        public GlimpsePackageStatus Status
        {
            get { return Packages.Any(x => x.Status == GlimpsePackageStatus.Red) ? GlimpsePackageStatus.Red : GlimpsePackageStatus.Green; }
        }

        public List<GlimpsePackage> Packages { get; set; }

        public List<string> Tags { get; set; }

        public string Name { get; set; }
    }
}