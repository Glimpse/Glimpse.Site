namespace Glimpse.VersionCheck
{
    public interface IReleaseService
    {
        ReleaseDetails GetReleaseInfo(string name, string version);
    }
}