using static Theory_inPractice.Config.ConfigurationManager;


namespace Theory_inPractice.Config
{
    public interface IConfigurationManager
    {
        Configuration GetBrowserConfig();
    }
}
