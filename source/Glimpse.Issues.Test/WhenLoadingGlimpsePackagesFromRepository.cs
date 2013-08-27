using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Xunit;

namespace Glimpse.Issues.Test
{
    public class WhenLoadingGlimpsePackagesFromRepository
    {
        private List<GlimpsePackage> _packages;
        private GlimpsePackage _package;

        public WhenLoadingGlimpsePackagesFromRepository()
        {
            string jsonFile = Path.Combine(Environment.CurrentDirectory, "packages.json");
            var packageRepository = new PackageRepository(jsonFile);

            _packages = packageRepository.GetAllPackages().ToList();
            _package = _packages.FirstOrDefault();
        }

        [Fact]
        public void ShouldLoadGlimpsePackageAsList()
        {
            Assert.Equal(1, _packages.Count);
        }

        [Fact]
        public void ShouldPopulateAppropriateProperties()
        {
            Assert.NotNull(_package);
            Assert.Equal("Glimpse.MVC4", _package.Title);
            Assert.Equal("MVC", _package.Category);
            Assert.Equal(new[]{"MVC","MVC4"}, _package.Tags);
            Assert.Equal(GlimpsePackageStatus.Green, _package.Status);
            Assert.Equal("status description.", _package.StatusDescription);
        }
    }
}
