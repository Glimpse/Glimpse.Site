using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Issues.Test
{
    public class PackageIssueProvider
    {
        private readonly CachePackageRepository _cachePackageRepository;
        private readonly IIssueRepository _cacheIssueRepository;

        public PackageIssueProvider(CachePackageRepository cachePackageRepository, IIssueRepository cacheIssueRepository)
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
                AddIssueToAssociatedPackage(packages, issue);
            }
            return packages;
        }

        private void AddIssueToAssociatedPackage(IEnumerable<GlimpsePackage> packages, GithubIssue issue)
        {
            foreach (var package in packages.Where(package => issue.Body.Contains(package.Tag)))
            {
                package.Issues.Add(issue);
            }
        }
    }
}