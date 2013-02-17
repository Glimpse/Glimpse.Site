using System;
using log4net;

namespace Glimpse.Package
{
    public class SystemLoggerLog4Net : ISystemLogger
    { 
        internal ILog Logger { get; set; }

        public SystemLoggerLog4Net(ILog logger)
        {
            Logger = logger;
        }

        public bool IsDebugEnabled
        {
            get { return Logger.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return Logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return Logger.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return Logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return Logger.IsFatalEnabled; }
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public void Debug(object obj)
        {
            Logger.Debug(obj);
        }

        public void Info(object obj)
        {
            Logger.Info(obj);
        }

        public void Warn(object obj)
        {
            Logger.Warn(obj);
        }

        public void Error(object obj)
        {
            Logger.Error(obj);
        }

        public void Fatal(object obj)
        {
            Logger.Fatal(obj);
        }

        public void Debug(Func<object> func)
        {
            if (Logger.IsDebugEnabled)
                Debug(func());
        }

        public void Info(Func<object> func)
        {
            if (Logger.IsInfoEnabled)
                Info(func());
        }

        public void Warn(Func<object> func)
        {
            if (Logger.IsWarnEnabled)
                Warn(func());
        }

        public void Error(Func<object> func)
        {
            if (Logger.IsErrorEnabled)
                Error(func());
        }

        public void Fatal(Func<object> func)
        {
            if (Logger.IsFatalEnabled)
                Fatal(func());
        }

        public void Debug(string format, params object[] args)
        {
            Logger.DebugFormat(format, args);
        }

        public void Info(string format, params object[] args)
        {
            Logger.InfoFormat(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Logger.WarnFormat(format, args);
        }

        public void Error(string format, params object[] args)
        {
            Logger.ErrorFormat(format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            Logger.FatalFormat(format, args);
        }

        public void Debug(string message, Exception exception)
        {
            Logger.Debug(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            Logger.Info(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            Logger.Warn(message, exception);
        }

        public void Error(string message, Exception exception)
        {
            Logger.Error(message, exception);
        }

        public void Fatal(string message, Exception exception)
        {
            Logger.Fatal(message, exception);
        }
    }
}
