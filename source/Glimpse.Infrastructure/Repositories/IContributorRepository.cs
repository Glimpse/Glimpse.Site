using System.Collections.Generic;
using Glimpse.Infrastructure.Models;

namespace Glimpse.Infrastructure.Repositories
{
    public interface IContributorRepository
    {
        IEnumerable<GlimpseContributor> GetMembers();
    }
}