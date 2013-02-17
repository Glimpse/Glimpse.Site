using System;

namespace Glimpse.Package
{
    public interface ISystemLoggerProvider
    {
        ISystemLogger CreateLogger();

        ISystemLogger CreateLogger(Type name);

        ISystemLogger CreateLogger(string name);
    }
}
