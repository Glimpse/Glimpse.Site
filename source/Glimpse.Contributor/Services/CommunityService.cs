namespace Glimpse.Contributor.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommitterProvider _committerProvider;
        private readonly IContributorProvider _contributorProvider;
        private readonly IPackageAuthorProvider _packageAuthorProvider;

        public CommunityService(ICommitterProvider committerProvider, IContributorProvider contributorProvider, IPackageAuthorProvider packageAuthorProvider)
        {
            _committerProvider = committerProvider;
            _contributorProvider = contributorProvider;
            _packageAuthorProvider = packageAuthorProvider;
        }

        public Community AllCommunity()
        {
            var community = new Community();
            community.Committers = _committerProvider.GetAllMembers();
            community.Contributors = _contributorProvider.GetAllContributors();
            community.Authors = _packageAuthorProvider.AllPackageAuthors();

            return community;
        }
    }
}
