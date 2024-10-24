using Theory_inPractice.Config;
using Microsoft.Playwright;

namespace Theory_inPractice.BrowserFactory
{
    public class BrowserFactory : IBrowserFactory
    {
        private static readonly ThreadLocal<IBrowser> threadLocalBrowser = new();
        private static BrowserFactory? instance;
        private readonly IConfigurationManager _configurationManager;

        private BrowserFactory(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public static BrowserFactory Instance(IConfigurationManager configurationManager)
                    => instance ??= new BrowserFactory(configurationManager);

        public async Task<IBrowser> GetBrowser()
        {
            if (threadLocalBrowser.Value == null)
            {
                var config = _configurationManager.GetBrowserConfig();
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
