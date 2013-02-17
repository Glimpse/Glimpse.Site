using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glimpse.Package
{
    public class NugetReleaseFeedProvider : IReleaseFeedProvider
    { 
        private readonly object _lock = new object();

        public IEnumerable<ReleaseFeedItem> GetAllCurrentReleases()
        {
            // TODO: Need to get this data from the db instead of being hardcoded
            return GetAllCurrentReleases(new ReleaseFeedOptions { Depends = new List<string> { "Glimpse" } });
        }

        public IEnumerable<ReleaseFeedItem> GetAllCurrentReleases(ReleaseFeedOptions options)
        {
            var results = new Dictionary<string, ReleaseFeedItem>();

            // NOTE: I know that this marged this concept isn't the best but it means we can do all lookups in Parallel
            Parallel.ForEach(options.GetMergedOptions(), x =>
                {
                    IEnumerable<ReleaseFeedItem> found = null;
                    if (x.Type == ReleaseFeedOptions.ReleaseFeedOptionsMergedTypes.Specific)
                        found = GetAllReleasesForSpecific(x.Value);
                    else if (x.Type == ReleaseFeedOptions.ReleaseFeedOptionsMergedTypes.Depends)
                        found = GetAllReleasesForDepends(x.Value);
                    else 
                        throw new NotSupportedException("Type was out of range");

                    MergeResults(found, results);
                }); 

            return results.Values;
        }

        private IEnumerable<ReleaseFeedItem> GetAllReleasesForDepends(string id)
        {
            var context = new NugetFeed.FeedContext(new Uri("https://nuget.org/api/v2"));

            var skip = 0;
            var result = new List<ReleaseFeedItem>();
            var shouldCheck = true;
            while (shouldCheck)
            {
                var query = (from p in context.Packages
                             where p.Dependencies.Contains("|" + id + ":") || p.Dependencies.StartsWith(id + ":") || p.Id == id
                             select new ReleaseFeedItem { DownloadCount = p.DownloadCount, Name = p.Id, Version = p.Version, VersionDownloadCount = p.VersionDownloadCount, Created = p.Created, IsAbsoluteLatestVersion = p.IsAbsoluteLatestVersion, IsLatestVersion = p.IsLatestVersion, IsPrerelease = p.IsPrerelease, ReleaseNotes = p.ReleaseNotes, IconUrl = p.IconUrl, Description = p.Description })
                            .Skip(skip).Take(40).ToList();
                skip += 40;
                shouldCheck = query.Count == 40;
                result.AddRange(query);
            } 
            
            return result;
        }

        private IEnumerable<ReleaseFeedItem> GetAllReleasesForSpecific(string id)
        {
            var context = new NugetFeed.FeedContext(new Uri("https://nuget.org/api/v2"));

            var query = from p in context.Packages
                        where p.Id == id
                        select new ReleaseFeedItem { DownloadCount = p.DownloadCount, Name = p.Id, Version = p.Version, VersionDownloadCount = p.VersionDownloadCount, Created = p.Created, IsAbsoluteLatestVersion = p.IsAbsoluteLatestVersion, IsLatestVersion = p.IsLatestVersion, IsPrerelease = p.IsPrerelease, ReleaseNotes = p.ReleaseNotes, IconUrl = p.IconUrl, Description = p.Description };
            var result = query.ToList();

            return result;
        }

        private void MergeResults(IEnumerable<ReleaseFeedItem> soruce, IDictionary<string, ReleaseFeedItem> destination)
        {
            foreach (var item in soruce)
            {
                if (!destination.ContainsKey(item.GetKey()))
                {
                    lock (_lock)
                    {
                        if (!destination.ContainsKey(item.GetKey()))
                            destination.Add(item.GetKey(), item);
                    }
                }
            }
        }

    }
}