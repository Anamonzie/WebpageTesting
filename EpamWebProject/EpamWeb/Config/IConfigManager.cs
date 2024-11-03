using Microsoft.Extensions.Configuration;
using static EpamWeb.Config.ConfigManager;

namespace EpamWeb.Config
{
    public interface IConfigManager
    {
        IConfigurationRoot GetFullConfiguration();
        Configuration GetBrowserConfig();
        IConfigurationSection GetSerilogConfig();
    }
}
