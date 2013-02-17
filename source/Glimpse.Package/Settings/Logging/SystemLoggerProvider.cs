using System;

namespace Glimpse.Package
{
    public abstract class SystemLoggerProvider : ISystemLoggerProvider
    {
        protected readonly ISettings _settings;

        protected SystemLoggerProvider(ISettings settings)
        {
            _settings = settings;
        }
        
        public ISystemLogger CreateLogger()
        {
            return CreateLogger("ZocMon");
        }

        public ISystemLogger CreateLogger(Type name)
        {
            return CreateLogger(name.FullName);
        }

        public ISystemLogger CreateLogger(string name)
        {
            return BuildLoggingInstance(_settings.LoggingEnabled, name);
        }
         
        protected abstract ISystemLogger BuildLoggingInstance(bool loggingEnabled, string loggerName);
    }
}