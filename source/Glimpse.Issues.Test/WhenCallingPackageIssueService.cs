using System;
using System.Dynamic;
using Moq;
using Xunit;

namespace Glimpse.Issues.Test
{
    public class WhenCallingPackageIssueService
    {
        private Mock<IIssueRepository> _issueRepository;
        private Mock<IPackageRepository> _packageRepository;
        private PackageIssueProvider _issueService;
        private PackageBuilder _packageBuilder;
        private IssueBuilder _issueBuilder;
        private GlimpsePackage _glimpseCorePackage;

        public WhenCallingPackageIssueService()
        {
            _packageRepository = new Mock<IPackageRepository>();
            _issueRepository = new Mock<IIssueRepository>();
            _issueService = new PackageIssueProvider(_packageRepository.Object, _issueRepository.Object);
            _issueBuilder = new IssueBuilder();
            _packageBuilder = new PackageBuilder();
            _glimpseCorePackage = _packageBuilder
                .WithTag("Glimpse Core")
                .Build();
            StubPackages(_glimpseCorePackage);
        }

        [Fact]
        public void ShouldAddOpenTaggedIssueToTheCorrectPackage()
        {
            var issueId = Guid.NewGuid().ToString();
            var glimpeCoreOpenIssue = _issueBuilder
                .WithLabel("Glimpse Core")
                .WithState("open")
                .WithId(issueId)
                .Build();
            StubGithubIssues(glimpeCoreOpenIssue);

            _issueService.GetPackageIssues();

            Assert.True(_glimpseCorePackage.Issues.Contains(glimpeCoreOpenIssue));
        }

        [Fact]
        public void ShouldAddClosedTaggedIssueToTheCorrectPackage()
        {
            var issueId = Guid.NewGuid().ToString();
            var glimpeCoreOpenIssue = _issueBuilder
                .WithLabel("Glimpse Core")
                .WithState("closed")
                .WithMilestone("vnext")
                .WithId(issueId)
                .Build();
            StubGithubIssues(glimpeCoreOpenIssue);

            _issueService.GetPackageIssues();

            Assert.True(_glimpseCorePackage.Issues.Contains(glimpeCoreOpenIssue));
        }


        private void StubPackages(params GlimpsePackage[] glimpsePackages)
        {
            _packageRepository.Setup(p => p.GetAllPackages()).Returns(glimpsePackages);
        }

        private void StubGithubIssues(params GithubIssue[] issues)
        {
            _issueRepository.Setup(i => i.GetAllIssues()).Returns(issues);
        }
    }
}

