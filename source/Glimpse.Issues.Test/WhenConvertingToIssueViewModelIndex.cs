using System.Linq;
using Glimpse.Site.Framework;
using Glimpse.Site.Models;
using Xunit;

namespace Glimpse.Issues.Test
{
    public class WhenConvertingToIssueViewModelIndex
    {
        [Fact]
        public void GivenTwoPackagesWithSameCategory_ShouldAddThemUnderSameCategory()
        {
            var package = new PackageBuilder()
                .WithTag("Tag1")
                .WithCategory("Category1")
                .Build();
            var package2 = new PackageBuilder()
                .WithTag("Tag2")
                .WithCategory("Category1")
                .Build();
            var mapper = new GlimpsePackageViewModelMapper();
           
            var indexViewModel = mapper.ConvertToIndexViewModel(new [] { package, package2});

            Assert.Equal(1, indexViewModel.PackageCategories.Count);
            Assert.Equal("Category1", indexViewModel.PackageCategories.First().Name);

        }

        [Fact]
        public void GivenAnOpenBugIssue_ShouldChangeStatusOfPackageToRed()
        {
            var package = new PackageBuilder()
                .WithTag("Tag1")
                .WithCategory("Category1")
                .Build();
            var issue = new IssueBuilder()
                .WithId("1")
                .WithLabel("Tag1")
                .WithLabel("Bug")
                .WithState("open")
                .Build();
            package.AddIssue(issue);
            var mapper = new GlimpsePackageViewModelMapper();

            var indexViewModel = mapper.ConvertToIndexViewModel(new[] {package});

            var packageViewModel = indexViewModel.PackageCategories[0].Packages[0];
            Assert.Equal(PackageStatus.Red, packageViewModel.Status);
        }

        [Fact]
        public void GivenIssueAcrossDifferentPackagesUnderSameCategory_ShouldOnlyAddIssueToCategory()
        {
            
            var package = new PackageBuilder()
                .WithTag("Tag1")
                .WithCategory("Category1")
                .Build();
            var package2 = new PackageBuilder()
                .WithTag("Tag2")
                .WithCategory("Category1")
                .Build();
            var issue = new IssueBuilder()
                .WithId("1")
                .WithLabel("Tag1")
                .WithLabel("Tag2")
                .WithState("open")
                .Build();
            package.AddIssue(issue);
            package2.AddIssue(issue);
            var mapper = new GlimpsePackageViewModelMapper();

            var indexViewModel = mapper.ConvertToIndexViewModel(new[] {package,package2});

            var packageViewModel = indexViewModel.PackageCategories[0];
            Assert.Equal(1, packageViewModel.AcknowledgedIssues.Count);
        }

        [Fact]
        public void ShouldAddOpenLabelledIssuesToAcknowledgedIssues()
        {
            var package = new PackageBuilder()
                .WithTag("Tag1")
                .WithCategory("Category1")
                .Build();
            var issue = new IssueBuilder()
                .WithId("1")
                .WithLabel("Tag1")
                .WithLabel("Tag2")
                .WithState("open")
                .Build();
            package.AddIssue(issue);
            var mapper = new GlimpsePackageViewModelMapper();

            var indexViewModel = mapper.ConvertToIndexViewModel(new[] {package});

            var packageViewModel = indexViewModel.PackageCategories[0];
            Assert.Equal(issue.Id, packageViewModel.AcknowledgedIssues.First().IssueId);
        }

        [Fact]
        public void ShouldAddClosedIssuesToCompletedIssues()
        {
            
            var package = new PackageBuilder()
                .WithTag("Tag1")
                .WithCategory("Category1")
                .Build();
            var issue = new IssueBuilder()
                .WithId("1")
                .WithLabel("Tag1")
                .WithLabel("Tag2")
                .WithState("closed")
                .Build();
            package.AddIssue(issue);
            var mapper = new GlimpsePackageViewModelMapper();

            var indexViewModel = mapper.ConvertToIndexViewModel(new[] {package});

            var packageViewModel = indexViewModel.PackageCategories[0];
            Assert.Equal(issue.Id, packageViewModel.CompletedIssues.First().IssueId);
        }
    }
}
