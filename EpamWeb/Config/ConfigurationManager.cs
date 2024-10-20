using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Configuration GetBrowserConfig()
        {
            var config = new Configuration();

            configuration.GetSection("BrowserSettings").Bind(config.BrowserSettings);
            
            return config;
        }

        public class Configuration
        {
            public BrowserSettings? BrowserSettings { get; set; }
        }

        public class BrowserSettings
        {
            public string? DefaultBrowser { get; set; }
        }
    }
}
