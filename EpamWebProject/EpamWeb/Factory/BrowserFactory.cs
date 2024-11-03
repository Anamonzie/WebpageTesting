using EpamWeb.Config;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class BrowserFactory : IBrowserFactory
    {
        private static readonly Lazy<BrowserFactory> instance = new(() => new BrowserFactory(ConfigManager.Instance));
        private readonly IConfigManager configurationManager;
        private static readonly ThreadLocal<IBrowser> threadLocalBrowser = new();

        private BrowserFactory(IConfigManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public static BrowserFactory Instance => instance.Value;

        public async Task<IBrowser> GetBrowser()
        {
            if (threadLocalBrowser.Value == null)
            {
                var config = configurationManager.GetBrowserConfig();
                var browserType = config.BrowserSettings?.DefaultBrowser ?? "chrome";
                var playwright = await Playwright.CreateAsync();

                switch (browserType.ToLower())
                {
                    case "chrome":
                        threadLocalBrowser.Value = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                        {
                            Channel = "chrome",
                            Headless = true,
                            Timeout = 30000
                        });
                        break;
                    case "firefox":
                        threadLocalBrowser.Value = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                        {
                            Channel = "firefox",
                            Headless = true,
                            Timeout = 30000
                        });
                        break;
                    default:
                        throw new ArgumentException($"Browser type '{browserType} is not yet supported.'");
                }
            }

            return threadLocalBrowser.Value;
        }
    }
}