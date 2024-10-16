using Microsoft.Playwright;
using System.Xml.Linq;

namespace EpamWeb.Factory
{
    public class BrowserFactory : IBrowserFactory
    {
        private static readonly ThreadLocal<IBrowser> threadLocalBrowser = new();
        private static BrowserFactory? instance;

        private BrowserFactory() { }

        public static BrowserFactory Instance => instance ??= new BrowserFactory();  

        public async Task<IBrowser> GetBrowser()
        {
            if (threadLocalBrowser.Value == null)
            {
                var config = await ConfigurationManager.GetBrowserConfig();
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
