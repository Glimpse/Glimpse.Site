namespace Glimpse.Package
{
    public interface IRefreshReleaseService
    {
        RefreshReleaseResults Execute();

        RefreshReleaseResults Execute(bool force);
    }
}