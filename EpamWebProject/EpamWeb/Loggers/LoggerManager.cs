using EpamWeb.Config;
using NUnit.Framework;
using Serilog;

namespace EpamWeb.Loggers
{
    public class LoggerManager : ILoggerManager
    {
        private readonly IConfigManager configManager;
        private readonly ILogger logger;
        public Guid UniqueId { get; } = Guid.NewGuid();

        public LoggerManager(IConfigManager configManager)
        {
            this.configManager = configManager;
            logger = ConfigureLogger(); // Initialize the logger for the first time
        }

        private ILogger ConfigureLogger()
        {
            var serilogConfig = configManager.GetSerilogConfig();
            if (serilogConfig == null)
            {
                throw new InvalidOperationException("Serilog configuration is missing.");
            }

            var testName = TestContext.CurrentContext.Test.Name;
            var testWorkerId = TestContext.CurrentContext.WorkerId;
            var uniqueId = Guid.NewGuid(); // Unique ID for each test instance
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", $"{testName}");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var logFilePath = Path.Combine(logDirectory, $"{testName}-log.txt");

            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFilePath,
                    rollingInterval: RollingInterval.Infinite,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm} [{Level:u3}] ({ThreadId}) {ProcessId} || {Message}{NewLine}{Exception}")
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
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
            Log.CloseAndFlush();
        }
    }
}
