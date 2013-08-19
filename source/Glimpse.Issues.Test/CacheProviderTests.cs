using Xunit;

namespace Glimpse.Issues.Test
{
    public class CacheProviderTests
    {
        [Fact]
        public void ShouldReturnNullForExpiredItems()
        {
            var cacheProvider = new CacheProvider(-1);
            cacheProvider.Add("key","test");

            var value = cacheProvider.Get("key");

            Assert.Null(value);
        }

        [Fact]
        public void ShouldReturnValueOfObjectForNonExpiredCacheItems()
        {
            var cacheProvider = new CacheProvider(1);
            cacheProvider.Add("key","test");

            var value = cacheProvider.Get("key");

            Assert.Equal(value, "test");
        }

    }
}