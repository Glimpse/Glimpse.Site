namespace Glimpse.Package
{
    public class MergedReleaseDetails
    {
        public int RecordsAffected { get { return RecordsAdded + RecordsUpdated; } }

        public int RecordsAdded { get; set; }

        public int RecordsUpdated { get; set; }

        public int ExistingRecordsFound { get; set; }
    }
}