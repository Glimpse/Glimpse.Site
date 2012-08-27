using System.Collections.Generic;

namespace Glimpse.VersionCheck
{
    public interface IReleaseFeedProvider
    {
        IEnumerable<ReleaseFeedItem> GetAllCurrentReleases();

        IEnumerable<ReleaseFeedItem> GetAllCurrentReleases(ReleaseFeedOptions options);
    }
}