using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Glimpse.Issues.Test
{
    public class CacheIssueRepository
    {
        private readonly GithubIssueService _githubIssueService;
        private const string CacheKey = "githubIssues";
        private readonly CacheProvider _cacheProvider;

        public CacheIssueRepository(GithubIssueService githubIssueService, CacheProvider cacheProvider)
        {
            _githubIssueService = githubIssueService;
            _cacheProvider = cacheProvider;
        }

        public virtual IEnumerable<GithubIssue> GetAllIssues()
        {
            var cache = _cacheProvider.Get(CacheKey) as IEnumerable<GithubIssue>;
            if (cache != null)
                return cache;
            var githubIssues = RefreshCacheFromGithubService();
            return githubIssues;
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

    public class CacheProvider
    {
        private readonly int _minutesToExpire;
        private readonly Dictionary<string, GlimpseCacheItem> _cache = new Dictionary<string, GlimpseCacheItem>();

        public CacheProvider() :this(30)
        {
            
        }

        public CacheProvider(int minutesToExpire)
        {
            _minutesToExpire = minutesToExpire;
        }

        public virtual object Get(string key)
        {
            GlimpseCacheItem cacheItem;
            _cache.TryGetValue(key, out cacheItem);
            return cacheItem != null ? cacheItem.Value : null;
        }

        public void Add(string key, object value)
        {
            var glimpseCacheItem = new GlimpseCacheItem(value, DateTime.Now.AddMinutes(_minutesToExpire));
            if(!_cache.ContainsKey(key))
                _cache.Add(key, glimpseCacheItem);
            else
                _cache[key] = glimpseCacheItem;
        }
    }

    internal class GlimpseCacheItem
    {
        private readonly object _value;
        private readonly DateTime _expireTime;

        public GlimpseCacheItem(object value, DateTime expireTime)
        {
            _value = value;
            _expireTime = expireTime;
        }

        public object Value
        {
            get
            {
                return HasExpired() ? null : _value;
            }
        }

        protected internal virtual bool HasExpired()
        {
            return _expireTime <= DateTime.Now;
        }
    }
}