using System.Collections.Generic;

namespace Glimpse.Contributor
{
    public interface IPackageAuthorProvider
    {
        IDictionary<string, IList<string>> AllPackageAuthors();
    }
}