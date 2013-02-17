using System;

namespace Glimpse.Package
{
    public class ReleaseFeedItem
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public DateTime Created { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }

        public bool IsPrerelease { get; set; }

        public string ReleaseNotes { get; set; }

        public int DownloadCount { get; set; }

        public int VersionDownloadCount { get; set; }

        public string IconUrl { get; set; }

        public string Description { get; set; }

        public string GetKey()
        {
            return String.Format("{0}||{1}", Name, Version);
        }
    }
}