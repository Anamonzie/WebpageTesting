using EpamWeb.Services;

namespace EpamWeb.Factory
{
    public interface IServiceFactory
    {
        IHomepageService CreateHomepageService();
        IInsightsPageService CreateInsightsPageService();
    }
}