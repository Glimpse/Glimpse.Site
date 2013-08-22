namespace Glimpse.Site.Models
{
    public class PackageViewModel
    {
        public string Name { get; set; }
        public PackageStatus Status { get; set; }

        public string StatusImage
        {
            get
            {
                return Status == PackageStatus.Red ? "redIcon.png" : "greenIcon.png";
            }
        }

        public string StatusDescription { get; set; }
    }
}