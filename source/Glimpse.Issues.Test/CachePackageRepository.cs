using System.Collections.Generic;

public class CachePackageRepository
{
    public virtual IEnumerable<Glimpse.Issues.Test.GlimpsePackage> GetAllPackages()
    {
        return new List<Glimpse.Issues.Test.GlimpsePackage>();
    }
}