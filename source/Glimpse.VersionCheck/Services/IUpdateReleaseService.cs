namespace Glimpse.VersionCheck
{
    public interface IUpdateReleaseService
    {
        UpdateReleaseResults Execute();

        UpdateReleaseResults Execute(bool force);
    }
}