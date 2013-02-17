using System;
using Dapper;

namespace Glimpse.Package
{
    [Table("PackageReleaseStatistic")]
    public class ReleasePersistencyStatisticsItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int DownloadCount { get; set; }

        public int VersionDownloadCount { get; set; }

        public DateTime Scrapped { get; set; } 

        public string GetKey()
        {
            return String.Format("{0}||{1}", Name, Version);
        }
    }
}