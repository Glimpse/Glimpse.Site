namespace Glimpse.Package
{
    public class MergedStatisticReleaseDetails
    {
        public int RecordsAffected { get { return RecordsAdded; } }

        public int RecordsAdded { get; set; } 

        public int ExistingRecordsFound { get; set; }
    }
}