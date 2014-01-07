using System.Collections.Generic;
using System.Linq;
using Glimpse.Infrastructure.Models;
using Glimpse.Infrastructure.Repositories;

namespace Glimpse.Infrastructure.Services
{
    public class ContributorService
    {
        private readonly GlimpseTeamMemberRepository _teamMemberRepository;
        private readonly IGithubContributerService _githubContributerService;

        public ContributorService(GlimpseTeamMemberRepository teamMemberRepository, IGithubContributerService githubContributerService)
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
            var glimpseContributors = GetContributorsExcludingMembers(glimpseGithubContributors, coreMembers);

            contributors.AddRange(glimpseContributors.ToList());
            contributors.AddRange(coreMembers);
            return contributors;
        }

        private static IEnumerable<GlimpseContributor> GetContributorsExcludingMembers(IEnumerable<GithubContributor> glimpseGithubContributor, IEnumerable<GlimpseContributor> coreMembers)
        {
            return from c in glimpseGithubContributor
                   where !(from o in coreMembers
                           select o.Name).Contains(c.Login)
                   select new GlimpseContributor
                              {
                                  Category = "Contributor",
                                  GithubUsername = c.Login,
                                  Name = c.Login,
                                  TotalContributions = c.Contributions
                              };
        }
    }
}