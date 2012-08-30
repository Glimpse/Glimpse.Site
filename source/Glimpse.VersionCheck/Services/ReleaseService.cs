namespace Glimpse.VersionCheck
{
    public class ReleaseService : IReleaseService
    {
        private readonly IReleaseQueryProvider _queryProvider;

        public ReleaseService(IReleaseQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public ReleaseDetails GetReleaseInfo(string name, string version)
        {
            var release = _queryProvider.SelectRelease(name, version);
        }
    }
}