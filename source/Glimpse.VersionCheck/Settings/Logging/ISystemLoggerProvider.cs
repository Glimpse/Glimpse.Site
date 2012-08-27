using System;

namespace Glimpse.VersionCheck
{
    public interface ISystemLoggerProvider
    {
        ISystemLogger CreateLogger();

        ISystemLogger CreateLogger(Type name);

        ISystemLogger CreateLogger(string name);
    }
}
