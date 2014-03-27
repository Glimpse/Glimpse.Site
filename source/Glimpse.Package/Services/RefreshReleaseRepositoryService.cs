using System;
using System.Linq; 

namespace Glimpse.Package
{
    public class RefreshReleaseRepositoryService : IRefreshReleaseRepositoryService
    { 
        private readonly IReleaseFeedProvider _releaseFeedProvider;
        private readonly IReleasePersistencyProvider _releasePersistencyProvider;

        public RefreshReleaseRepositoryService(IReleaseFeedProvider releaseFeedProvider, IReleasePersistencyProvider releasePersistencyProvider)
        { 
            _releaseFeedProvider = releaseFeedProvider;
            _releasePersistencyProvider = releasePersistencyProvider;
        }

        public RefreshReleaseRepositoryResults Execute()
        {
            var results = new RefreshReleaseRepositoryResults();

            var feedReleases = _releaseFeedProvider.GetAllCurrentReleases();

            var updateList = feedReleases.Select(x => new ReleasePersistencyItem { Name = x.Name, Scrapped = DateTime.UtcNow, Version = x.Version, Created = x.Created, IsAbsoluteLatestVersion = x.IsAbsoluteLatestVersion, IsPrerelease = x.IsPrerelease, IsLatestVersion = x.IsLatestVersion, ReleaseNotes = x.ReleaseNotes, IconUrl = x.IconUrl, Description = x.Description, Authors = x.Authors, DownloadCount = x.DownloadCount, VersionDownloadCount = x.VersionDownloadCount });
            var statisticsList = feedReleases.Select(x => new ReleasePersistencyStatisticsItem { DownloadCount = x.DownloadCount, Name = x.Name, Scrapped = DateTime.UtcNow, Version = x.Version, VersionDownloadCount = x.VersionDownloadCount });

            results.Results = updateList;

            results.ReleaseDetails = _releasePersistencyProvider.AddReleases(updateList);
            results.StatisticReleaseDetails = _releasePersistencyProvider.AddStatisticsReleases(statisticsList);
             
            return results;
        }
    }
}
