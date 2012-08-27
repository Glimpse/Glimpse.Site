using System;

namespace Glimpse.VersionCheck
{
    public class SystemLoggerNull : ISystemLogger
    {
        public bool IsDebugEnabled { get { return false; } }
        public bool IsInfoEnabled { get { return false; } }
        public bool IsWarnEnabled { get { return false; } }
        public bool IsErrorEnabled { get { return false; } }
        public bool IsFatalEnabled { get { return false; } }

        public void Trace(string message)
        {
        }

        public void Debug(string message)
        {
        }

        public void Info(string message)
        {
        }

        public void Warn(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Trace(object obj)
        {
        }

        public void Debug(object obj)
        {
        }

        public void Info(object obj)
        {
        }

        public void Warn(object obj)
        {
        }

        public void Error(object obj)
        {
        }

        public void Fatal(object obj)
        {
        }

        public void Trace(Func<object> func)
        {
        }

        public void Debug(Func<object> func)
        {
        }

        public void Info(Func<object> func)
        {
        }

        public void Warn(Func<object> func)
        {
        }

        public void Error(Func<object> func)
        {
        }

        public void Fatal(Func<object> func)
        {
        }

        public void Trace(string format, params object[] args)
        {
        }

        public void Debug(string format, params object[] args)
        {
        }

        public void Info(string format, params object[] args)
        {
        }

        public void Warn(string format, params object[] args)
        {
        }

        public void Error(string format, params object[] args)
        {
        }

        public void Fatal(string format, params object[] args)
        {
        }

        public void Trace(string message, Exception exception)
        {
        }

        public void Debug(string message, Exception exception)
        {
        }

        public void Info(string message, Exception exception)
        {
        }

        public void Warn(string message, Exception exception)
        {
        }

        public void Error(string message, Exception exception)
        {
        }

        public void Fatal(string message, Exception exception)
        {
        }
    }
}