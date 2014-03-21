using System.Collections.Generic;

namespace Glimpse.Infrastructure.Repositories
{
    public interface IPackageRepository
    {
        IEnumerable<GlimpsePackage> GetAllPackages();
    }
}