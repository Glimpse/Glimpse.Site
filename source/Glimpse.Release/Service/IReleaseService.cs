namespace Glimpse.Release
{
    public interface IReleaseService
    {
        Release GetRelease(string milestoneNumber);
    }
}