using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using Glimpse.Service;

namespace Glimpse.Release
{
    public class MilestoneProvider : IMilestoneProvider
    {
        private readonly IHttpClient _httpClient;
        private IList<GithubMilestone> _milestones;
        private IList<GithubMilestone> _currentMilestones;
        private IDictionary<string, GithubMilestone> _indexedMilestones;

        protected IList<GithubMilestone> Milestones
        {
            get { return _milestones ?? (_milestones = InnerGetAllMilestones()); }
        }

        protected IList<GithubMilestone> CurrentMilestones
        {
            get { return _currentMilestones ?? (_currentMilestones = InnerGetCurrentMilestones()); }
        }

        protected IDictionary<string, GithubMilestone> IndexedMilestones
        {
            get { return _indexedMilestones ?? (_indexedMilestones = InnerIndexMilestones()); }
        }

        public MilestoneProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GithubMilestone GetMilestone(string title)
        {
            var milestone = (GithubMilestone)null;
            IndexedMilestones.TryGetValue(title.ToLower(), out milestone);
            return milestone;
        }

        public GithubMilestone GetLatestMilestoneWithIssues(string state)
        {
            return (from g in Milestones
                    where g.State == state && (g.Open_Issues > 0 || g.Closed_Issues > 0)
                    orderby g.Created_At descending
                    select g).FirstOrDefault();
        }

        public IList<GithubMilestone> GetAllMilestones()
        {
            return Milestones;
        }

        public IList<GithubMilestone> GetCurrentMilestones()
        {
            return CurrentMilestones;
        }

        public void Clear()
        {
            _milestones = null;
            _currentMilestones = null;
        }

        private IDictionary<string, GithubMilestone> InnerIndexMilestones()
        {
            return Milestones.ToDictionary(x => x.Title.ToLower(), x => x);
        }

        private IList<GithubMilestone> InnerGetCurrentMilestones()
        {
            return Milestones.Where(x => (x.Number >= 17 && x.State.ToLower() == "closed") || x.Title.ToLower() == "vnext").OrderByDescending(x => x.Created_At).ToList();
        }

        private IList<GithubMilestone> InnerGetAllMilestones()
        {
            var data = new List<GithubMilestone>();

            try
            {
                Parallel.ForEach(new[] { "https://api.github.com/repos/glimpse/glimpse/milestones", "https://api.github.com/repos/glimpse/glimpse/milestones?state=closed" }, x =>
                {
                    var result = _httpClient.GetPagedDataAsync<GithubMilestone>(new Uri(x));

                    data.AddRange(result);
                });
            }
            catch (Exception e)
            {
                //Not doing anything because we want to try and recover from this
            }

            return data;
        }
    }
}
