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

            try
            {
                Parallel.ForEach(new[] { "https://api.github.com/repos/glimpse/glimpse/contributors", "https://api.github.com/repos/glimpse/glimpse.client/contributors", "https://api.github.com/repos/glimpse/glimpse.site/contributors" }, x =>
                {
                    var result = _httpClient.GetPagedDataAsync<Contributor>(new Uri(x));
                    foreach (var contributor in result)
                    {
                        var existing = (Contributor)null;
                        if (data.TryGetValue(contributor.Login, out existing))
                        {
                            existing.Contributions += contributor.Contributions;
                        }
                        else
                            data.Add(contributor.Login, contributor);
                    }
                });
            }
            catch (Exception e)
            {
                //Not doing anything because we want to try and recover from this
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
