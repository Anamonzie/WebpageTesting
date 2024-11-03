using EpamWeb.Config;
using Serilog;


namespace EpamWeb.Loggers
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly Lazy<LoggerManager> instance = new(() => new LoggerManager(ConfigManager.Instance));
        private readonly IConfigManager configManager;
        private readonly ILogger logger;
        //private string logPath;

        public static LoggerManager Instance => instance.Value;
        public ILogger Logger => logger;

        private LoggerManager(IConfigManager configManager)
        {
            this.configManager = configManager;
            logger = ConfigureLogger();
        }

        //public string SetLogPath(string logPath)
        //{
        //    this.logPath = logPath;

        //    var testName = TestContext.CurrentContext.Test.Name;
        //    var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", testName);

        //    if (!Directory.Exists(logDirectory))
        //    {
        //        Directory.CreateDirectory(logDirectory);
        //    }

        //    return logPath;
        //}

        public ILogger ConfigureLogger()
        {
            var serilogConfig = configManager.GetSerilogConfig();

            if (serilogConfig == null)
            {
                throw new InvalidOperationException("Serilog configuration is missing.");
            }

            //var testName = TestContext.CurrentContext.Test.Name;

            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            //var logFileName = $"./logs/log-.txt";
            //var currentLogDirectory = SetLogPath(logPath);

            var configuredLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                //.Enrich.WithProperty("TestName", TestContext.CurrentContext.Test.Name)
                .WriteTo.File((Path.Combine(logDirectory, "log-.txt")),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] ({ThreadId}) {ProcessId} || {Message}{NewLine}{Exception}")
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .CreateLogger();
            //.ReadFrom.Configuration(serilogConfig)
            //.CreateLogger();

            //Log.Logger = configuredLogger; // Set the global logger
            //configuredLogger.Information("Logger configured successfully."); // Test logging

            return configuredLogger;
        }

        public void Info(string message)
        {
            Logger.Information(message);
        }

        public void Warn(string message)
        {
            Logger.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            Logger.Error(ex, message);
        }

        public void CloseAndFlush()
        {
            Logger.Information("Closing and flushing logger");
            Log.CloseAndFlush();
        }
    }
}
