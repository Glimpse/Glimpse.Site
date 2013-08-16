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
            var cachedRepository = new CacheIssueRepository(githubIssueRepository.Object,cacheProvider.Object);
            cacheProvider.Setup(c => c.Get("githubIssues")).Returns(null);

            cachedRepository.GetAllIssues();

            githubIssueRepository.Verify(g => g.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Open, RepoName = "glimpse/glimpse" }));
            githubIssueRepository.Verify(g => g.GetIssues(new GithubIssueQuery() { State = GithubIssueStatus.Closed, RepoName = "glimpse/glimpse", MilestoneNumber = 8 }));
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

    public class CacheProviderTests
    {
        [Fact]
        public void ShouldReturnNullForExpiredItems()
        {
            var cacheProvider = new CacheProvider(-1);
            cacheProvider.Add("key","test");

            var value = cacheProvider.Get("key");

            Assert.Null(value);
        }

        [Fact]
        public void ShouldReturnValueOfObjectForNonExpiredCacheItems()
        {
            var cacheProvider = new CacheProvider(1);
            cacheProvider.Add("key","test");

            var value = cacheProvider.Get("key");

            Assert.Equal(value, "test");
        }

            }

    public class GithubIssueQuery
    {
        public GithubIssueQuery()
        {
            State = GithubIssueStatus.Open;
        }

        public GithubIssueStatus State { get; set; }
        public string RepoName { get; set; }
        public int MilestoneNumber { get; set; }

        protected bool Equals(GithubIssueQuery other)
        {
            return string.Equals(State, other.State) && string.Equals(RepoName, other.RepoName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GithubIssueQuery) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((State != null ? State.GetHashCode() : 0)*397) ^ (RepoName != null ? RepoName.GetHashCode() : 0);
            }
        }

    }

    public class GithubIssueService
    {
        public virtual IEnumerable<GithubIssue> GetIssues(GithubIssueQuery issueQuery)
        {
            throw new System.NotImplementedException();
        }
    }
}
