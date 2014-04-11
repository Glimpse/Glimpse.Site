using Glimpse.Contributor.Services;

namespace Glimpse.Contributor
{
    public interface ISettings
    { 
        SettingsExtensionOptions Options { get; }

        ICommitterProvider CommitterProvider { get; }

        IContributorProvider ContributorProvider { get; }

        IPackageAuthorProvider PackageAuthorProvider { get; }

        ICommunityService CommunityService { get; }

        void Initialize();
    }
}
