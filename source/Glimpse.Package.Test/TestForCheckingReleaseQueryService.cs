using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Glimpse.Package.Test
{
    public class TestForCheckingReleaseQueryService
    {
        public class UsingGetLatestReleaseInfo
        {
            [Fact]
            public void ShouldReturnEmptyIsNoMatchFound()
            {
                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns((ReleaseQueryItem)null).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.False(result.HasResult);
                Assert.False(result.HasNewer);
                Assert.Null(result.Summary);
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void ShouldIndicateThatNoNewerVersionIsFoundIfOneDoesntExist()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = true };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem>()).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.True(result.HasResult);
                Assert.False(result.HasNewer);
                Assert.Null(result.Summary);
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void ShouldProduceReleaseSummaryWhenOnlyNewerReleasesFound()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 12), Version = "1.0" };
                var release1 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 13), Version = "1.1" };
                var release2 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 14), Version = "1.2" };
                var release3 = new ReleaseQueryItem { IsLatestVersion = true, Created = new DateTime(2012, 12, 15), Version = "1.3" };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem> { release1, release2, release3 }).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.True(result.HasResult);
                Assert.True(result.HasNewer);
                Assert.NotNull(result.Summary);
                Assert.True(result.Summary.ContainsKey("release"));
                Assert.Equal("1.3", result.Summary["release"].LatestVersion);
                Assert.False(result.Summary.ContainsKey("preRelease"));
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void ShouldProducePreReleaseSummaryWhenOnlyNewerPreReleasesFound()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 12), Version = "1.0", IsPrerelease = true };
                var release1 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 13), Version = "1.1", IsPrerelease = true };
                var release2 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 14), Version = "1.2", IsPrerelease = true };
                var release3 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 15), Version = "1.3", IsPrerelease = true, IsAbsoluteLatestVersion = true };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem> { release1, release2, release3 }).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.True(result.HasResult);
                Assert.True(result.HasNewer);
                Assert.NotNull(result.Summary);
                Assert.True(result.Summary.ContainsKey("preRelease"));
                Assert.Equal("1.3", result.Summary["preRelease"].LatestVersion);
                Assert.False(result.Summary.ContainsKey("release"));
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void ShouldProduceSummaryWhenOnlyNewerReleasesFound()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 12), Version = "1.0" };
                var release1 = new ReleaseQueryItem { IsLatestVersion = true, Created = new DateTime(2012, 12, 13), Version = "1.1" };
                var release2 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 14), Version = "1.2", IsPrerelease = true };
                var release3 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 15), Version = "1.3", IsPrerelease = true, IsAbsoluteLatestVersion = true };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem> { release1, release2, release3 }).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.True(result.HasResult);
                Assert.True(result.HasNewer);
                Assert.NotNull(result.Summary);
                Assert.True(result.Summary.ContainsKey("preRelease"));
                Assert.Equal("1.3", result.Summary["preRelease"].LatestVersion);
                Assert.True(result.Summary.ContainsKey("release"));
                Assert.Equal("1.1", result.Summary["release"].LatestVersion);
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void WhenOnReleaseChannelShouldOnlyIndicateHasNewerWhenNewerVersionPresentOnReleaseChannel()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = true, Created = new DateTime(2012, 12, 12), Version = "1.0" };
                var release1 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 13), Version = "1.1", IsPrerelease = true };
                var release2 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 14), Version = "1.2", IsPrerelease = true };
                var release3 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 15), Version = "1.3", IsPrerelease = true, IsAbsoluteLatestVersion = true };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem> { release1, release2, release3 }).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.True(result.HasResult);
                Assert.False(result.HasNewer);
                Assert.NotNull(result.Summary);
                Assert.True(result.Summary.ContainsKey("preRelease"));
                Assert.Equal("1.3", result.Summary["preRelease"].LatestVersion);
                Assert.False(result.Summary.ContainsKey("release"));
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void WhenOnPreReleaseChannelShouldIndicateHasNewerWhenNewerVersionPresentOnEitherChannel()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 12), Version = "1.0", IsPrerelease = true };
                var release1 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 13), Version = "1.1" };
                var release2 = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 14), Version = "1.2"};
                var release3 = new ReleaseQueryItem { IsLatestVersion = true, Created = new DateTime(2012, 12, 15), Version = "1.3", IsAbsoluteLatestVersion = true };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem> { release1, release2, release3 }).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo("Test", "1.0");

                Assert.True(result.HasResult);
                Assert.True(result.HasNewer);
                Assert.NotNull(result.Summary);
                Assert.True(result.Summary.ContainsKey("release"));
                Assert.Equal("1.3", result.Summary["release"].LatestVersion);
                Assert.False(result.Summary.ContainsKey("preRelease"));
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void ShouldBeAbleToUseObjectOverload()
            {
                var input = new VersionCheckDetailsItem() {Name = "Test", Version = "1.0"};

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns((ReleaseQueryItem)null).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo(input);

                Assert.False(result.HasResult);
                Assert.False(result.HasNewer);
                Assert.Null(result.Summary);
                Assert.Null(result.Releases);

                repository.VerifyAll();
            }

            [Fact]
            public void ShouldBeAbleToUseCollectiontOverload()
            {
                var input1 = new VersionCheckDetailsItem() { Name = "Test", Version = "1.0" };
                var input2 = new VersionCheckDetailsItem() { Name = "Other", Version = "1.0" };
                var input = new VersionCheckDetails {Packages = new List<VersionCheckDetailsItem> {input1, input2}};

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns((ReleaseQueryItem)null).Verifiable();
                repository.Setup(x => x.SelectRelease("Other", "1.0")).Returns((ReleaseQueryItem)null).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo(input);

                Assert.Equal(2, result.Details.Count);
                Assert.True(result.Details.ContainsKey("Test"));
                Assert.True(result.Details.ContainsKey("Other")); 
                Assert.False(result.HasNewer);

                repository.VerifyAll();
            }

            [Fact]
            public void ShoudlIndicateHasNewerWhenAnyPackageHasNewer()
            {
                var release = new ReleaseQueryItem { IsLatestVersion = false, Created = new DateTime(2012, 12, 12), Version = "1.0" };
                var release1 = new ReleaseQueryItem { IsLatestVersion = true, Created = new DateTime(2012, 12, 13), Version = "1.1" };

                var input1 = new VersionCheckDetailsItem() { Name = "Test", Version = "1.0" };
                var input2 = new VersionCheckDetailsItem() { Name = "Other", Version = "1.0" };
                var input = new VersionCheckDetails { Packages = new List<VersionCheckDetailsItem> { input1, input2 } };

                var repository = new Mock<IReleaseQueryProvider>();
                repository.Setup(x => x.SelectRelease("Test", "1.0")).Returns(release).Verifiable();
                repository.Setup(x => x.SelectRelease("Other", "1.0")).Returns((ReleaseQueryItem)null).Verifiable();
                repository.Setup(x => x.FindReleasesAfter("Test", "1.0")).Returns(new List<ReleaseQueryItem> { release1 }).Verifiable();

                var service = new CheckingForReleaseQueryService(repository.Object);
                var result = service.GetLatestReleaseInfo(input);

                Assert.Equal(2, result.Details.Count);
                Assert.True(result.Details.ContainsKey("Test"));
                Assert.True(result.Details["Test"].HasNewer);
                Assert.True(result.Details["Test"].HasResult);
                Assert.True(result.Details.ContainsKey("Other"));
                Assert.False(result.Details["Other"].HasNewer);
                Assert.False(result.Details["Other"].HasResult);
                Assert.True(result.HasNewer);

                repository.VerifyAll();
            }
        }
    }
}
