using System.Collections.Generic;
using System.Linq;
using Glimpse.Package;

namespace Glimpse.Contributor
{
    public class PackageAuthorProvider : IPackageAuthorProvider
    {
        private readonly IReleaseQueryProvider _queryProvider;

        public PackageAuthorProvider(IReleaseQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public IDictionary<string, IList<string>> AllPackageAuthors()
        {
            return _queryProvider.SelectAllPackageAuthors();
        }
    }
}