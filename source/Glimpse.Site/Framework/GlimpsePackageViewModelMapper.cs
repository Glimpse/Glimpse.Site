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
                var packageView = new PackageViewModel
                                  {
                                      Name = glimpsePackage.Title,
                                      Status = glimpsePackage.Status,
                                      StatusDescription = glimpsePackage.StatusDescription
                                  };
                packageCategory.Packages.Add(packageView);
            }
            return issuesView;
        }

        private void AddIssuesToViewModel(GlimpsePackage packageIssue, PackageCategoryViewModel packageCategory)
        {
            var openIssues = packageIssue.Issues.Where(p => p.Status == GithubIssueStatus.Open).ToList();
            var closedIssues = packageIssue.Issues.Where(p => p.Status == GithubIssueStatus.Closed).ToList();
            AddToIssueViewModel(openIssues, packageIssue.Tags, packageCategory.AcknowledgedIssues);
            AddToIssueViewModel(closedIssues, packageIssue.Tags,packageCategory.CompletedIssues);
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

        private void AddToIssueViewModel(List<GithubIssue> openIssues, List<string> packageTags, List<IssueViewModel> issueViewModels)
        {
            foreach (var openIssue in openIssues)
            {
                var issueView = ConvertToIssueViewModel(openIssue,packageTags);
                if (issueViewModels.Count(p => p.IssueId == openIssue.Id) == 0)
                    issueViewModels.Add(issueView);
            }
        }

        private IssueViewModel ConvertToIssueViewModel(GithubIssue openIssue, List<string> packageTags)
        {
            var issueView = new IssueViewModel();
            issueView.IssueId = openIssue.Id;
            issueView.IssueLinkUrl = openIssue.Html_Url;
            foreach (var label in openIssue.Labels)
            {
                if(!packageTags.Contains(label.Name))
                    issueView.Category += label.Name + ",";
            }
            RemoveLastComma(issueView);
            issueView.Description = openIssue.Title;
            return issueView;
        }

        private static void RemoveLastComma(IssueViewModel issueView)
        {
            if (!string.IsNullOrEmpty(issueView.Category))
            {
                var lastComma = issueView.Category.Length - 1;
                issueView.Category = issueView.Category.Substring(0, lastComma);
            }
        }
    }
}