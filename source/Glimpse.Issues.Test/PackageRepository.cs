using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Glimpse.Package;
using Newtonsoft.Json;
using Xunit;

namespace Glimpse.Issues.Test
{
    public class PackageRepositoryTests
    {
        [Fact]
        public void GivenPackageJsonFile_ShouldReturnAllPackagesWithinFile()
        {
            var packageRepository = new PackageRepository();
            var packages = packageRepository.GetAllPackages();
            Assert.True(packages.Count() == 2);
        }
    }
    public class PackageRepository
    {
        public virtual IEnumerable<GlimpsePackage> GetAllPackages()
        {
            var packagesFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "packages.json"));
            var packages = JsonConvert.DeserializeObject<IEnumerable<GlimpsePackage>>(packagesFile);
            return packages;
        }
    }
}