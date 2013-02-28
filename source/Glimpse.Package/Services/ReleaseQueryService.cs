using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Package
{
    public class ReleaseQueryService : IReleaseQueryService
    {
        private readonly IReleaseQueryProvider _queryProvider;

        public ReleaseQueryService(IReleaseQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public ReleaseQueryInfo GetReleaseInfo(VersionCheckDetails request, bool includeReleasesData = false)
        {
            var info = new ReleaseQueryInfo { Details = new Dictionary<string, ReleaseQueryDetails>() };

            foreach (var package in request.Packages)
            {
                var detail = GetReleaseInfo(package, includeReleasesData);

                info.Details.Add(package.Name, detail);
                if (detail.HasNewer)
                    info.HasNewer = true;
            } 

            return info;
        }

        public ReleaseQueryDetails GetReleaseInfo(VersionCheckDetailsItem request, bool includeReleasesData = false)
        {
            return GetReleaseInfo(request.Name, request.Version, request.VersionRange, includeReleasesData);
        }
           
        public ReleaseQueryDetails GetReleaseInfo(string name, string oldVersion, string currentVersion, bool includeReleasesData = false)
        {
            var details = new ReleaseQueryDetails();

            var currentRelease = _queryProvider.SelectRelease(name, currentVersion); 
            if (currentRelease != null)
            { 
                details.HasResult = true; 
                details.HasNewer = !(currentRelease.IsLatestVersion || currentRelease.IsAbsoluteLatestVersion); 
                details.Channel = currentRelease.IsPrerelease ? "preRelease" : "release";
                details.Version = currentVersion;
                details.Summary = new Dictionary<string, ReleaseQuerySummaryInfo>();
                details.PackageDescription = currentRelease.Description;
                details.PackageIconUrl = currentRelease.IconUrl;
                details.Release = new ReleaseQueryVersionData { Created = currentRelease.Created, IsLatestVersion = currentRelease.IsLatestVersion, IsAbsoluteLatestVersion = currentRelease.IsAbsoluteLatestVersion, IsPrerelease = currentRelease.IsPrerelease, ReleaseNotes = currentRelease.ReleaseNotes, Description = currentRelease.Description, IconUrl = currentRelease.IconUrl };

                var allNewReleases = _queryProvider.FindReleasesAfter(name, oldVersion).ToList();
                if (allNewReleases.Count > 0)
                {
                    var preReleases = allNewReleases.Where(x => x.IsPrerelease).ToList();
                    var nonPreRelease = allNewReleases.Where(x => !x.IsPrerelease).ToList();
                    var newestPreRelease = preReleases.LastOrDefault();
                    var newestNonPreRelease = nonPreRelease.LastOrDefault();
                    var newestRelease = currentRelease.IsPrerelease && newestPreRelease != null && (newestNonPreRelease == null || newestNonPreRelease.Created < newestPreRelease.Created) ? newestPreRelease : newestNonPreRelease;

                    if (newestPreRelease != null)
                        details.Summary.Add("preRelease", new ReleaseQuerySummaryInfo { LatestVersion = newestPreRelease.Version, TotalNewerReleases = preReleases.Count });
                    if (newestNonPreRelease != null)
                        details.Summary.Add("release", new ReleaseQuerySummaryInfo { LatestVersion = newestNonPreRelease.Version, TotalNewerReleases = nonPreRelease.Count });
                    if (newestRelease != null)
                    {
                        details.Channel = newestRelease.IsPrerelease ? "preRelease" : "release";
                        details.PackageDescription = newestRelease.Description;
                        details.PackageIconUrl = newestRelease.IconUrl;
                    }
                    details.TotalNewerReleases = currentRelease.IsPrerelease ? allNewReleases.Count : nonPreRelease.Count;

                    // Releases details
                    if (includeReleasesData)
                    {
                        details.RequestedReleases = new Dictionary<string, ReleaseQueryVersionData>(StringComparer.OrdinalIgnoreCase);
                        details.AvailableReleases = new Dictionary<string, ReleaseQueryVersionData>(StringComparer.OrdinalIgnoreCase);

                        var trigger = String.Compare(oldVersion, currentVersion, StringComparison.OrdinalIgnoreCase) == 0;

                        var releases = currentRelease.IsPrerelease ? allNewReleases : nonPreRelease;
                        foreach (var release in releases)
                        {
                            var data = new ReleaseQueryVersionData { Created = release.Created, IsLatestVersion = release.IsLatestVersion, IsAbsoluteLatestVersion = release.IsAbsoluteLatestVersion, IsPrerelease = release.IsPrerelease, ReleaseNotes = release.ReleaseNotes, Description = release.Description, IconUrl = release.IconUrl };
                            if (!trigger)
                            {
                                details.RequestedReleases.Add(release.Version, data);
                                trigger = String.Compare(release.Version, currentVersion, StringComparison.OrdinalIgnoreCase) == 0;
                            }
                            else
                                details.AvailableReleases.Add(release.Version, data);
                        }   
                    }
                }
            }

            return details;
        }
    }

}