using System.Collections.Generic;
using System.Linq;
using Glimpse.Issues;
using Glimpse.Site.Models;

namespace Glimpse.Site.Framework
{
    public class GlimpsePackageViewModelMapper
    {
        public IssuesIndexViewModel ConvertToIndexViewModel(IEnumerable<GlimpsePackage> glimpsePackages)
        {
            var issuesView = new IssuesIndexViewModel();
            foreach (var glimpsePackage in glimpsePackages)
            {
                var packageCategory = GetOrCreatePackageCategoryViewModel(issuesView, glimpsePackage);
                AddIssuesToViewModel(glimpsePackage, packageCategory);
                var packageView = new PackageViewModel {Name = glimpsePackage.Title};
                packageView.Status = glimpsePackage.Issues.Count(i => i.Status == GithubIssueStatus.Open && i.Labels.Count(l => l.Name == "Bug") > 0) > 0
                    ? PackageStatus.Red
                    : PackageStatus.Green;
                packageCategory.Packages.Add(packageView);
            }
            return issuesView;
        }

        private void AddIssuesToViewModel(GlimpsePackage packageIssue, PackageCategoryViewModel packageCategory)
        {
            var openIssues = packageIssue.Issues.Where(p => p.Status == GithubIssueStatus.Open).ToList();
            var closedIssues = packageIssue.Issues.Where(p => p.Status == GithubIssueStatus.Closed).ToList();
            AddToIssueViewModel(openIssues, packageCategory, packageCategory.AcknowledgedIssues);
            AddToIssueViewModel(closedIssues, packageCategory,packageCategory.CompletedIssues);
        }

        private PackageCategoryViewModel GetOrCreatePackageCategoryViewModel(IssuesIndexViewModel issuesView, GlimpsePackage packageIssue)
        {
            var packageCategory = issuesView.PackageCategories.FirstOrDefault(c => c.Name == packageIssue.Category);
            if (packageCategory == null)
            {
                packageCategory = new PackageCategoryViewModel {Name = packageIssue.Category};
                issuesView.PackageCategories.Add(packageCategory);
            }
            return packageCategory;
        }

        private void AddToIssueViewModel(List<GithubIssue> openIssues, PackageCategoryViewModel packageCategory, List<IssueViewModel> issueViewModels)
        {
            foreach (var openIssue in openIssues)
            {
                var issueView = ConvertToIssueViewModel(openIssue);
                if (issueViewModels.Count(p => p.IssueId == openIssue.Id) == 0)
                    issueViewModels.Add(issueView);
            }
        }

        private IssueViewModel ConvertToIssueViewModel(GithubIssue openIssue)
        {
            var issueView = new IssueViewModel();
            issueView.IssueId = openIssue.Id;
            issueView.IssueLinkUrl = openIssue.Html_Url;
            foreach (var label in openIssue.Labels)
                issueView.Category += label.Name + ",";
            issueView.Description = openIssue.Title;
            return issueView;
        }
    }
}