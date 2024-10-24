using EpamWeb.Config;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class BrowserFactory : IBrowserFactory
    {
        private static readonly ThreadLocal<IBrowser> threadLocalBrowser = new();
        //private static BrowserFactory? instance;
        private readonly IConfigurationManager configurationManager;
        private static readonly Lazy<BrowserFactory> instance = new(() => new BrowserFactory(ConfigurationManager.Instance()));

        private BrowserFactory(IConfigurationManager configurationManager)
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
                            Headless = true
                        });
                        break;
                    case "firefox":
                        threadLocalBrowser.Value = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                        {
                            Channel = "firefox",
                            Headless = true
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
