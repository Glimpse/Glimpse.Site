using System;

namespace Glimpse.VersionCheck
{
    public class SettingsExtensionOptions
    { 
        public ISystemLoggerProvider LoggerProvider { get; set; }

        public IConfigProvider ConfigProvider { get; set; } 

        public override string ToString()
        {
            return String.Format("{0}: LoggerProvider = '{1}', ConfigProvider = '{2}'", base.ToString(), LoggerProvider.GetTypeIfNotNull(), ConfigProvider.GetTypeIfNotNull());
        }
    }
}