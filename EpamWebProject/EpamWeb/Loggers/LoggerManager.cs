using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace EpamWeb.Loggers
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly Lazy<LoggerManager> instance = new(() => new LoggerManager());
        private readonly Logger inMemoryLogger;
        private readonly BlockingCollection<LogEvent> logEvents;
        private readonly ConcurrentDictionary<string, string> logFilePaths;

        private LoggerManager()
        {
            logEvents = new BlockingCollection<LogEvent>();
            logFilePaths = new ConcurrentDictionary<string, string>();

            inMemoryLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Sink(new InMemorySink(logEvents))
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public static LoggerManager Instance => instance.Value;

        public void InitializeLogFilePath(string testName)
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", testName);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var logFilePath = Path.Combine(logDirectory, $"{testName}-log.txt");
            logFilePaths[testName] = logFilePath;  // Store path for each test
        }

        public void CloseAndFlush(string testName)
        {
            if (!logFilePaths.TryGetValue(testName, out var logFilePath))
            {
                throw new InvalidOperationException($"Log file path not found for test: {testName}");
            }

            using (var fileLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("TestName", testName)
                .WriteTo.File(logFilePath,
                    rollingInterval: RollingInterval.Infinite,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm} [{Level:u3}] ({ThreadId}) {ProcessId} {TestName} || {Message}{NewLine}{Exception}")
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.FromLogContext()
                .CreateLogger())
            {
                foreach (var logEvent in logEvents)
                {
                    if (logEvent.Properties.TryGetValue("TestName", out var testNameProperty) &&
                                            testNameProperty.ToString() == $"\"{testName}\"")  // Only write logs for the specific test
                    {
                        fileLogger.Write(logEvent);
                    }
                }
            }

            logFilePaths.TryRemove(testName, out _);  // Clean up after writing the log
        }

        public void Info(string testName, string message)
        {
            LogEventWithTestName(testName, LogEventLevel.Information, message);
        }

        public void Warn(string testName, string message)
        {
            LogEventWithTestName(testName, LogEventLevel.Warning, message);
        }

        public void Error(string testName, string message, Exception ex = null)
        {
            LogEventWithTestName(testName, LogEventLevel.Error, message, ex);
        }

        public void LogEventWithTestName(string testName, LogEventLevel level, string message, Exception ex = null)
        {
            using (LogContext.PushProperty("TestName", testName))  // Add TestName to the log context
            {
                if (ex != null)
                {
                    inMemoryLogger.Write(level, ex, message);
                }
                else
                {
                    inMemoryLogger.Write(level, message);
                }
            }
        }
    }
}
