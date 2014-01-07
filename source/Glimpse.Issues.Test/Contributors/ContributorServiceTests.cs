using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Glimpse.Infrastructure.Http;
using Glimpse.Infrastructure.Models;
using Glimpse.Infrastructure.Repositories;
using Glimpse.Infrastructure.Services;
using Moq;
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
            _githubContributorService.Setup(g => g.GetContributors("glimpse/glimpse")).Returns(new[] {new GithubContributor() {Name = "GlimpseCoreContributorName"}});
            _githubContributorService.Setup(g => g.GetContributors("glimpse/glimpse.client")).Returns(new[] {new GithubContributor() {Name = "ClientContributor"}});
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
            Assert.True(githubContributors.First().Name == "GlimpseCoreContributorName");
        }
    }
}
