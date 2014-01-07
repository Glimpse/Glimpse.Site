using System.Collections.Generic;
using System.IO;
using Glimpse.Infrastructure.Models;
using Newtonsoft.Json;

namespace Glimpse.Infrastructure.Repositories
{
    public class GlimpseTeamMemberRepository
    {
        private string _teamMemberJsonFile;

        public GlimpseTeamMemberRepository(string teamMemberJsonFile)
        {
            _teamMemberJsonFile = teamMemberJsonFile;
        }

        public IEnumerable<GlimpseContributor> GetMembers()
        {
            var teamFileContent = File.ReadAllText(_teamMemberJsonFile);
            var teamContributors = JsonConvert.DeserializeObject<IEnumerable<GlimpseContributor>>(teamFileContent);
            return teamContributors;
        }
    }
}