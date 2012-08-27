using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Glimpse.VersionCheck
{
    public class SystemLoggerProviderLog4Net : SystemLoggerProvider
    { 
        public SystemLoggerProviderLog4Net(ISettings settings)
            : base(settings)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(settings.LoggingPath));
        } 

        protected override ISystemLogger BuildLoggingInstance(bool loggingEnabled, string loggerName)
        {
            if (!loggingEnabled)
                return new SystemLoggerNull();

            return new SystemLoggerLog4Net(log4net.LogManager.GetLogger(loggerName));
        }
    } 
}
