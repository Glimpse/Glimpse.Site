namespace Glimpse.Package
{
    public interface IExistingReleaseQueryService
    {
        ExistingReleaseInfo GetReleaseInfo(VersionCheckDetails request);

        ExistingReleaseDetails GetReleaseInfo(VersionCheckDetailsItem request);

        ExistingReleaseDetails GetReleaseInfo(string name, string version);
    }
}