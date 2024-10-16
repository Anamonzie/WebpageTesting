using System.Text.Json;

namespace EpamWeb.Factory
{
    public static class ConfigurationManager
    {
        public static async Task<Configuration> GetBrowserConfig()
        {
            using var reader = new StreamReader("appsettings.json");
            string fileContent = await reader.ReadToEndAsync();
            var config = JsonSerializer.Deserialize<Configuration>(fileContent);

            return config ?? new Configuration { BrowserSettings = new BrowserSettings { DefaultBrowser = "chrome" } };
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