using System.Collections.Generic;

namespace Glimpse.Issues
{
    public interface IPackageRepository
    {
        IEnumerable<GlimpsePackage> GetAllPackages();
    }
}