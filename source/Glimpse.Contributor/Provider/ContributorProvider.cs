using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Service; 

namespace Glimpse.Contributor
{
    public class ContributorProvider : IContributorProvider
    {
        private readonly IHttpClient _httpClient;
        private IList<Contributor> _committers;
        private ICommitterProvider _committerProvider;

        protected IList<Contributor> Commiters
        {
            get { return _committers ?? (_committers = InnerGetAllContributors()); }
        }

        public ContributorProvider(IHttpClient httpClient, ICommitterProvider committerProvider)
        {
            _httpClient = httpClient;
            _committerProvider = committerProvider;
        }

        public IList<Contributor> GetAllContributors()
        {
            return Commiters;
        }

        public void Clear()
        {
            _committers = null;
        }

        protected virtual Dictionary<string, Contributor> RawGetAllContributors()
        {
            var data = new Dictionary<string, Contributor>();

            var result1 = _httpClient.GetPagedDataAsync<Contributor>(new Uri("https://api.github.com/repos/glimpse/glimpse/contributors"));
            var result2 = _httpClient.GetPagedDataAsync<Contributor>(new Uri("https://api.github.com/repos/glimpse/glimpse.site/contributors"));
            var result3 = _httpClient.GetPagedDataAsync<Contributor>(new Uri("https://api.github.com/repos/glimpse/glimpse.client/contributors"));

            // Merge together the datasets
            var resultArray = new IList<Contributor>[] { result1, result2, result3 };
            foreach (var contributors in resultArray)
            {
                foreach (var contributor in contributors)
                {
                    var existing = (Contributor)null;
                    if (data.TryGetValue(contributor.Login, out existing))
                    {
                        existing.Contributions += contributor.Contributions;
                    }
                    else
                        data.Add(contributor.Login, contributor);
                }
            }

            return data;
        }

        private IList<Contributor> InnerGetAllContributors()
        {
            var data = RawGetAllContributors();

            // Pull out the contributors
            var comitters = _committerProvider.GetAllMembers();
            foreach (var comitter in comitters)
            {
                var existing = (Contributor)null;
                if (data.TryGetValue(comitter.GithubUsername, out existing))
                {
                    comitter.Contributions = existing.Contributions;
                    data.Remove(comitter.GithubUsername);
                }
            }

            data = data.OrderByDescending(x => x.Value.Contributions).ToDictionary(x => x.Key, x => x.Value);

            return data.Values.ToList();
        }
    }
}
