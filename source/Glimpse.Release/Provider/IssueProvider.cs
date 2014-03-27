using System;
using System.Collections.Generic;
using System.Linq; 
using Glimpse.Service;

namespace Glimpse.Release
{
    public class IssueProvider : IIssueProvider
    {
        private readonly IHttpClient _httpClient;
        private IList<GithubIssue> _issues;

        protected IList<GithubIssue> Issues
        {
            get { return _issues ?? (_issues = InnerGetAllIssues()); }
        }

        public IssueProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GithubIssue GetIssue(string id)
        {
            return Issues.FirstOrDefault(g => g.Id == id);
        }

        public IList<GithubIssue> GetAllIssuesByMilestone(int number)
        {
            return Issues.Where(g => g.Milestone.Number == number).ToList();
        }

        public IList<GithubIssue> GetAllIssuesByMilestoneThatHasTag(int number, IList<string> tags)
        {
            return Issues.Where(g => g.Milestone != null && g.Milestone.Number == number && g.Labels.Any(x => tags.Contains(x.Name))).ToList();
        }

        public IList<GithubIssue> GetAllIssues()
        {
            return Issues;
        }

        public void Clear()
        {
            _issues = null;
        }

        private IList<GithubIssue> InnerGetAllIssues()
        {
            var result = new List<GithubIssue>();

            try
            {
                result = _httpClient.GetPagedDataAsync<GithubIssue>(new Uri("https://api.github.com/repos/glimpse/glimpse/issues?state=all"));
            }
            catch (Exception)
            {
                //Not doing anything because we want to try and recover from this
            }

            return result;
        }
    }
}