using System.Collections.Generic;
using System.Linq;

namespace Glimpse.VersionCheck
{
    public class NewReleaseAvailableService : INewReleaseAvailableService
    {
        private readonly IReleaseQueryProvider _queryProvider;

        public NewReleaseAvailableService(IReleaseQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public LatestReleaseInfo GetLatestReleaseInfo(VersionCheckDetails request, bool includeReleasesData = false)
        {
            var info = new LatestReleaseInfo { Details = new Dictionary<string, LatestReleaseDetails>() };

            foreach (var package in request.Packages)
            {
                var detail = GetLatestReleaseInfo(package, includeReleasesData);

                info.Details.Add(package.Name, detail);
                if (detail.HasNewer)
                    info.HasNewer = true;
            }

            return info;
        }

        public LatestReleaseDetails GetLatestReleaseInfo(VersionCheckDetailsItem request, bool includeReleasesData = false)
        {
            return GetLatestReleaseInfo(request.Name, request.Version, includeReleasesData);
        }

        public LatestReleaseDetails GetLatestReleaseInfo(string name, string version, bool includeReleasesData = false)
        {
            var details = new LatestReleaseDetails(); 

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
                    var newestPreRelease = allNewReleases.LastOrDefault(x => x.IsPrerelease);
                    var newestRelease = allNewReleases.LastOrDefault(x => !x.IsPrerelease);

                    if (newestPreRelease != null)
                        summary.Add("preRelease", new LatestReleaseDetailsSummaryInfo { LatestVersion = newestPreRelease.Version });
                    if (newestRelease != null)
                        summary.Add("release", new LatestReleaseDetailsSummaryInfo { LatestVersion = newestRelease.Version });

                    // Releases details
                    if (includeReleasesData)
                        details.Releases = allNewReleases.ToDictionary(x => x.Version, x => new LatestReleaseVersionData { Created = x.Created, IsLatestVersion = x.IsLatestVersion, IsAbsoluteLatestVersion = x.IsAbsoluteLatestVersion, IsPrerelease = x.IsPrerelease, ReleaseNotes = x.ReleaseNotes, Description = x.Description, IconUrl = x.IconUrl });
                }

                if (summary.Count > 0)
                    details.Summary = summary;
            }

            return details;
        }
    }

}