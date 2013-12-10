using System.Collections.Generic;
using System.Linq;
using Glimpse.Infrastructure.GitHub;
using Xunit;

namespace Glimpse.Issues.Test.Contributors
{
    public class ContributorServiceTests
    {
        private ContributorService _contributorService;
        private IList<GlimpseContributor> _githubContributors;

        public ContributorServiceTests()
        {
            _contributorService = new ContributorService();
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
        public IEnumerable<GlimpseContributor> GetContributors()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GlimpseContributor
    {
        public string Name { get; set; }
        public string GithubUsername { get; set; }
        public string TwitterUsername { get; set; }
        public string Category { get; set; }
        public int TotalContributions { get; set; }

    public class GlimpseContribution
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfContributions { get; set; }
    }
}
