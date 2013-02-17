namespace Glimpse.Package
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

            if (release != null)
            {
                var details = new ReleaseDetails
                    {
                        Created = release.Created,
                        Description = release.Description,
                        IconUrl = release.IconUrl,
                        IsAbsoluteLatestVersion = release.IsAbsoluteLatestVersion,
                        IsLatestVersion = release.IsLatestVersion,
                        IsPrerelease = release.IsPrerelease,
                        ReleaseNotes = release.ReleaseNotes,
                        Name = name,
                        Version = version
                    };

                return details;
            }
            return null;
        }
    }
}