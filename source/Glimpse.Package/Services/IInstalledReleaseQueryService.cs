namespace Glimpse.Package
{
    public interface IInstalledReleaseQueryService
    {
        InstalledReleaseInfo GetReleaseInfo(VersionCheckDetails request);

        InstalledReleaseDetails GetReleaseInfo(VersionCheckDetailsItem request);

        InstalledReleaseDetails GetReleaseInfo(string name, string version);
    }
}