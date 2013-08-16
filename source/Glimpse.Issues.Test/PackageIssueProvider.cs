using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Issues.Test
{
    public class PackageIssueProvider
    {
        private readonly CachePackageRepository _cachePackageRepository;
        private readonly CacheIssueRepository _cacheIssueRepository;

        public PackageIssueProvider(CachePackageRepository cachePackageRepository, CacheIssueRepository cacheIssueRepository)
        {
            _cachePackageRepository = cachePackageRepository;
            _cacheIssueRepository = cacheIssueRepository;
        }

        public IEnumerable<GlimpsePackage> GetPackageIssues()
        {
            var issues = _cacheIssueRepository.GetAllIssues();
            var packages = _cachePackageRepository.GetAllPackages().ToList();
            foreach (var issue in issues)
            {
                foreach (var package in packages)
                {
                    if (issue.Body.Contains(package.Tag))
                        package.Issues.Add(issue);
                }
            }
            return null;
            ;
        }

    }
}