using System;
using Newtonsoft.Json;

namespace Glimpse.VersionCheck
{
    public class ReleaseDetails
    {
        public DateTime Created { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }

        public bool IsPrerelease { get; set; }

        public string ReleaseNotes { get; set; }

        public string IconUrl { get; set; }

        public string Description { get; set; }
    }
}