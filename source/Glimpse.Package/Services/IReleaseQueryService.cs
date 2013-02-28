namespace Glimpse.Package
{
    public interface IReleaseQueryService
    {
        ReleaseQueryInfo GetReleaseInfo(VersionCheckDetails request, bool includeReleasesData);

        ReleaseQueryDetails GetReleaseInfo(VersionCheckDetailsItem request, bool includeReleasesData);

        ReleaseQueryDetails GetReleaseInfo(string name, string versionFrom, string currentVersion, bool includeReleasesData);
    }
}