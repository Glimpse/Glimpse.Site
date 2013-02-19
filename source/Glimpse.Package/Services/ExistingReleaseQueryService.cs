using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Package
{
    public class ExistingReleaseQueryService : IExistingReleaseQueryService
    {
        private readonly IReleaseQueryProvider _queryProvider;

        public ExistingReleaseQueryService(IReleaseQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public ExistingReleaseInfo GetReleaseInfo(VersionCheckDetails request)
        {
            var info = new ExistingReleaseInfo { Details = new Dictionary<string, ExistingReleaseDetails>() };

            foreach (var package in request.Packages)
            {
                var detail = GetReleaseInfo(package);

                info.Details.Add(package.Name, detail);
            } 

            return info;
        }

        public ExistingReleaseDetails GetReleaseInfo(VersionCheckDetailsItem request)
        {
            return GetReleaseInfo(request.Name, request.Version);
        }

        public ExistingReleaseDetails GetReleaseInfo(string name, string version)
        {
            var details = new ExistingReleaseDetails(); 

            var currentRelease = _queryProvider.SelectRelease(name, version); 
            if (currentRelease != null)
            {
                var summary = new Dictionary<string, LatestReleaseDetailsSummaryInfo>();

                // We have a match
                details.HasResult = true;
                // Has newer version
                details.HasNewer = !(currentRelease.IsLatestVersion || currentRelease.IsAbsoluteLatestVersion);
                // Which channel
                details.Channel = currentRelease.IsPrerelease ? "preRelease" : "release";
                // Which version
                details.Version = version;

                var allNewReleases = _queryProvider.FindReleasesAfter(name, version).ToList();
                if (allNewReleases.Count > 0)
                {
                    // Summary details
                    var preReleases = allNewReleases.Where(x => x.IsPrerelease).ToList();
                    var nonPreRelease = allNewReleases.Where(x => !x.IsPrerelease).ToList();

                    var newestPreRelease = preReleases.LastOrDefault();
                    var newestNonPreRelease = nonPreRelease.LastOrDefault();

                    if (newestPreRelease != null)
                        summary.Add("preRelease", new LatestReleaseDetailsSummaryInfo { LatestVersion = newestPreRelease.Version, TotalNewerReleases = preReleases.Count });
                    if (newestNonPreRelease != null)
                        summary.Add("release", new LatestReleaseDetailsSummaryInfo { LatestVersion = newestNonPreRelease.Version, TotalNewerReleases = nonPreRelease.Count }); 
                    
                    // Releases details
                    details.Release = new ReleaseVersionData { Created = currentRelease.Created, IsLatestVersion = currentRelease.IsLatestVersion, IsAbsoluteLatestVersion = currentRelease.IsAbsoluteLatestVersion, IsPrerelease = currentRelease.IsPrerelease, ReleaseNotes = currentRelease.ReleaseNotes, Description = currentRelease.Description, IconUrl = currentRelease.IconUrl };
                }

                details.TotalNewerReleases = allNewReleases.Count;

                if (summary.Count > 0)
                    details.Summary = summary;
            }

            return details;
        }
    }

}