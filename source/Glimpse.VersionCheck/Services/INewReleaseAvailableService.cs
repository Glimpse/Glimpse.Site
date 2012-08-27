using System.Collections.Generic;

namespace Glimpse.VersionCheck
{
    public interface INewReleaseAvailableService
    {
        LatestReleaseInfo GetLatestReleaseInfo(VersionCheckDetails request, bool includeReleasesData);

        LatestReleaseDetails GetLatestReleaseInfo(VersionCheckDetailsItem request, bool includeReleasesData);

        LatestReleaseDetails GetLatestReleaseInfo(string name, string version, bool includeReleasesData);
    }
}