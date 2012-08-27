using System.Collections.Generic;
using System.Linq;

namespace Glimpse.VersionCheck
{
    public class ReleaseFeedOptions
    {
        public IEnumerable<string> Depends { get; set; }

        public IEnumerable<string> Specific { get; set; }

        internal class ReleaseFeedOptionsMerged
        {
            public string Value { get; set; }

            public ReleaseFeedOptionsMergedTypes Type { get; set; }
        }

        internal enum ReleaseFeedOptionsMergedTypes
        {
            Depends,
            Specific
        }

        internal IEnumerable<ReleaseFeedOptionsMerged>  GetMergedOptions()
        {
            var options = new List<ReleaseFeedOptionsMerged>();
            if (Depends != null)
                options.AddRange(Depends.Select(x => new ReleaseFeedOptionsMerged { Value = x, Type = ReleaseFeedOptionsMergedTypes.Depends }));
            if (Specific != null)
                options.AddRange(Specific.Select(x => new ReleaseFeedOptionsMerged { Value = x, Type = ReleaseFeedOptionsMergedTypes.Specific }));

            return options;
        }
    }
}