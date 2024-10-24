using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public class ConfigurationManager : IConfigurationManager
    {
        private static ConfigurationManager? instance;
        private readonly IConfigurationRoot configuration;

        // Private constructor ensures it can't be instantiated externally
        private ConfigurationManager()
        {
            configuration = LoadConfiguration();
        }

        // Singleton instance getter, returned as IConfigurationManager
        public static IConfigurationManager Instance => instance ??= new ConfigurationManager();

        private IConfigurationRoot LoadConfiguration()
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Config");
            Console.WriteLine($"Base path for configuration: {basePath}");

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        // Access the configuration
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
