using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Glimpse.Infrastructure.Test.Contributors
{
    public class ContributorServiceTests
    {
        private readonly ContributorService _contributorService;
        private readonly IList<GlimpseContributor> _githubContributors;
        private readonly Mock<IGithubContributerService> _githubContributorService;

        public ContributorServiceTests()
        {
            _githubContributorService = new Mock<IGithubContributerService>();
            _githubContributorService.Setup(g => g.GetContributors("Glimpse.Glimpse")).Returns(new[] {new GithubContributor() {Name = "GlimpseCoreContributorName"}});
            _githubContributorService.Setup(g => g.GetContributors("Glimpse.Client")).Returns(new[] {new GithubContributor() {Name = "ClientContributor"}});
            string teamMemberJsonFile = Path.Combine(Environment.CurrentDirectory, "Contributors", "testTeamMembers.json");
            _contributorService = new ContributorService(new GlimpseTeamMemberRepository(teamMemberJsonFile), _githubContributorService.Object);
            _githubContributors = _contributorService.GetContributors().ToList();
        }

        [Fact]
        public void ShouldReturnAllReviewers()
        {
            var reviewers =_githubContributors.Where(a => a.Category == "Reviewer").ToList();
            Assert.True(reviewers.First().Name == "Reviewer Name");
        }

        [Fact]
        public void ShouldReturnAllCommitters()
        {
            var committers = _githubContributors.Where(a => a.Category == "Committer").ToList();
            Assert.True(committers.First().Name == "Committer Name");
        }

        [Fact]
        public void ShouldReturnAllGithubContributors()
        {
            var githubContributors = _githubContributors.Where(a => a.Category == "Contributor").ToList();
            Assert.True(githubContributors.First().Name == "ContributorName");
        }
    }

    public class ContributorService
    {
        private readonly GlimpseTeamMemberRepository _teamMemberRepository;
        private readonly IGithubContributerService _githubContributerService;

        public ContributorService(GlimpseTeamMemberRepository teamMemberRepository, IGithubContributerService githubContributerService)
        {
            _teamMemberRepository = teamMemberRepository;
            _githubContributerService = githubContributerService;
        }

        public IEnumerable<GlimpseContributor> GetContributors()
        {
            //get reviwers and committers
            return _teamMemberRepository.GetMembers();
        }
    }

    public interface IGithubContributerService
    {
        IEnumerable<GithubContributor> GetContributors(string githubRepoName);
    }

    public class GithubContributor
    {
        public string Name { get; set; }
    }

    public class GlimpseTeamMemberRepository
    {
        private string _teamMemberJsonFile;

        public GlimpseTeamMemberRepository(string teamMemberJsonFile)
        {
            _teamMemberJsonFile = teamMemberJsonFile;
        }

        public IEnumerable<GlimpseContributor> GetMembers()
        {
            var teamFileContent = File.ReadAllText(_teamMemberJsonFile);
            var teamContributors = JsonConvert.DeserializeObject<IEnumerable<GlimpseContributor>>(teamFileContent);
            return teamContributors;
        }
    }

    public class GlimpseContributor
    {
        public string Name { get; set; }
        public string GithubUsername { get; set; }
        public string TwitterUsername { get; set; }
        public string Category { get; set; }
        public int TotalContributions { get; set; }
    }
}
