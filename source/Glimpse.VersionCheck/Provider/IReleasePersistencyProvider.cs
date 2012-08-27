using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.VersionCheck
{
    public interface IReleasePersistencyProvider
    {
        MergedReleaseDetails AddReleases(IEnumerable<ReleasePersistencyItem> data);

        MergedStatisticReleaseDetails AddStatisticsReleases(IEnumerable<ReleasePersistencyStatisticsItem> data);

        IDictionary<string, ReleasePersistencyStatisticsItem> SelectLastestStatisticsReleases();
    }
}
