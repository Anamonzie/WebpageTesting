using Serilog.Events;

namespace EpamWeb.Loggers
{
    public interface ILoggerManager
    {
        void Info(string testName, string message);
        void Warn(string testName, string message);
        void Error(string testName, string message, Exception ex = null);
        void CloseAndFlush(string testName);
        void InitializeLogFilePath(string testName);
        void LogEventWithTestName(string testName, LogEventLevel level, string message, Exception ex = null);
    }
}
