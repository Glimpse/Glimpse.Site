using System.Collections.Generic;
using System.Linq;
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
            var reviewers =_githubContributors.Where(a => a.Contribution == "Reviewer").ToList();
            Assert.True(reviewers.Count ==2);
        }

        [Fact]
        public void ShouldReturnAllCommitters()
        {
            var committers = _githubContributors.Where(a => a.Contribution == "Committer").ToList();
            Assert.True(committers.Count == 2);
        }

        [Fact]
        public void ShouldReturnAllGithubContributors()
        {
            
        }

        [Fact]
        public void ShouldReturnTheTopNContributors()
        {
            
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
        public string Contribution { get; set; }
        
    }
}
