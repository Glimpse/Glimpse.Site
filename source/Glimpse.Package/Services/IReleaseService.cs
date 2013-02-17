namespace Glimpse.Package
{
    public interface IReleaseService
    {
        ReleaseDetails GetReleaseInfo(string name, string version);
    }
}