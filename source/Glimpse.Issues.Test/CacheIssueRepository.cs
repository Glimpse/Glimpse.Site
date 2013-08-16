using System;
using System.Collections.Generic;

namespace Glimpse.Issues.Test
{
    public class CacheIssueRepository : IIssueRepository
    {
        private readonly GithubIssueService _githubIssueService;
        private const string CacheKey = "githubIssues";
        private readonly CacheProvider _cacheProvider;

        public CacheIssueRepository(GithubIssueService githubIssueService, CacheProvider cacheProvider)
        {
            _githubIssueService = githubIssueService;
            _cacheProvider = cacheProvider;
        }

        public IEnumerable<GithubIssue> GetAllIssues()
        {
            var cache = _cacheProvider.Get(CacheKey) as IEnumerable<GithubIssue>;
            if (cache != null)
                return cache;
            return RefreshCacheFromGithubService();
        }

        private IEnumerable<GithubIssue> RefreshCacheFromGithubService()
        {
            var githubIssues = new List<GithubIssue>();
            githubIssues.AddRange( _githubIssueService.GetIssues(new GithubIssueQuery() { RepoName = "glimpse/glimpse", State = GithubIssueStatus.Open }));
            githubIssues.AddRange( _githubIssueService.GetIssues(new GithubIssueQuery() { RepoName = "glimpse/glimpse", State = GithubIssueStatus.Closed, MilestoneNumber = 8 }));
            _cacheProvider.Add(CacheKey, githubIssues);
            return githubIssues;
        }
    }

}