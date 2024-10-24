using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationRoot configuration;
        private static ConfigurationManager? instance;
        private static readonly object lockObject = new();

        private ConfigurationManager()
        {
            configuration = LoadConfiguration();
        }

        public static IConfigurationManager Instance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new ConfigurationManager();
                    }
                }
            }

            return instance;
        }

        private IConfigurationRoot LoadConfiguration()
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Config");
            Console.WriteLine($"Base path for configuration: {basePath}");

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public IConfigurationRoot GetFullConfiguration()
        {
            return configuration;
        }

        // Example method to get specific browser config
        public Configuration GetBrowserConfig()
        {
            var config = new Configuration();
            configuration.GetSection("BrowserSettings").Bind(config.BrowserSettings);
            return config;
        }

        public class Configuration
        {
            public BrowserSettings? BrowserSettings { get; set; } = new BrowserSettings();
        }

        public class BrowserSettings
        {
            public string? DefaultBrowser { get; set; }
        }
    }
}
