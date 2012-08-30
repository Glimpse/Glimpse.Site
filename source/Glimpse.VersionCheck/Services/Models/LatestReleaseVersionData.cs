using System;
using Newtonsoft.Json;

namespace Glimpse.VersionCheck
{
    public class LatestReleaseVersionData
    {
        public DateTime Created { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }

        public bool IsPrerelease { get; set; }

        public string ReleaseNotes { get; set; }

        [JsonIgnore]
        public string IconUrl { get; set; }

        [JsonIgnore]
        public string Description { get; set; }
    }
}