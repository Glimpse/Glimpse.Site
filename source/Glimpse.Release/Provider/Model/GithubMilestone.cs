using System;

namespace Glimpse.Release
{
    public class GithubMilestone
    {
        public string Url { get; set; }

        public int Number { get; set; }

        public string State { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Open_Issues { get; set; }

        public int Closed_Issues { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime? Due_On { get; set; }
    }
}
