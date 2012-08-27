using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Glimpse.VersionCheck
{
    public class UpdateReleaseService : IUpdateReleaseService
    { 
        private readonly ISettings _settings;
        private readonly IUpdateReleaseRepositoryService _updateeRepositoryService;
        private readonly IReleaseQueryProvider _queryProvider;
        private readonly object _lock = new object();
        private DateTime _nextUpdate;
        private UpdateReleaseResultsDetail _lastDetails;

        public UpdateReleaseService(ISettings settings, IUpdateReleaseRepositoryService updateeRepositoryService, IReleaseQueryProvider queryProvider)
        {
            _settings = settings;
            _updateeRepositoryService = updateeRepositoryService;
            _queryProvider = queryProvider;
        }

        public UpdateReleaseResults Execute()
        {
            return Execute(false);
        }

        public UpdateReleaseResults Execute(bool force)
        {
            var results = new UpdateReleaseResults();
            results.LastUpdate = _lastDetails;

            if (force || DateTime.Now > _nextUpdate)
            {
                lock (_lock)
                {
                    if (force || DateTime.Now > _nextUpdate)
                    {
                        // Trigger the repository to update the database
                        var repositoryResults = _updateeRepositoryService.Execute();

                        // Transform the data into a format that the cache is expecting
                        var groupedResult = repositoryResults.Results.GroupBy(x => x.Name).ToDictionary(g => g.Key, g => g.Select(x => new ReleaseQueryItem { Created = x.Created, IsAbsoluteLatestVersion = x.IsAbsoluteLatestVersion, IsLatestVersion = x.IsLatestVersion, IsPrerelease = x.IsPrerelease, Name = x.Name, ReleaseNotes = x.ReleaseNotes, Version = x.Version, Description = x.Description, IconUrl = x.IconUrl }));
                        _queryProvider.UpdateCache(groupedResult);

                        // Setup when we can update again
                        _nextUpdate = DateTime.Now.AddMilliseconds(_settings.MinServiceTriggerInterval);

                        // Copy over results
                        var details = new UpdateReleaseResultsDetail();
                        details.ReleaseDetails = repositoryResults.ReleaseDetails;
                        details.StatisticReleaseDetails = repositoryResults.StatisticReleaseDetails;
                        details.TimeOccured = DateTime.Now;
                        details.UpdateWasForced = force;
                        details.NextUpdateCanOccur = _nextUpdate;

                        results.UpdateOccured = true;
                        results.LastUpdate = details;

                        _lastDetails = details;
                    }
                }
            } 

            return results;
        }
    }
}