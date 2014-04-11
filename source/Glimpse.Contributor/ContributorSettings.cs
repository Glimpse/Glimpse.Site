namespace Glimpse.Contributor
{
    /// <summary>
    /// Singleton container for library settings.
    /// </summary>
    public static class ContributorSettings
    {
        static ContributorSettings()
        {
            Settings = new Settings();
        }

        /// <summary>
        /// Configuration settings for the system
        /// </summary>
        public static ISettings Settings { get; private set; }
    }
}