using EpamWeb.PageServices;

namespace EpamWeb.Factory
{
    public interface IServiceFactory
    {
        IHomepageService CreateHomepageService();
        IInsightsPageService CreateInsightsPageService();
    }
}