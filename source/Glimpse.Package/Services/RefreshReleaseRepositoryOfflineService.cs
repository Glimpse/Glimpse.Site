using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Glimpse.Package.Services
{
    public class RefreshReleaseRepositoryOfflineService : IRefreshReleaseRepositoryService
    {
        private readonly string _jsonFile;

        public RefreshReleaseRepositoryOfflineService(string jsonFile)
        {
            _jsonFile = jsonFile;
        }

        public RefreshReleaseRepositoryResults Execute()
        { 
            var nugetFileContent = File.ReadAllText(_jsonFile);
            var result = JsonConvert.DeserializeObject<RefreshReleaseRepositoryResults>(nugetFileContent);

            return result; 
        }
    }
}
