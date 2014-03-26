namespace Glimpse.Release
{ 
    public interface ISettings
    {
        SettingsExtensionOptions Options { get; }

        IMilestoneProvider MilestoneProvider { get; }

        IIssueProvider IssueProvider { get; }

        IReleaseService ReleaseService { get; }

        void Initialize();
    }
}
