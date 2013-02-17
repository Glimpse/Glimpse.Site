using System.Collections.Generic;

namespace Glimpse.Package
{
    public interface IReleaseFeedProvider
    {
        IEnumerable<ReleaseFeedItem> GetAllCurrentReleases();

        IEnumerable<ReleaseFeedItem> GetAllCurrentReleases(ReleaseFeedOptions options);
    }
}