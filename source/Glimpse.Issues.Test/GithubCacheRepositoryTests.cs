using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Glimpse.Issues.Test
{
    public class GithubCacheRepositoryTests
    {
        [Fact]
        public void GivenExpiredCachedIssues_ShouldMakeCallToGithubApi()
        {
            var githubIssueRepository = new Mock<GithubIssueService>();
            var cacheProvider = new Mock<CacheProvider>();
            var cachedRepository = new CacheIssueRepository(githubIssueRepository.Object, cacheProvider.Object);
            cacheProvider.Setup(c => c.Get("githubIssues")).Returns(null);

            cachedRepository.GetAllIssues();

            githubIssueRepository.Verify( g => g.GetIssues(new GithubIssueQuery() {State = GithubIssueStatus.Open, RepoName = "glimpse/glimpse"})); 
            githubIssueRepository.Verify( g => g.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Closed, RepoName = "glimpse/glimpse", MilestoneNumber = 8 }));
        }

        [Fact]
        public void GivenNonExpiredCachedIssues_ShouldReturnCachedItems()
        {
            var githubIssueRepository = new Mock<GithubIssueService>();
            var cacheProvider = new Mock<CacheProvider>();
            var cachedRepository = new CacheIssueRepository(githubIssueRepository.Object, cacheProvider.Object);
            object cachedItems = new List<GithubIssue>();
            cacheProvider.Setup(c => c.Get("githubIssues")).Returns(cachedItems);

            var issues = cachedRepository.GetAllIssues();

            Assert.Equal(issues, cachedItems);
        }

    }
}
