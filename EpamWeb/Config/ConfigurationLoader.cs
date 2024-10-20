using Microsoft.Extensions.Configuration;

namespace EpamWeb.Config
{
    public static class ConfigurationLoader
    {
        private static readonly Lazy<IConfigurationRoot> _configuration = new Lazy<IConfigurationRoot>(LoadConfiguration);

        private static IConfigurationRoot LoadConfiguration()
        {
            var absolutePath = @"C:\Users\PC\Desktop\EPAM\Internship\Code\WebpageTesting\EpamWebTests\Config";

            return new ConfigurationBuilder()
                .SetBasePath(absolutePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public static IConfigurationRoot GetConfiguration()
        {
            return _configuration.Value;
        }
    }
}
