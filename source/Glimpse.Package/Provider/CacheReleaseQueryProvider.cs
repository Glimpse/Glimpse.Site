using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Package
{
    public class CacheReleaseQueryProvider : IReleaseQueryProvider
    {
        private IDictionary<string, IList<string>> _packageAuthors;
        private IDictionary<string, IEnumerable<ReleaseQueryItem>> _releaseCache;

        public IDictionary<string, IEnumerable<ReleaseQueryItem>> SelectAllPackages()
        {
            if (_releaseCache == null)
                throw new NullReferenceException("Cache has not been defined");
            return _releaseCache;
        }

        public IDictionary<string, IList<string>> SelectAllPackageAuthors()
        {
            return _packageAuthors ?? (_packageAuthors = InnerSelectAllPackageAuthors());
        }

        public IEnumerable<ReleaseQueryItem> SelectPackage(string packageName)
        {
            return SelectAllPackages().GetValueOrDefault(packageName);
        }

        public ReleaseQueryItem LatestPackageRelease(string packageName)
        {
            var releases = SelectPackage(packageName);
            return releases != null ? releases.FirstOrDefault(x => x.IsLatestVersion) : null;
        }

        public ReleaseQueryItem LatestPackageAbsoluteRelease(string packageName)
        {
            var releases = SelectPackage(packageName);
            return releases != null ? releases.FirstOrDefault(x => x.IsAbsoluteLatestVersion) : null;
        }

        public ReleaseQueryItem SelectRelease(string packageName, string version)
        {
            var releases = SelectPackage(packageName);
            return releases != null ? releases.FirstOrDefault(x => String.Compare(x.Version, version, StringComparison.OrdinalIgnoreCase) == 0) : null;
        }

        public IEnumerable<ReleaseQueryItem> FindReleasesAfter(string packageName, string version)
        {
            var release = SelectRelease(packageName, version);

            var releases = SelectPackage(packageName);
            return release != null && releases != null ? releases.Where(x => x.Created > release.Created) : new List<ReleaseQueryItem>();
        }

        public void UpdateCache(IDictionary<string, IEnumerable<ReleaseQueryItem>> releaseCache)
        {
            if (releaseCache == null)
                throw new ArgumentNullException("releaseCache");

            var keys = releaseCache.Keys.ToList();
            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var value = releaseCache[key];
                if (value != null)
                {
                    releaseCache[key] = value.OrderBy(x => x.Created);
                }
            }

            _releaseCache = releaseCache;
            _packageAuthors = null;
        }

        private IDictionary<string, IList<string>> InnerSelectAllPackageAuthors()
        {
            var data = new Dictionary<string, IList<string>>();

            var packages = SelectAllPackages();
            foreach (var packageVersions in packages)
            {
                var package = packageVersions.Value.FirstOrDefault();
                if (package != null && !string.IsNullOrEmpty(package.Authors))
                {
                    var authors = package.Authors.Split(new [] {", "}, StringSplitOptions.None);
                    foreach (var author in authors)
                    {
                        var key = author.Trim();
                        var authorPackages = (IList<string>)null;

                        if (data.TryGetValue(key, out authorPackages))
                            authorPackages.Add(package.Name);
                        else
                            data.Add(key, new List<string> { package.Name }); 
                    }
                }
            }

            data = data.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);

            return data;
        }
    }
}