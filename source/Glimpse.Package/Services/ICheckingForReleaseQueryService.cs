using System.Collections.Generic;

namespace Glimpse.Package
{
    public interface ICheckingForReleaseQueryService
    {
        CheckReleaseInfo GetLatestReleaseInfo(VersionCheckDetails request, bool includeReleasesData);

        CheckReleaseDetails GetLatestReleaseInfo(VersionCheckDetailsItem request, bool includeReleasesData);

        CheckReleaseDetails GetLatestReleaseInfo(string name, string version, bool includeReleasesData);
    }
}