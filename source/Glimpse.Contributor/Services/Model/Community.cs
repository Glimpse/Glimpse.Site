using System.Collections.Generic;

namespace Glimpse.Contributor
{
    public class Community
    {
        public IList<Contributor> Contributors { get; set; }

        public IList<Committer> Committers { get; set; }

        public IDictionary<string, IList<string>> Authors { get; set; }
    }
}