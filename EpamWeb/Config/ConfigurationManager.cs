using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public class ConfigurationManager : IConfigurationManager
    {
        private static readonly Lazy<IConfigurationRoot> configuration = new(LoadConfiguration);

        public ConfigurationManager() { }

        private static IConfigurationRoot LoadConfiguration()
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Config");
            Console.WriteLine($"Base path for configuration: {basePath}");

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public static IConfigurationRoot GetFullConfiguration()
        {
            return configuration.Value;
        }

        public Configuration GetBrowserConfig()
        {
            var config = new Configuration();
            GetFullConfiguration().GetSection("BrowserSettings").Bind(config.BrowserSettings);
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
