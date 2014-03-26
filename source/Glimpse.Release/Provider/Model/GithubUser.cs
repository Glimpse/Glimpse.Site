using System.Collections.Generic;

namespace Glimpse.Release
{
    public class GithubUser : IEqualityComparer<GithubUser>
    {
        public string Login { get; set; }

        public string Id { get; set; }

        public string Avatar_Url { get; set; }

        public string Url { get; set; }

        public string Html_Url { get; set; }

        public bool Equals(GithubUser x, GithubUser y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(GithubUser obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}