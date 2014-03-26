using System.Collections.Generic;
using System.Linq;
using Glimpse.Infrastructure.Models;
using Glimpse.Infrastructure.Repositories;

namespace Glimpse.Infrastructure.Services
{
    public class ContributorService
    {
        private readonly ContributorRepository _teamMemberRepository;
        private readonly IGithubContributerService _githubContributerService;

        public ContributorService(ContributorRepository teamMemberRepository, IGithubContributerService githubContributerService)
        {
            _teamMemberRepository = teamMemberRepository;
            _githubContributerService = githubContributerService;
        }

        public IEnumerable<GlimpseContributor> GetContributors()
        {
            var contributors = new List<GlimpseContributor>();
            var glimpseGithubContributors = _githubContributerService.GetContributors("glimpse/glimpse").ToList();
            glimpseGithubContributors.AddRange(_githubContributerService.GetContributors("glimpse/glimpse.site"));
            glimpseGithubContributors.AddRange(_githubContributerService.GetContributors("glimpse/glimpse.client"));
            var coreMembers = _teamMemberRepository.GetMembers().ToList();
            var glimpseContributors = GroupContributorsByName(GetContributorsExcludingMembers(glimpseGithubContributors, coreMembers).ToList());

            contributors.AddRange(coreMembers);
            contributors.AddRange(glimpseContributors.OrderByDescending(g => g.TotalContributions).ToList());
            return contributors;
        }

        private static IEnumerable<GlimpseContributor> GroupContributorsByName(IEnumerable<GlimpseContributor> contributors)
        {
            var contributorMap = new Dictionary<string, GlimpseContributor>();
            foreach (var contributor in contributors)
            {
                if (contributorMap.ContainsKey(contributor.GithubUsername))
                    contributorMap[contributor.GithubUsername].TotalContributions += contributor.TotalContributions;
                else
                    contributorMap.Add(contributor.GithubUsername, contributor);
            }
            return contributorMap.Values.ToList();
        }

        private static IEnumerable<GlimpseContributor> GetContributorsExcludingMembers(IEnumerable<GithubContributor> glimpseGithubContributor, IEnumerable<GlimpseContributor> coreMembers)
        {
            return from c in glimpseGithubContributor
                   where !(from o in coreMembers
                           select o.GithubUsername).Contains(c.Login)
                   select new GlimpseContributor
                              {
                                  Category = "Contributor",
                                  AvatarUrl = c.Avatar_Url,
                                  GithubUsername = c.Login,
                                  Name = c.Login,
                                  TotalContributions = c.Contributions,
                              };
        }
    }
}