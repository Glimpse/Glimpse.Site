using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Glimpse.Release
{
    public class PackageProvider : IPackageProvider
    {
        private readonly string _jsonFile;
        private IList<GlimpsePackage> _packages;
        private IDictionary<string, GlimpsePackageGroup> _grouping;
        private IList<string> _tags;

        protected IList<GlimpsePackage> Packages
        {
            get { return _packages ?? (_packages = InnerGetAllPackages()); }
        }

        public PackageProvider(string jsonFile)
        {
            _jsonFile = jsonFile;
        }

        public IList<string> GetAllPackagesTags()
        {
            return _tags ?? (_tags = Packages.SelectMany(x => x.Tags).Distinct().ToList());
        }

        public IList<GlimpsePackage> GetAllPackages()
        { 
            return Packages;
        }

        public IDictionary<string, GlimpsePackageGroup> GetAllPackagesGroupedByCategory()
        {
            return _grouping ?? (_grouping = InnerGetAllPackagesGroupedByCategory());
        }

        private IList<GlimpsePackage> InnerGetAllPackages()
        {
            var packagesFile = File.ReadAllText(_jsonFile);
            var packages = JsonConvert.DeserializeObject<IList<GlimpsePackage>>(packagesFile);

            return packages;
        }

        public IDictionary<string, GlimpsePackageGroup> InnerGetAllPackagesGroupedByCategory()
        {
            var grouping = Packages.GroupBy(x => x.Category).ToDictionary(g => g.Key, g =>
            {
                var items = g.ToList();
                var tags = items.SelectMany(x => x.Tags).Distinct().ToList(); 

                return new GlimpsePackageGroup {Packages = items, Tags = tags, Name = g.Key};
            });

            return grouping;
        }
    }
}
