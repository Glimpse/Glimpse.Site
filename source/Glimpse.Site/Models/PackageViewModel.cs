using Glimpse.Issues;

namespace Glimpse.Site.Models
{
    public class PackageViewModel
    {
        public string Name { get; set; }
        public GlimpsePackageStatus Status { get; set; }

        public string StatusImage
        {
            get
            {
                return Status == GlimpsePackageStatus.Red ? "redIcon.png" : "greenIcon.png";
            }
        }

        public string StatusDescription { get; set; }
    }
}