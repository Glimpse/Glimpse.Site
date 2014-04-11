using System.Configuration;
using Glimpse.Contributor.Services;
using Glimpse.Package;
using Glimpse.Service;

namespace Glimpse.Contributor
{
    public class Settings : ISettings
    {
        private bool? _useOfflineData;

        public Settings()
        {
            Options = new SettingsExtensionOptions(); 
        }

        public ICommitterProvider CommitterProvider { get; private set; }

        public IContributorProvider ContributorProvider { get; private set; }

        public IPackageAuthorProvider PackageAuthorProvider { get; private set; }

        public ICommunityService CommunityService { get; private set; } 

        public SettingsExtensionOptions Options { get; set; }

        public void Initialize()
        {
            var contributorListingPath = Options.ContributorListingPath;

            var httpClient = HttpClientFactory.CreateGithub();

            CommitterProvider = new CommitterProvider(contributorListingPath); 
            if (PackageSettings.Settings.UseOfflineData)
                ContributorProvider = new ContributorOfflineProvider(CommitterProvider);
            else
                ContributorProvider = new ContributorProvider(httpClient, CommitterProvider);  
            PackageAuthorProvider = new PackageAuthorProvider(PackageSettings.Settings.QueryProvider);
            CommunityService = new CommunityService(CommitterProvider, ContributorProvider, PackageAuthorProvider);
        }
    }
}