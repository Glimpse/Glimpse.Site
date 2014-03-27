using System;
using Dapper;

namespace Glimpse.Package
{
    [Table("PackageRelease")]
    public class ReleasePersistencyItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public DateTime Created { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }

        public bool IsPrerelease { get; set; }

        public string ReleaseNotes { get; set; } 

        public DateTime Scrapped { get; set; }

        public int Hash { get { return GetHashCode(); } }

        public string IconUrl { get; set; }

        public string Description { get; set; }

        [Write(false)]
        public string Authors { get; set; }

        [Write(false)]
        public int DownloadCount { get; set; }

        [Write(false)]
        public int VersionDownloadCount { get; set; }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + (Name ?? "").GetHashCode();
                hash = hash * 23 + (Version ?? "").GetHashCode();
                hash = hash * 23 + Created.GetHashCode();
                hash = hash * 23 + IsLatestVersion.GetHashCode();
                hash = hash * 23 + IsAbsoluteLatestVersion.GetHashCode();
                hash = hash * 23 + IsPrerelease.GetHashCode();
                hash = hash * 23 + (ReleaseNotes ?? "").GetHashCode();
                hash = hash * 23 + (IconUrl ?? "").GetHashCode();
                hash = hash * 23 + (Description ?? "").GetHashCode();
                
                return hash;
            }
        }

        public string GetKey()
        {
            return String.Format("{0}||{1}", Name, Version);
        }
    }
}