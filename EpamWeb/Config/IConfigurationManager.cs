using static EpamWeb.Config.ConfigurationManager;

namespace EpamWeb.Config
{
    public interface IConfigurationManager
    {
        Configuration GetBrowserConfig();
    }
}
