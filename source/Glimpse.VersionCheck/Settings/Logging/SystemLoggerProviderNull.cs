using System;

namespace Glimpse.VersionCheck
{
    public class SystemLoggerProviderNull : ISystemLoggerProvider
    {
        public ISystemLogger CreateLogger()
        {
            return new SystemLoggerNull();
        }

        public ISystemLogger CreateLogger(Type name)
        {
            return new SystemLoggerNull();
        }

        public ISystemLogger CreateLogger(string name)
        {
            return new SystemLoggerNull();
        }
    }
}