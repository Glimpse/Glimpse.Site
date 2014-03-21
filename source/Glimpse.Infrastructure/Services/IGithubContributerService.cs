using System.Collections.Generic;
using Glimpse.Infrastructure.Models;

namespace Glimpse.Infrastructure.Services
{
    public interface IGithubContributerService
    {
        IEnumerable<GithubContributor> GetContributors(string githubRepoName);
    }
}