using System.Collections.Generic;

namespace Glimpse.Contributor
{
    public interface IContributorProvider
    {
        IList<Contributor> GetAllContributors();

        void Clear();
    }
}