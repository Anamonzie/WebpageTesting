using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public static class ConfigurationLoader
    {
        private static readonly Lazy<IConfigurationRoot> configuration = new(LoadConfiguration);

        private static IConfigurationRoot LoadConfiguration()
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Config");
            Console.WriteLine($"Base path for configuration: {basePath}");

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public static IConfigurationRoot GetConfiguration()
        {
            return configuration.Value;
        }
    }
}
