namespace Glimpse.Release
{
    /// <summary>
    /// Singleton container for library settings.
    /// </summary>
    public static class ReleaseSettings
    {
        static ReleaseSettings()
        {
            Settings = new Settings();
        }

        /// <summary>
        /// Configuration settings for the system
        /// </summary>
        public static ISettings Settings { get; private set; }
    }
}