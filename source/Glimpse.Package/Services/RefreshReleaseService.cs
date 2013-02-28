using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Glimpse.Package
{
    public class RefreshReleaseService : IRefreshReleaseService
    { 
        private readonly ISettings _settings;
        private readonly IRefreshReleaseRepositoryService _refreshRepositoryService;
        private readonly IReleaseQueryProvider _queryProvider;
        private readonly object _lock = new object();
        private DateTime _nextUpdate;
        private RefreshReleaseResultsDetail _lastDetails;

        public RefreshReleaseService(ISettings settings, IRefreshReleaseRepositoryService refreshReleaseRepositoryService, IReleaseQueryProvider queryProvider)
        {
            _settings = settings;
            _refreshRepositoryService = refreshReleaseRepositoryService;
            _queryProvider = queryProvider;
        }

        public RefreshReleaseResults Execute()
        {
            return Execute(false);
        }

        public RefreshReleaseResults Execute(bool force)
        {
            var results = new RefreshReleaseResults();
            results.LastRefresh = _lastDetails;

            if (force || DateTime.Now > _nextUpdate)
            {
                lock (_lock)
                {
                    if (force || DateTime.Now > _nextUpdate)
                    {
                        // Trigger the repository to update the database
                        var repositoryResults = _refreshRepositoryService.Execute();

                        // Transform the data into a format that the cache is expecting
                        var groupedResult = repositoryResults.Results.GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToDictionary(g => g.Key, g => g.Select(x => new ReleaseQueryItem { Created = x.Created, IsAbsoluteLatestVersion = x.IsAbsoluteLatestVersion, IsLatestVersion = x.IsLatestVersion, IsPrerelease = x.IsPrerelease, Name = x.Name, ReleaseNotes = x.ReleaseNotes, Version = x.Version, Description = x.Description, IconUrl = x.IconUrl }), StringComparer.OrdinalIgnoreCase);
                        _queryProvider.UpdateCache(groupedResult);

                        // Setup when we can update again
                        _nextUpdate = DateTime.Now.AddMilliseconds(_settings.MinServiceTriggerInterval);

                        // Copy over results
                        var details = new RefreshReleaseResultsDetail();
                        details.ReleaseDetails = repositoryResults.ReleaseDetails;
                        details.StatisticReleaseDetails = repositoryResults.StatisticReleaseDetails;
                        details.TimeOccured = DateTime.Now;
                        details.UpdateWasForced = force;
                        details.NextUpdateCanOccur = _nextUpdate;

                        results.UpdateOccured = true;
                        results.LastRefresh = details;

                        _lastDetails = details;
                    }
                }
            } 

            return results;
        }
    }
}