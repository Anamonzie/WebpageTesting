using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public class ConfigManager : IConfigManager
    {
        private static readonly Lazy<ConfigManager> instance = new(() => new ConfigManager());
        private readonly IConfigurationRoot configuration;

        private ConfigManager()
        {
            configuration = LoadConfiguration();
        }
        public static ConfigManager Instance => instance.Value;

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

        public IConfigurationSection GetSerilogConfig()
        {
            var serilogSection = configuration.GetSection("Serilog");
            Console.WriteLine($"Serilog Section Found: {serilogSection.Exists()}"); // Check if section exists
            return serilogSection;
        }

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
