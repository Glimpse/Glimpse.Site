using System.Collections.Generic;

namespace Glimpse.Release
{
    public interface IPackageProvider
    {
        IList<GlimpsePackage> GetAllPackages();

        IList<string> GetAllPackagesTags();

        IDictionary<string, GlimpsePackageGroup> GetAllPackagesGroupedByCategory();
    }
}