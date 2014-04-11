using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Package
{ 
    public interface ISettings
    {
        ISystemLoggerProvider LoggerProvider { get; }
         
        bool LoggingEnabled { get; set; }

        bool LogEverything { get; set; }

        string LoggingPath { get; set; }
         
        int MinServiceTriggerInterval { get; set; }

        bool UseOfflineData { get; set; }

        string NugetListingPath { get; set; }

        IRefreshReleaseRepositoryService RefreshReleaseRepositoryService { get; }

        IReleaseQueryService ReleaseQueryService { get; }
         
        IRefreshReleaseService RefreshReleaseService { get; }

        IReleaseService ReleaseService { get; }

        IReleaseQueryProvider QueryProvider { get; }

        void Initialize();
    }
}
