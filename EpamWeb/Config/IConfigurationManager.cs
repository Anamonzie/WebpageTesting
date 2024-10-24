using Microsoft.Extensions.Configuration;
using static EpamWeb.Config.ConfigurationManager;

namespace EpamWeb.Config
{
    public interface IConfigurationManager
    {
        IConfigurationRoot GetFullConfiguration();
        Configuration GetBrowserConfig();
    }
}
