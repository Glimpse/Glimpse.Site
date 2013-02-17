using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;

namespace Glimpse.Package
{
    public class ReleasePersistencyProvider : IReleasePersistencyProvider
    {
        private readonly ISqlFactory _sqlFactory;

        private const string _selectReleases =
            @"SELECT Id, Name, Version, Created, IsLatestVersion, IsAbsoluteLatestVersion, IsPrerelease, ReleaseNotes, Scrapped, Hash, Description, IconUrl
              FROM PackageRelease";


        private const string _selectLastestStatisticsReleases =
            @"SELECT Id, PS.Name, PS.Version, DownloadCount, VersionDownloadCount, Scrapped
              FROM PackageReleaseStatistic AS PS
	              JOIN (SELECT MAX(LPS.Scrapped) AS LastScrappedDateTime, LPS.Name, LPS.Version 
                        FROM PackageReleaseStatistic AS LPS  
                        GROUP BY LPS.Name, LPS.Version) AS LPS
		            ON PS.Scrapped = LPS.LastScrappedDateTime 
                        AND PS.Name = LPS.Name 
                        AND PS.Version = LPS.Version"; 



        public ReleasePersistencyProvider(ISqlFactory sqlFactory)
        {
            _sqlFactory = sqlFactory;
        }

        public MergedReleaseDetails AddReleases(IEnumerable<ReleasePersistencyItem> data)
        { 
            var summary = new MergedReleaseDetails();

            using (var connection = _sqlFactory.CreateDbConnection())
            {
                connection.Open();

                // Pull out the existing 
                var currentRecords = connection.Query<ReleasePersistencyItem>(_selectReleases).ToDictionary(x => x.GetKey(), x => x);

                summary.ExistingRecordsFound = currentRecords.Count;

                // Rip through the data we have
                foreach (var record in data)
                {
                    // See if the record already exists
                    var currentRecord = currentRecords.GetValueOrDefault(record.GetKey()); 
                    if (currentRecord != null)
                    {
                        if (record.GetHashCode() != currentRecord.GetHashCode())
                        {
                            // Need to get our ID
                            record.Id = currentRecord.Id;

                            // Update record because the data has changed
                            connection.Update(record);
                            summary.RecordsUpdated++;
                        }
                    }
                    else
                    {
                        // Insert record because we don't have it 
                        connection.Insert(record);
                        summary.RecordsAdded++;
                    } 
                }
            }

            return summary;
        }

        public MergedStatisticReleaseDetails AddStatisticsReleases(IEnumerable<ReleasePersistencyStatisticsItem> data)
        {
            //var recordsAffected = 0;
            var summary = new MergedStatisticReleaseDetails();

            using (var connection = _sqlFactory.CreateDbConnection())
            {
                connection.Open();

                var previousReleases = SelectLastestStatisticsReleases(connection);

                summary.ExistingRecordsFound = previousReleases.Count;

                foreach (var releaseData in data)
                {
                    var previousRelease = previousReleases.GetValueOrDefault(releaseData.GetKey());

                    // Only insert if we don't have the data already or if the data has changed
                    if (previousRelease == null || previousRelease.VersionDownloadCount != releaseData.VersionDownloadCount)
                    {
                        connection.Insert(releaseData);
                        summary.RecordsAdded++;
                    }
                }
            }

            return summary;
        }

        public IDictionary<string, ReleasePersistencyStatisticsItem> SelectLastestStatisticsReleases()
        {
            using (var connection = _sqlFactory.CreateDbConnection())
            {
                connection.Open();

                return SelectLastestStatisticsReleases(connection);
            }
        }

        public IDictionary<string, ReleasePersistencyStatisticsItem> SelectLastestStatisticsReleases(DbConnection connection)
        { 
            var result = connection.Query<ReleasePersistencyStatisticsItem>(_selectLastestStatisticsReleases);
            var index = result.ToDictionary(x => x.GetKey(), x => x);
                 
            return index; 
        }
    }
}