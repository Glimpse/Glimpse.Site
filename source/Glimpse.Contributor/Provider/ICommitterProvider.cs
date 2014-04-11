using System;
using System.Collections.Generic;

namespace Glimpse.Contributor
{
    public interface ICommitterProvider
    {
        IList<Committer> GetAllMembers();
    }
}
