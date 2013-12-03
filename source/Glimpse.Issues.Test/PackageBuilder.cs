using Glimpse.Infrastructure;

namespace Glimpse.Issues.Test
{
    public class PackageBuilder
    {
        private readonly GlimpsePackage _glimpsePackage = new GlimpsePackage();
        public PackageBuilder WithTag(string tag)
        {
            _glimpsePackage.Tags.Add(tag);
            return this;
        }

        public GlimpsePackage Build()
        {
            return _glimpsePackage;
        }

        public PackageBuilder WithCategory(string category)
        {
            _glimpsePackage.Category = category;
            return this;
        }

        public PackageBuilder WithStatus(GlimpsePackageStatus status)
        {
            _glimpsePackage.Status = status;
            return this;
        }

        public PackageBuilder WithStatusDescription(string statusDescription)
        {
            _glimpsePackage.StatusDescription = statusDescription;
            return this;
        }
    }
}