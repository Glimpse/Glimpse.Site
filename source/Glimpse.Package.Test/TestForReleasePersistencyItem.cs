using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Glimpse.Package.Test
{
    public class TestForReleasePersistencyItem
    {
        public class UsingGetHashCode
        {
            [Fact]
            public void ShouldReturnSameHasForDifferentObjects()
            {
                var item1 = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };
                var item2 = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };

                Assert.Equal(item1.GetHashCode(), item2.GetHashCode());
            }

            [Fact]
            public void ShouldReturnNotSameHasForDifferentData()
            {
                var item1 = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 13), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };
                var item2 = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };

                Assert.NotEqual(item1.GetHashCode(), item2.GetHashCode());

                var item1b = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };
                var item2b = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = false, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };

                Assert.NotEqual(item1b.GetHashCode(), item2b.GetHashCode());

                var item1c = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };
                var item2c = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test1", ReleaseNotes = "", Version = "1.0" };

                Assert.NotEqual(item1c.GetHashCode(), item2c.GetHashCode());

                var item1d = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };
                var item2d = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = " ", Version = "1.0" };

                Assert.NotEqual(item1d.GetHashCode(), item2d.GetHashCode());

                var item1e = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.0" };
                var item2e = new ReleasePersistencyItem { Created = new DateTime(2012, 12, 12), IsAbsoluteLatestVersion = true, IsLatestVersion = true, IsPrerelease = false, Name = "Test", ReleaseNotes = "", Version = "1.1" };

                Assert.NotEqual(item1e.GetHashCode(), item2e.GetHashCode());
            }
        }
    }
}
