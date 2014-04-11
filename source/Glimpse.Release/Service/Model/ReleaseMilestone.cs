using System;

namespace Glimpse.Release
{
    public class ReleaseMilestone
    {
        public string Url { get; set; }

        public int Number { get; set; }

        public string State { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int OpenIssues { get; set; }

        public int ClosedIssues { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DueOn { get; set; }

        public bool IsVNext
        {
            get { return String.Equals(Title, "vnext", StringComparison.OrdinalIgnoreCase); }
        }
    }
}
