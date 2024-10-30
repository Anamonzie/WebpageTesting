using EpamWeb.Config;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class BrowserFactory : IBrowserFactory
    {
        private readonly IConfigurationManager configurationManager;
        private static readonly Lazy<BrowserFactory> instance = new(() => new BrowserFactory(ConfigurationManager.Instance()));

        private BrowserFactory(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public static BrowserFactory Instance => instance.Value;

        public async Task<IBrowser> GetBrowser()
        {
                var config = configurationManager.GetBrowserConfig();
                var browserType = config.BrowserSettings?.DefaultBrowser ?? "chrome";
                var playwright = await Playwright.CreateAsync();

            return browserType.ToLower() switch
            {
                "chrome" => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "chrome",
                    Headless = true,
                    Timeout = 30000
                }),
                "firefox" => await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "firefox",
                    Headless = true,
                    Timeout = 30000
                }),
                _ => throw new ArgumentException($"Browser type '{browserType} is not yet supported.'"),
            };
        }
    }
}
