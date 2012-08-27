using System.Configuration;
using System.Data.Common;

namespace Glimpse.VersionCheck
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly ISystemLogger _logger;  

        public ConfigProvider(ISystemLogger logger)
        { 
            _logger = logger;
        }

        public string GetAppSetting(string name)
        {
            _logger.Info("Get App Setting - Looking up AppSettings value '{0}' to see what DbProviderFactory should be looked up", name);

            return ConfigurationManager.AppSettings[name]; 
        }

        public DbProviderFactory GetDbProviderFactory(string connectionStringName)
        {
            _logger.Info("Get App Setting - Looking up DbProviderFactory value '{0}' to see what DbProviderFactory should be retrieved", connectionStringName);

            return DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName);
        }

        public string GetConnectionString(string connectionStringName)
        {
            _logger.Info("Get App Setting - Pulling out ConnectionString for '{0}'", connectionStringName);

            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        public T GetSection<T>(string name) where T : class
        {
            return ConfigurationManager.GetSection(name) as T;
        }
    }
}