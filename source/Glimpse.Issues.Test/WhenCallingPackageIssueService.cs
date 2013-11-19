using System;
using System.Dynamic;
using Moq;
using Xunit;

namespace Glimpse.Issues.Test
{
    public class WhenCallingPackageIssueService
    {
        private const int VNextMilestoneNumber = 8;
        private Mock<IIssueRepository> _issueRepository;
        private Mock<IPackageRepository> _packageRepository;
        private Mock<IGithubMilestoneService> _milestoneService;
        private PackageIssueProvider _issueService;
        private PackageBuilder _packageBuilder;
        private IssueBuilder _issueBuilder;
        private GlimpsePackage _glimpseCorePackage;

        public WhenCallingPackageIssueService()
        {
            _packageRepository = new Mock<IPackageRepository>();
            _issueRepository = new Mock<IIssueRepository>();
            _milestoneService = new Mock<IGithubMilestoneService>();
            _issueService = new PackageIssueProvider(_packageRepository.Object, _issueRepository.Object, _milestoneService.Object);
            _issueBuilder = new IssueBuilder();
            _packageBuilder = new PackageBuilder();
            _glimpseCorePackage = _packageBuilder
                .WithTag("Glimpse Core")
                .Build();
            StubPackages(_glimpseCorePackage);
        }

        [Fact]
        public void ShouldUseLatestMilestoneWhichHasOpenOrClosedIssues()
        {
            var issueId = Guid.NewGuid().ToString();
            var glimpsePreviousReleaseIssue = _issueBuilder
                .WithLabel("Glimpse Core")
                .WithState("open")
                .WithId(issueId)
                .Build();
            StubGithubIssues(glimpsePreviousReleaseIssue);
            int firstVersionMilestoneNumber = 1;
            int secondVersionMilestoneNumber = 2;
            _milestoneService.Setup(m => m.GetMilestones())
                             .Returns(new[]
                                          {
                                              new GithubMilestone() {Number = VNextMilestoneNumber,Created_At = new DateTime(2013,4,2),Open_Issues = 0, Closed_Issues = 0, Title = "vnext"},
                                               new GithubMilestone()
                                                  {
                                                      Number = firstVersionMilestoneNumber,
                                                      Title = "1.7.0",
                                                      Closed_Issues = 4,
                                                      Created_At = new DateTime(2013, 1, 2)
                                                  },
                                              new GithubMilestone()
                                                  {
                                                      Number = secondVersionMilestoneNumber,
                                                      Title = "1.8.0",
                                                      Closed_Issues = 4,
                                                      Created_At = new DateTime(2013, 3, 2)
                                                  }
                                          });
            _issueService.GetLatestPackageIssues();

            _issueRepository.Verify(i => i.GetAllIssuesFromMilestone(secondVersionMilestoneNumber));
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

            _issueService.GetLatestPackageIssues();

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

            _issueService.GetLatestPackageIssues();

            Assert.True(_glimpseCorePackage.Issues.Contains(glimpeCoreOpenIssue));
        }


        private void StubPackages(params GlimpsePackage[] glimpsePackages)
        {
            _packageRepository.Setup(p => p.GetAllPackages()).Returns(glimpsePackages);
        }

        private void StubGithubIssues(params GithubIssue[] issues)
        {
            _issueRepository.Setup(i => i.GetAllIssuesFromMilestone(VNextMilestoneNumber)).Returns(issues);
        }
    }
}

