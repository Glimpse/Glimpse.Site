using System;

namespace Glimpse.Package
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