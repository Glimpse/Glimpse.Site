namespace Glimpse.Package
{
    public interface IUpdateReleaseService
    {
        UpdateReleaseResults Execute();

        UpdateReleaseResults Execute(bool force);
    }
}