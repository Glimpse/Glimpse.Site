using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Glimpse.Contributor
{
    public class CommitterProvider : ICommitterProvider
    {
        private readonly string _jsonFile;
        private IList<Committer> _committers;

        public CommitterProvider(string jsonFile)
        {
            _jsonFile = jsonFile;
        }

        protected IList<Committer> AllMembers
        {
            get { return _committers ?? (_committers = InnerGetAllMembers()); }
        }

        public IList<Committer> GetAllMembers()
        { 
            return AllMembers;
        }

        private IList<Committer> InnerGetAllMembers()
        {
            var teamFileContent = File.ReadAllText(_jsonFile);
            var teamContributors = JsonConvert.DeserializeObject<IList<Committer>>(teamFileContent);

            return teamContributors;
        }
    }
}