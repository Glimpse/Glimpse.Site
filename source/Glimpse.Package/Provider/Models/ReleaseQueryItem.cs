using System;

namespace Glimpse.Package
{
    public class ReleaseQueryItem
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public DateTime Created { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }

        public bool IsPrerelease { get; set; }

        public string ReleaseNotes { get; set; }

        public string IconUrl { get; set; }

        public string Description { get; set; }
    }
}