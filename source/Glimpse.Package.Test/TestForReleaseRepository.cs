using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace Glimpse.Package.Test
{
    public class TestForReleaseRepository
    {
        public class UsingSelectAllPackages
        {
            [Fact]
            public void ShouldThrowExceptionWhenCacheNotSet()
            {
                var respository = new CacheReleaseQueryProvider();
                Assert.Throws<NullReferenceException>(() => respository.SelectAllPackages());
            }

            [Fact]
            public void ShouldReturnAllResults()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();
                 
                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Same(data, respository.SelectAllPackages());
            }
        }

        public class UsingSelectPackage
        {
            [Fact]
            public void ShouldReturnPackageIfPresent()
            {
                var dataList = new List<ReleaseQueryItem>();
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };
                 
                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Same(dataList, respository.SelectPackage("Test"));
            }

            [Fact]
            public void ShouldReturnNullIfPackageNotPresent()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.SelectPackage("Test"));
            } 
        }

        public class UsingLatestPackageRelease
        {
            [Fact]
            public void ShouldReturnLatestReleaseIfPresent()
            {
                var item1 = new ReleaseQueryItem { IsLatestVersion = true };
                var item2 = new ReleaseQueryItem();

                var dataList = new List<ReleaseQueryItem> { item1, item2 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Same(item1, respository.LatestPackageRelease("Test"));
            }

            [Fact]
            public void ShouldReturnNullIfNoLastestVersionPresent()
            {
                var item1 = new ReleaseQueryItem();

                var dataList = new List<ReleaseQueryItem> { item1 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.LatestPackageRelease("Test"));
            }

            [Fact]
            public void ShouldReturnNullIfPackageNotPresent()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.LatestPackageRelease("Test"));
            }
        }

        public class UsingLatestPackageAbsoluteRelease
        {
            [Fact]
            public void ShouldReturnAbsoluteLatestReleaseIfPresent()
            {
                var item1 = new ReleaseQueryItem { IsAbsoluteLatestVersion = true };
                var item2 = new ReleaseQueryItem();

                var dataList = new List<ReleaseQueryItem> { item1, item2 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Same(item1, respository.LatestPackageAbsoluteRelease("Test"));
            }

            [Fact]
            public void ShouldReturnNullIfNoAbsoluteLastestVersionPresent()
            {
                var item1 = new ReleaseQueryItem();

                var dataList = new List<ReleaseQueryItem> { item1 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.LatestPackageAbsoluteRelease("Test"));
            }

            [Fact]
            public void ShouldReturnNullIfPackageNotPresent()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.LatestPackageAbsoluteRelease("Test"));
            }
        }

        public class UsingSelectRelease
        {
            [Fact]
            public void ShouldReturnReleaseIfPresent()
            {
                var item1 = new ReleaseQueryItem { Version = "1.0" };
                var item2 = new ReleaseQueryItem { Version = "0.9" };

                var dataList = new List<ReleaseQueryItem> { item1, item2 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Same(item1, respository.SelectRelease("Test", "1.0"));
            }

            [Fact]
            public void ShouldReturnNullIfNoVersionPresent()
            {
                var item1 = new ReleaseQueryItem();

                var dataList = new List<ReleaseQueryItem> { item1 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.SelectRelease("Test", "1.0"));
            }

            [Fact]
            public void ShouldReturnNullIfPackageNotPresent()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.SelectRelease("Test", "1.0"));
            }
        }

        public class UsingFindReleasesAfter
        {
            [Fact]
            public void ShouldReturnReleaseIfPresent()
            {
                var item1 = new ReleaseQueryItem { Version = "1.0", Created = new DateTime(2012, 12, 1) };
                var item2 = new ReleaseQueryItem { Version = "1.1", Created = new DateTime(2012, 12, 2) };
                var item3 = new ReleaseQueryItem { Version = "1.2", Created = new DateTime(2012, 12, 3) };
                var item4 = new ReleaseQueryItem { Version = "1.3", Created = new DateTime(2012, 12, 4) };

                var dataList = new List<ReleaseQueryItem> { item1, item2, item3, item4 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                var result = respository.FindReleasesAfter("Test", "1.0").ToList();

                Assert.Equal(3, result.Count);
                Assert.Equal("1.1", result[0].Version);
                Assert.Equal("1.2", result[1].Version);
                Assert.Equal("1.3", result[2].Version);
            }

            [Fact]
            public void ShouldReturnEmptyIfNoNewerVersionPresent()
            {
                var item1 = new ReleaseQueryItem();

                var dataList = new List<ReleaseQueryItem> { item1 };
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>> { { "Test", dataList } };

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Empty(respository.FindReleasesAfter("Test", "1.0"));
            }

            [Fact]
            public void ShouldReturnEmptyIfPackageNotPresent()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data);

                Assert.Null(respository.SelectRelease("Test", "1.0"));
            }
        }

        public class UsingUpdateCache
        {
            [Fact]
            public void ShouldThrowExceptionWhenCacheEmpty()
            {
                var respository = new CacheReleaseQueryProvider();
                Assert.Throws<ArgumentNullException>(() => respository.UpdateCache(null));
            }

            [Fact]
            public void ShouldBeAbleToSetCache()
            {
                var data = new Dictionary<string, IEnumerable<ReleaseQueryItem>>();

                var respository = new CacheReleaseQueryProvider();
                respository.UpdateCache(data); 
            }
        }
    }
}
