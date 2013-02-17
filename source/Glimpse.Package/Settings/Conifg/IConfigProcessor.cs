namespace Glimpse.Package
{
    public interface IConfigProcessor
    {
        /// <summary>
        /// Process any app/web.config to load in config options
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>Options for any providers that the user wants to use</returns>
        void Process(ISettings settings);
    }
}