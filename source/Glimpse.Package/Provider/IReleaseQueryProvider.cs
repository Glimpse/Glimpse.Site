using System.Collections.Generic;

namespace Glimpse.Package
{
    public interface IReleaseQueryProvider
    {
        IDictionary<string, IEnumerable<ReleaseQueryItem>> SelectAllPackages();

        IEnumerable<ReleaseQueryItem> SelectPackage(string packageName);

        ReleaseQueryItem LatestPackageRelease(string packageName);

        ReleaseQueryItem LatestPackageAbsoluteRelease(string packageName);

        ReleaseQueryItem SelectRelease(string packageName, string version);

        IEnumerable<ReleaseQueryItem> FindReleasesAfter(string packageName, string version);

        IDictionary<string, IList<string>> SelectAllPackageAuthors();

        void UpdateCache(IDictionary<string, IEnumerable<ReleaseQueryItem>> releaseCache);
    }
}