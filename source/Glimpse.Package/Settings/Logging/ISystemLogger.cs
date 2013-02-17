using System;

namespace Glimpse.Package
{   
    public interface ISystemLogger
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; } 
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
        void Debug(object obj);
        void Info(object obj);
        void Warn(object obj);
        void Error(object obj);
        void Fatal(object obj);
        void Debug(Func<object> func);
        void Info(Func<object> func);
        void Warn(Func<object> func);
        void Error(Func<object> func);
        void Fatal(Func<object> func);
        void Debug(string format, params object[] args);
        void Info(string format, params object[] args);
        void Warn(string format, params object[] args);
        void Error(string format, params object[] args);
        void Fatal(string format, params object[] args);
        void Debug(string message, Exception exception);
        void Info(string message, Exception exception);
        void Warn(string message, Exception exception);
        void Error(string message, Exception exception);
        void Fatal(string message, Exception exception);
    }
}
