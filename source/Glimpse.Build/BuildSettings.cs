namespace Glimpse.Build
{
    /// <summary>
    /// Singleton container for library settings.
    /// </summary>
    public static class BuildSettings
    {
        static BuildSettings()
        {
            Settings = new Settings();
        }

        /// <summary>
        /// Configuration settings for the system
        /// </summary>
        public static ISettings Settings { get; private set; }
    }
}