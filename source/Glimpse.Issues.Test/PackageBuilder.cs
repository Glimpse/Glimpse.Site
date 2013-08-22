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
    }
}