using EpamWeb.Config;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using Serilog;
using Serilog.Context;

namespace EpamWeb.Loggers
{
    public class LoggerManager : ILoggerManager
    {
        private readonly IConfigManager configManager;
        private readonly ILogger logger;

        public LoggerManager(IConfigManager configManager, string testName)
        {
            this.configManager = configManager;
            logger = ConfigureLogger(testName); // Initialize the logger for the first time
        }

        private ILogger ConfigureLogger(string testName)
        {
            //var testName = TestContext.CurrentContext.Test.Name;
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", $"{testName}");
            var serilogConfig = configManager.GetSerilogConfig();

            if (serilogConfig == null)
            {
                throw new InvalidOperationException("Serilog configuration is missing.");
            }

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var logFilePath = Path.Combine(logDirectory, $"{testName}-log.txt");
            LogContext.PushProperty("TestName", testName);

            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("TestName", testName)
                .WriteTo.File(logFilePath,
                    rollingInterval: RollingInterval.Infinite,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm} [{Level:u3}] ({ThreadId}) {ProcessId} {TestName} || {Message}{NewLine}{Exception}")
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public void Info(string message)
        {
            logger.Information(message);
        }

        public void Warn(string message)
        {
            logger.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            logger.Error(ex, message);
        }

        public void CloseAndFlush()
        {
            logger.Information("Closing and flushing logger");
            (logger as IDisposable)?.Dispose();
        }
    }
}
